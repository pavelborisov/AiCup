﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class PathPattern
    {
        public int From;
        public int To;
        public int Step;

        public AMove Move;
    }

    public enum TurnPatternType
    {
        ToNext,
        FromNext,
    }

    public class TurnPattern
    {
        public TurnPatternType Pattern;
    }

    public class PassedInfo
    {
        public BitMask Bonuses = new BitMask();
        public bool Slicks;
        public BitMask Projectiles = new BitMask();
        public bool Cars;
        public bool ExactlyBorder;
        public bool OutOfBoreder;
        public bool WayPoint; // HARD TO FIX: можно пропустить несколько вейпоинтов

        public int Time;
        public double Importance;

        public PassedInfo Clone()
        {
            return new PassedInfo
            {
                Bonuses = Bonuses.Clone(),
                Slicks = Slicks,
                Projectiles = Projectiles.Clone(),
                Cars = Cars,
                ExactlyBorder = ExactlyBorder,
                OutOfBoreder = OutOfBoreder,
                WayPoint = WayPoint,

                Time = Time,
                Importance = Importance
            };
        }
    }

    public class PathBruteForcer
    {
        public readonly PathPattern[] Patterns;
        public ACar Self;
        public bool Special;
        public AMove LastStageMove;
        public int LastSuccess; // Когда последний раз что-то находил

        private PathPattern[] _patterns;
        private Moves _movesStack, _bestMovesStack;
        private int _bestTime;
        private double _bestImportance;
        private Point[] _bruteWayPoints;

        private delegate void CarCallback(ACar car, PassedInfo passed);


        private Moves _cache;
        private int _lastCall; // Когда последний раз вызывали. Если не вызывали - значит был success

        private Point _turnTo, _turnTo2;
        private double _needDist, _needDist2;
        private readonly int _interval;
        private int _waypointsCount;
        private bool _useDist2;

        private ABonus[] _bonusCandidates;
        private AOilSlick[] _slickCandidates;
        private AProjectile[][] _projCandidates;
        private ACar[][] _carCandidates;

        private int _bonusesCount05;
        private int _bonusesCount2;

        public PathBruteForcer(PathPattern[] patterns, int interval, AMove lastStageMove, int waypointsCount, bool useDist2)
        {
            Patterns = patterns;
            _interval = interval;
            LastStageMove = lastStageMove;
            _waypointsCount = waypointsCount;
            _useDist2 = useDist2;
        }

        private void _doRecursive(ACar model, int patternIndex, PassedInfo total)
        {
            model = model.Clone();
            total = total.Clone();

            if (patternIndex == _patterns.Length)
            {
                var m = LastStageMove.Clone();
                m.WheelTurn = _turnTo.Clone();

                if (m.IsUseNitro && (model.Speed.Length > 22 || m.EnginePower < 0))
                    return;

                var penalty = 0.0;

                for (var i = 0; ; i++)
                {
                    if (i == 200 && m.EnginePower >= 0 || i == 250 && m.EnginePower < 0)
                        return;

                    var dst = _turnTo.GetDistanceTo2(model);
                    if (dst < _needDist*_needDist)
                        break;

                    if (!_modelMove(model, m, total))
                    {
                        if (_useDist2 && m.EnginePower <= 1 && !LastStageMove.IsUseNitro)
                        {
                            if (dst < _needDist2 * _needDist2)
                            {
                                penalty = MagicConst.SecondDistDangerCoeff;
                                total.Importance -= penalty;
                                m.Times++;
                                break;
                            }
                        }
                        return;
                    }

                    m.Times++;
                }
                if (!total.WayPoint)
                    return;
                if (!MyStrategy.CheckVisibility(Self.Original, model, penalty > 0 ? _turnTo2 : _turnTo, 20))
                    return;

                if (model.EnginePower < 0)
                {
                    penalty += MagicConst.BackMoveDangerCoeff;
                    total.Importance -= MagicConst.BackMoveDangerCoeff;
                }

                if (total.Time - total.Importance < _bestTime - _bestImportance)
                {
                    _bestTime = total.Time;
                    _bestImportance = total.Importance;
                    _bestMovesStack = _movesStack.Clone();
                    _bestMovesStack.Add(m);
                    _bestMovesStack.Penalty = penalty;
                }
                return;
            }

            var pattern = _patterns[patternIndex];

            _carMoveFunc(model, pattern.From, pattern.To, pattern.Step,
                pattern.Move.Clone(), total, (aCar, passed) =>
                {
                    // ReSharper disable once ConvertToLambdaExpression
                    _doRecursive(aCar.Clone(), patternIndex + 1, passed.Clone());
                });
        }

        private void _validateLastSuccessStack()
        {
            if (_lastSuccessStack == null)
                return;
            var car = new ACar(Self);
            var stack = _lastSuccessStack.Clone();
            var info = new PassedInfo();
            while (stack.Count > 0)
            {
                if (!AMove.ModelMove(car, stack[0], info, 
                    new ABonus[] {}, new AOilSlick[] {}, new AProjectile[][] {}, new ACar[][] {}))
                {
                    _lastSuccessStack = null;
                    return;
                }
                stack[0].Times--;
                stack.Normalize();
            }
        }

        private Moves _lastSuccessStack;

        private int _selectThisTick;
        public int SelectedCount;

        public void SelectThis()
        {
            SelectedCount++;
            _selectThisTick = MyStrategy.world.Tick;
        }

        private static int _turnsCount(IList<Point> pts)
        {
            var cnt = 0;
            for (var i = 2; i < pts.Count; i++)
            {
                if (Math.Abs(Geom.VectorProduct(pts[i - 2], pts[i - 1], pts[i])) > MyStrategy.Eps)
                    cnt++;
            }
            return cnt;
        }

        public Moves Do(ACar car, Points pts)
        {
            // Проверка что данный путь был выбран
            if (_selectThisTick + 1 != MyStrategy.world.Tick)
                _lastSuccessStack = null;

            Self = car.Clone();

            if (_lastCall == LastSuccess)
                LastSuccess = _lastCall;

            for (var t = 0; t < MyStrategy.world.Tick - _lastCall && _lastSuccessStack != null && _lastSuccessStack.Count > 0; t++)
            {
                _lastSuccessStack[0].Times--;
                _lastSuccessStack.Normalize();
            }
            if (_lastSuccessStack != null && (_lastSuccessStack.Count == 0 || _useDist2 && _lastSuccessStack.ComputeTime() < 30))
                _lastSuccessStack = null;

            _lastCall = MyStrategy.world.Tick;

            /*
             * Количество бонусов на расстоянии 0.5t
             * Если изменилось - пересчитывать сильно
             */
            var bonusesCount05 = MyStrategy.Bonuses
                .Count(bonus => Self.GetDistanceTo(bonus) < Const.TileSize / 2);

            /*
             * Количество бонусов на расстоянии 2t
             * Если изменилось - чуть нужно пересчитать
             */
            var bonusesCount2 = MyStrategy.Bonuses
                .Count(
                    bonus =>
                        Self.GetDistanceTo(bonus) < Const.TileSize*2 &&
                        MyStrategy.CellDistance(Self, bonus) <= 2);

            // Если был success на прошлом тике, то продолжаем. Или каждые _interval тиков.
            if (Const.Game.InitialFreezeDurationTicks < MyStrategy.world.Tick &&
                bonusesCount05 == _bonusesCount05 &&
                LastSuccess < MyStrategy.world.Tick - 1 &&
                (MyStrategy.world.Tick - (LastSuccess + 1))%_interval != 0)
            {
                _validateLastSuccessStack();
                return _lastSuccessStack;
            }

            /*
             * Смотрим на шины, которые на расстоянии не более 6 тайлов
             */
            var prevProj = _projCandidates;
            _projCandidates = MyStrategy.Tires
                .Where(
                    proj =>
                        Self.GetDistanceTo(proj[0]) <= Const.TileSize * 6 &&
                        MyStrategy.CellDistance(Self, proj[0]) <= 6)
                .ToArray();

            var extended = MyStrategy.ExtendWaySegments(pts, 50);
            _bruteWayPoints = extended.GetRange(0, Math.Min(_waypointsCount, extended.Count)).ToArray();
            if (LastStageMove.IsUseNitro && _turnsCount(_bruteWayPoints) > 1)
                return null;
#if DEBUG
            var bruteWayPoints = new Points();
            bruteWayPoints.AddRange(_bruteWayPoints);
            Visualizer.SegmentsDrawQueue.Add(new object[]{ Brushes.Brown, bruteWayPoints, 0.0 });
#endif
            _needDist = Const.TileSize*0.5 - 3;
            _needDist2 = Const.TileSize - 3;
            _turnTo = _bruteWayPoints[_bruteWayPoints.Length - 1];
            _turnTo2 = _bruteWayPoints[Math.Min(_bruteWayPoints.Length - 1, (int)(_bruteWayPoints.Length * 0.83))];
#if DEBUG
            Visualizer.CircleFillQueue.Add(new Tuple<Brush, ACircularUnit>(Brushes.OrangeRed, new ACircularUnit { X = _turnTo.X, Y = _turnTo.Y, Radius = 20}));
            Visualizer.CircleFillQueue.Add(new Tuple<Brush, ACircularUnit>(Brushes.Orange, new ACircularUnit { X = _turnTo2.X, Y = _turnTo2.Y, Radius = 20 }));
#endif

            _patterns = Patterns.Select(pt => new PathPattern
            {
                From = pt.From,
                To = pt.To,
                Step = pt.Step,
                Move = pt.Move.Clone()
            }).ToArray();
            foreach (var p in _patterns)
            {
                if (p.Move.WheelTurn is TurnPattern)
                {
                    var turnPattern = p.Move.WheelTurn as TurnPattern;
                    if (turnPattern.Pattern == TurnPatternType.ToNext)
                        p.Move.WheelTurn = Self.GetAngleTo(_turnTo) < 0 ? -1 : 1;
                    else if (turnPattern.Pattern == TurnPatternType.FromNext)
                        p.Move.WheelTurn = Self.GetAngleTo(_turnTo) < 0 ? 1 : -1;
                }
            }

            _movesStack = new Moves();
            _bestMovesStack = new Moves();
            _bestTime = MyStrategy.Infinity;
            _bestImportance = 0;

            /*
             * Смотрим на бонусы, которые на расстоянии не более 4t
             * TODO: уменьшить приоритет бонусов, которые может быть возьмет другой (в.т.ч тиммейт)
             */
            _bonusCandidates = MyStrategy.Bonuses
                .Where(
                    bonus =>
                        MyStrategy.world.Tick > 270 && // Не смотреть на бонусы при старте!!!
                        Self.GetDistanceTo(bonus) <= Const.TileSize * 4 &&
                        MyStrategy.CellDistance(Self, bonus) <= 4
                )
                .ToArray();


            /*
             * Смотрим на лужи, которые на расстоянии не более 5 тайлов
             */
            var prevSlicks = _slickCandidates;
            _slickCandidates = MyStrategy.OilSlicks
                .Where(
                    slick =>
                        Self.GetDistanceTo(slick) <= Const.TileSize*5 &&
                        MyStrategy.CellDistance(Self, slick) <= 5
                )
                .ToArray();

            /*
             * Пытаться объехать тех, которые
             * - Крашнулись
             * - Убиты
             * - Двигатель меньше чем на 0.5 мощности
             * - Двигаются по встречной
             * 
             * - Если у меня нитро, или будет нитро
             * 
             * - Своих
             */
            var prevCars = _carCandidates;
            _carCandidates = MyStrategy.Others
                .Where(opp => opp[0].GetDistanceTo(Self) < Const.TileSize*9)
                .Where(
                    opp =>
                        opp[0].Original.IsTeammate ||
                        MyStrategy.IsCrashed(opp[0].Original) ||
                        !DurabilityObserver.IsActive(opp[0].Original) ||
                        opp[0].EnginePower < 0.5 ||
                        Self.RemainingNitroTicks > 0 ||
                        Math.Abs(Geom.GetAngleBetween(Self.Speed, opp[0].Speed)) > Math.PI / 2
                )
                .Where(opp => MyStrategy.CellDistance(Self, opp[0]) <= 9) // 9 - потому что он может ехать по встречке
                .ToArray();


            if (_cache != null)
            {
                for (var k = 0; k < _patterns.Length; k++)
                {
                    var range = (prevSlicks == null || prevCars == null || prevProj == null
                        || _bonusesCount2 != bonusesCount2 
                        || prevSlicks.Length != _slickCandidates.Length
                        || prevCars.Length != _carCandidates.Length
                        || prevProj.Length != _projCandidates.Length)
                        ? (k == 0 ? 6 : 4)
                        : (k == 0 ? 6 : 2);

                    if (_bonusesCount05 != bonusesCount05 || Special && k == 0)
                        range = 10;
                    
                    _patterns[k].From = Math.Max(0, _cache[k].Times - range);
                    _patterns[k].To = Math.Min(_patterns[k].To * 9 / 7, _cache[k].Times + range);
                    _patterns[k].Step = 2;
                }
            }

            _bonusesCount05 = bonusesCount05;
            _bonusesCount2 = bonusesCount2;   

            var wayPointRequired = false;
            for(var i = _bruteWayPoints.Length - 1; i >= 0; i--)
            {
                if (_bruteWayPoints[i].GetDistanceTo2(_turnTo) < _needDist*_needDist)
                {
                    for (var j = 0; j < i; j++)
                        wayPointRequired |=
                            MyStrategy.GetNextWayPoint(Self.Original).Equals(MyStrategy.GetCell(_bruteWayPoints[j]));
                    break;
                }
            }

            _doRecursive(Self, 0, new PassedInfo { WayPoint = !wayPointRequired });
            _cache = null;
            if (_bestTime == MyStrategy.Infinity)
                return _lastSuccessStack;

            if (_bestMovesStack.ComputeTime() != _bestTime)
                throw new Exception("ComputeTime != BestTime");

            LastSuccess = MyStrategy.world.Tick;
            _cache = _bestMovesStack.Clone();

            if (_maxTicksInfo == null) 
                _maxTicksInfo = new int[_bestMovesStack.Count];
            for (var i = 0; i < _maxTicksInfo.Length; i++)
                _maxTicksInfo[i] = Math.Max(_maxTicksInfo[i], _bestMovesStack[i].Times);

            _bestMovesStack.Normalize();
            _lastSuccessStack = _bestMovesStack.Clone();
            return _bestMovesStack;
        }

        private int[] _maxTicksInfo;

        public int[] GetMaxTicksInfo()
        {
            return _maxTicksInfo;
        }

        private void _carMoveFunc(ACar model, int from, int to, int step, AMove m, PassedInfo passed, CarCallback callback)
        {
            model = model.Clone();
            passed = passed.Clone();
            m.Times = 0;

            for (var t = 0; t < from; t++)
            {
                if (!_modelMove(model, m, passed))
                    return;
                m.Times++;
            }

            for (var t = from; t <= to; t += step)
            {
                _movesStack.Add(m);
                callback(model, passed);
                _movesStack.Pop();
                for (var r = 0; r < step; r++)
                {
                    if (!_modelMove(model, m, passed))
                        return;
                    m.Times++;
                }
            }
        }

        public bool _modelMove(ACar car, AMove m, PassedInfo total)
        {
            return AMove.ModelMove(car, m, total, 
                _bonusCandidates, _slickCandidates, _projCandidates, _carCandidates);
        }
    }
}
