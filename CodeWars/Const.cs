﻿using System;
using Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk
{
    class Const
    {
        public static double Eps = 1e-9;
        public static int Infinity = 0x3f3f3f3f;
        public static double MapSize;

        public static void Initialize(World world, Game game)
        {
            MapSize = world.Width; // Width == Height

            _expect(game.ArrvDurability, G.MaxDurability);
            _expect(game.FighterDurability, G.MaxDurability);
            _expect(game.HelicopterDurability, G.MaxDurability);
            _expect(game.IfvDurability, G.MaxDurability);
            _expect(game.TankDurability, G.MaxDurability);

            _expect(game.CloudWeatherSpeedFactor, G.CloudWeatherSpeedFactor);
            _expect(game.RainWeatherSpeedFactor, G.RainWeatherSpeedFactor);
            _expect(game.SwampTerrainSpeedFactor, G.SwampTerrainSpeedFactor);
            _expect(game.ForestTerrainSpeedFactor, G.ForestTerrainSpeedFactor);

            const int
                arrv = (int) VehicleType.Arrv,
                fighter = (int) VehicleType.Fighter,
                helicopter = (int) VehicleType.Helicopter,
                ifv = (int) VehicleType.Ifv,
                tank = (int) VehicleType.Tank;

            _expect(Math.Max(0, game.FighterGroundDamage - game.ArrvAerialDefence), G.AttackDamage[fighter, arrv]);
            _expect(Math.Max(0, game.FighterAerialDamage - game.FighterAerialDefence), G.AttackDamage[fighter, fighter]);
            _expect(Math.Max(0, game.FighterAerialDamage - game.HelicopterAerialDefence), G.AttackDamage[fighter, helicopter]);
            _expect(Math.Max(0, game.FighterGroundDamage - game.IfvAerialDefence), G.AttackDamage[fighter, ifv]);
            _expect(Math.Max(0, game.FighterGroundDamage - game.TankAerialDefence), G.AttackDamage[fighter, tank]);

            _expect(Math.Max(0, game.HelicopterGroundDamage - game.ArrvAerialDefence), G.AttackDamage[helicopter, arrv]);
            _expect(Math.Max(0, game.HelicopterAerialDamage - game.FighterAerialDefence), G.AttackDamage[helicopter, fighter]);
            _expect(Math.Max(0, game.HelicopterAerialDamage - game.HelicopterAerialDefence), G.AttackDamage[helicopter, helicopter]);
            _expect(Math.Max(0, game.HelicopterGroundDamage - game.IfvAerialDefence), G.AttackDamage[helicopter, ifv]);
            _expect(Math.Max(0, game.HelicopterGroundDamage - game.TankAerialDefence), G.AttackDamage[helicopter, tank]);

            _expect(Math.Max(0, game.IfvGroundDamage - game.ArrvGroundDefence), G.AttackDamage[ifv, arrv]);
            _expect(Math.Max(0, game.IfvAerialDamage - game.FighterGroundDefence), G.AttackDamage[ifv, fighter]);
            _expect(Math.Max(0, game.IfvAerialDamage - game.HelicopterGroundDefence), G.AttackDamage[ifv, helicopter]);
            _expect(Math.Max(0, game.IfvGroundDamage - game.IfvGroundDefence), G.AttackDamage[ifv, ifv]);
            _expect(Math.Max(0, game.IfvGroundDamage - game.TankGroundDefence), G.AttackDamage[ifv, tank]);

            _expect(Math.Max(0, game.TankGroundDamage - game.ArrvGroundDefence), G.AttackDamage[tank, arrv]);
            _expect(Math.Max(0, game.TankAerialDamage - game.FighterGroundDefence), G.AttackDamage[tank, fighter]);
            _expect(Math.Max(0, game.TankAerialDamage - game.HelicopterGroundDefence), G.AttackDamage[tank, helicopter]);
            _expect(Math.Max(0, game.TankGroundDamage - game.IfvGroundDefence), G.AttackDamage[tank, ifv]);
            _expect(Math.Max(0, game.TankGroundDamage - game.TankGroundDefence), G.AttackDamage[tank, tank]);

            _expect(game.FighterGroundAttackRange, G.AttackRange[fighter, arrv]);
            _expect(game.FighterAerialAttackRange, G.AttackRange[fighter, fighter]);
            _expect(game.FighterAerialAttackRange, G.AttackRange[fighter, helicopter]);
            _expect(game.FighterGroundAttackRange, G.AttackRange[fighter, ifv]);
            _expect(game.FighterGroundAttackRange, G.AttackRange[fighter, tank]);

            _expect(game.HelicopterGroundAttackRange, G.AttackRange[helicopter, arrv]);
            _expect(game.HelicopterAerialAttackRange, G.AttackRange[helicopter, fighter]);
            _expect(game.HelicopterAerialAttackRange, G.AttackRange[helicopter, helicopter]);
            _expect(game.HelicopterGroundAttackRange, G.AttackRange[helicopter, ifv]);
            _expect(game.HelicopterGroundAttackRange, G.AttackRange[helicopter, tank]);

            _expect(game.IfvGroundAttackRange, G.AttackRange[ifv, arrv]);
            _expect(game.IfvAerialAttackRange, G.AttackRange[ifv, fighter]);
            _expect(game.IfvAerialAttackRange, G.AttackRange[ifv, helicopter]);
            _expect(game.IfvGroundAttackRange, G.AttackRange[ifv, ifv]);
            _expect(game.IfvGroundAttackRange, G.AttackRange[ifv, tank]);

            _expect(game.TankGroundAttackRange, G.AttackRange[tank, arrv]);
            _expect(game.TankAerialAttackRange, G.AttackRange[tank, fighter]);
            _expect(game.TankAerialAttackRange, G.AttackRange[tank, helicopter]);
            _expect(game.TankGroundAttackRange, G.AttackRange[tank, ifv]);
            _expect(game.TankGroundAttackRange, G.AttackRange[tank, tank]);

            _expect(game.FighterAttackCooldownTicks, G.AttackCooldownTicks);
            _expect(game.HelicopterAttackCooldownTicks, G.AttackCooldownTicks);
            _expect(game.IfvAttackCooldownTicks, G.AttackCooldownTicks);
            _expect(game.TankAttackCooldownTicks, G.AttackCooldownTicks);

            _expect(game.VehicleRadius, G.VehicleRadius);
        }

        private static void _expect<T>(T source, T value)
        {
            if (!value.Equals(source))
                throw new Exception("Expected " + source + " to equal " + value);
        }
    }

    class G
    {
        public static readonly double CellSize = 32;
        public static readonly double MaxDurability = 100;
        public static readonly double CloudWeatherSpeedFactor = 0.8;
        public static readonly double RainWeatherSpeedFactor = 0.6;
        public static readonly double SwampTerrainSpeedFactor = 0.6;
        public static readonly double ForestTerrainSpeedFactor = 0.8;
        public static readonly int AttackCooldownTicks = 60;
        public static readonly double VehicleRadius = 2;

        public static readonly int[,] AttackDamage = new int[5, 5]
        {
            {0, 0, 0, 0, 0},
            {0, 30, 60, 0, 0},
            {80, 10, 40, 20, 40},
            {40, 10, 40, 30, 10},
            {50, 0, 20, 40, 20}
        };

        public static readonly int[,] AttackRange = new int[5, 5]
        {
            {0, 0, 0, 0, 0},
            {0, 20, 20, 0, 0},
            {20, 18, 18, 20, 20},
            {18, 20, 20, 18, 18},
            {20, 18, 18, 20, 20}
        };

        public static int AttackRange2(VehicleType type1, VehicleType type2)
        {
            var r = AttackRange[(int) type1, (int) type2];
            return r*r;
        }
    }


}