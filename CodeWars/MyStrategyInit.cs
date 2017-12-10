﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk
{
    public partial class MyStrategy
    {
        public static int NewGroupMinSize;

        public static void Initialize()
        {
            var facilities = World.Facilities
               .Select(x => new AFacility(x))
               .OrderBy(x => x.Id)
               .ToArray();

            if (TerrainType == null)
            {
                TerrainType = World.TerrainByCellXY;
                WeatherType = World.WeatherByCellXY;
                FacilityIdx = new int[TerrainType.Length][];
                for (var i = 0; i < TerrainType.Length; i++)
                {
                    FacilityIdx[i] = new int[TerrainType[i].Length];
                    for (var j = 0; j < TerrainType[i].Length; j++)
                    {
                        FacilityIdx[i][j] = -1;
                        for (var k = 0; k < facilities.Length; k++)
                            if (facilities[k].ContainsPoint(new Point((i + 0.5) * G.CellSize, (j + 0.5) * G.CellSize)))
                                FacilityIdx[i][j] = k;
                    }
                }
            }

            var nuclears = World.Players
                .Where(player => player.NextNuclearStrikeVehicleId != -1)
                .Select(player => new ANuclear(
                    player.NextNuclearStrikeX,
                    player.NextNuclearStrikeY,
                    player.IsMe,
                    player.NextNuclearStrikeVehicleId,
                    player.NextNuclearStrikeTickIndex - World.TickIndex)
                )
                .ToArray();

            VehiclesObserver.Update();
            MoveObserver.Init();
            var prevEnv = Environment;
            Environment = new Sandbox(VehiclesObserver.VehicleById.Values, nuclears, facilities) { TickIndex = World.TickIndex };
            VehiclesObserver.Update2(prevEnv, Environment);
            OppClusters = Environment.GetClusters(false, Const.ClusteringMargin);

            NewGroupMinSize = (int)(Environment.MyVehicles.Count*44/500.0);

            var newVehicles = Environment.MyVehicles.Where(x => x.Groups == 0).ToArray();
            
            var ungroupedEnv = new Sandbox(
                newVehicles,
                new ANuclear[] { },
                new AFacility[] { },
                clone: true
                );

            MyUngroupedClusters = G.IsFacilitiesEnabled
                ? ungroupedEnv.GetClusters(true, Const.ClusteringMargin*2)
                : new List<VehiclesCluster>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WeatherType Weather(double x, double y)
        {
            int I, J;
            Utility.GetCell(x, y, out I, out J);
            return WeatherType[I][J];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TerrainType Terrain(double x, double y)
        {
            int I, J;
            Utility.GetCell(x, y, out I, out J);
            return TerrainType[I][J];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FacilityIndex(double x, double y)
        {
            int I, J;
            Utility.GetCell(x, y, out I, out J);
            return FacilityIdx[I][J];
        }
    }
}
