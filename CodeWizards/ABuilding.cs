﻿using System.Linq;
using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    public class ABuilding : ACombatUnit
    {
        public double Damage;
        public bool IsBesieded;
        public int OpponentsCount;
        public bool _isAssailable;

        public ABuilding(Building unit) : base(unit)
        {
            Damage = unit.Damage;
        }

        public ABuilding(ABuilding unit) : base(unit)
        {
            Damage = unit.Damage;
            IsBesieded = unit.IsBesieded;
            OpponentsCount = unit.OpponentsCount;
            _isAssailable = unit._isAssailable;
        }

        public override void EthalonMove(ACircularUnit target)
        {
            SkipTick();
        }

        public override bool EthalonCanHit(ACircularUnit target, bool checkCooldown = true)
        {
            return (!checkCooldown || RemainingActionCooldownTicks == 0) && 
                GetDistanceTo2(target) <= Geom.Sqr(CastRange);
        }

        public override ACombatUnit SelectTarget(ACombatUnit[] candidates)
        {
            var accessible = candidates
                .Where(x => x.Faction != Faction.Neutral && x.Faction != Faction && x.IsAlive && GetDistanceTo2(x) <= Geom.Sqr(CastRange))
                .ToArray();

            ACombatUnit sel = null;
			
			foreach (var x in accessible)
                if (x.Life > Damage)
                    if (sel == null || x.Life < sel.Life || Utility.Equals(x.Life, sel.Life) && x.Id == MyStrategy.Self.Id)
                        sel = x;
			
			if (sel != null)
				return sel;
			
            foreach (var x in accessible)
                if (x.Life <= Damage)
                    if (sel == null || x.Life > sel.Life || Utility.Equals(x.Life, sel.Life) && x.Id == MyStrategy.Self.Id)
                        sel = x;
                                    
            return sel;
        }

        public bool IsBase => Utility.Equals(MyStrategy.Game.FactionBaseRadius, Radius);

        public ALaneType Lane => RoadsHelper.GetLane(this);

        public int Order
        {
            get
            {
                if (IsBase)
                    return 2;

                if (BuildingsObserver.MyBase.Faction == Faction)
                    return BuildingsObserver.MyBase.GetDistanceTo2(this) < Geom.Sqr(Const.MapSize / 2 - Const.BaseBuildingDistance) ? 1 : 0;

                return BuildingsObserver.OpponentBase.GetDistanceTo2(this) < Geom.Sqr(Const.MapSize / 2 - Const.BaseBuildingDistance) ? 1 : 0;
            }
        }

        public override bool IsAssailable => _isAssailable;
    }
}
