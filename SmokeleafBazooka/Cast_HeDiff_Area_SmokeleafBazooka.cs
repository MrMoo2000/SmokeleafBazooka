using RimWorld;
using Verse;
using System.Collections.Generic;
using System;

namespace MRP_SmokeleafBazooka
{
    internal class Cast_HeDiff_Area_SmokeleafBazooka : Cast_HeDiff_Area
    {
        public Cast_HeDiff_Area_SmokeleafBazooka(Map map, float radius, IntVec3 position) : base(map, radius, position)
        {
        }
        protected override void applyHediffDefToPawn(Pawn pawn)
        {
            Hediff pawnHasSmokeLeafAddiction = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("SmokeleafAddiction"));
            Hediff pawnHasSmokeLeafTolerance = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("SmokeleafTolerance"));
            foreach (Tuple<HediffDef, float, float> tuple in base.hediffDefsToApply)
            {
                float rand = Rand.Value;
                if (rand <= tuple.Item3)
                {
                    if ((pawnHasSmokeLeafAddiction != null || pawnHasSmokeLeafTolerance != null) && tuple.Item1.defName == "MRP_Concentrated_SmokeleafHigh")
                    {
                        continue;
                    }
                    Hediff hediff = HediffMaker.MakeHediff(tuple.Item1, pawn);
                    hediff.Severity = tuple.Item2;
                    pawn.health.AddHediff(hediff);
                }
            }
        }
    }
}
