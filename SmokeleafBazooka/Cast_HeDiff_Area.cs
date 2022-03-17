using RimWorld;
using Verse;
using System.Collections.Generic;
using System;

namespace MRP_SmokeleafBazooka
{
    internal class Cast_HeDiff_Area
    {
        private Map map;
        private int PawnAffectedCellCount;
        private IntVec3 position;
        private List<Tuple<HediffDef, float, float>> hediffDefsToApply;
        public Cast_HeDiff_Area(Map map, float radius, IntVec3 position) 
        {
            this.map = map;
            this.PawnAffectedCellCount = GenRadial.NumCellsInRadius(radius);
            this.position = position;
            hediffDefsToApply = new List<Tuple<HediffDef, float, float>>();
        }
        public void ApplyHediffToArea()
        {
            Explosion explosionSight = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, position, map);

            for (int i = 0; i < PawnAffectedCellCount; i++)
            {
                IntVec3 areaCoverage = explosionSight.Position + GenRadial.RadialPattern[i];
                if (!areaCoverage.InBounds(map))
                {
                    continue;
                }

                List<Thing> thingList = areaCoverage.GetThingList(map);
                for (int j = 0; j < thingList.Count; j++)
                {
                    Pawn pawn = thingList[j] as Pawn;
                    if (pawn != null && GenSight.LineOfSightToThing(pawn.Position, explosionSight, map))
                    {
                        applyHediffDefToPawn(pawn);
                    }
                }
            }
            explosionSight.Destroy();
        }
        public void AddHediff(HediffDef hediffDef,float severity, float chance)
        {
            hediffDefsToApply.Add(new Tuple<HediffDef, float, float>(hediffDef, severity, chance));
        }
        private void applyHediffDefToPawn(Pawn pawn)
        {
            foreach (Tuple<HediffDef, float, float> tuple in hediffDefsToApply)
            {
                float rand = Rand.Value;
                if (rand <= tuple.Item3)
                {
                    Hediff hediff = HediffMaker.MakeHediff(tuple.Item1, pawn);
                    hediff.Severity = tuple.Item2;
                    pawn.health.AddHediff(hediff);
                }
            }
        }
    }
}
