using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MRP_SmokeleafBazooka
{
    internal class Add_HeDiff_Area
    {
        private bool postDestroyHitThing = false;
        private Map map;
        private Thing hitThing;
        private int PawnNotifyCellCount;
        private List<Room> exploderOverlapRooms;
        private HediffDef hediffDef;
        private IntVec3 position;
        public Add_HeDiff_Area(Map map, float radius, HediffDef hediffDef, IntVec3 position, Thing hitThing = null) 
        {
            this.map = map;
            this.PawnNotifyCellCount = GenRadial.NumCellsInRadius(radius);
            this.exploderOverlapRooms = new List<Room>();
            this.hediffDef = hediffDef;
            this.position = position;
            this.hitThing = hitThing;
        }
        public void AddHeDiffToArea()
        {
            hitThing = checkNullThing();

         //   Log.Message("CHECKING PASSABILITY...");
         //   Log.Message(hitThing.def.label);
         //   if (hitThing.def.passability == Traversability.Impassable || hitThing.def.IsDoor)
           // {
                Log.Message("IMPASSIBLE - CALLING EDGE CELL LOOP...");
                foreach (IntVec3 edgeCell in hitThing.OccupiedRect().ExpandedBy(1).EdgeCells)
                {
                    Room room = edgeCell.GetRoom(map);
                    if (!exploderOverlapRooms.Contains(room))
                    {
                        exploderOverlapRooms.Add(room);
                    }
                }
          //  }
          //  else
         //   {
         //       exploderOverlapRooms.Add(hitThing.GetRoom());
         //   }

            Log.Message("CALL PAWN INTERATE");
            for (int i = 0; i < PawnNotifyCellCount; i++)
            {
                IntVec3 c = hitThing.Position + GenRadial.RadialPattern[i];
                if (!c.InBounds(map))
                {
                    continue;
                }

                List<Thing> thingList = c.GetThingList(map);
                for (int j = 0; j < thingList.Count; j++)
                {
                    Pawn pawn2 = thingList[j] as Pawn;
                    if (pawn2 != null)
                    {
                        Room room2 = pawn2.GetRoom();
                        Log.Message(room2.DebugString());
                        Log.Message("ROOM CONTAINS? " + exploderOverlapRooms.Contains(room2).ToString());
                        if (room2 == null || room2.CellCount == 1 || (exploderOverlapRooms.Contains(room2) && GenSight.LineOfSightToThing(pawn2.Position, hitThing, map, skipFirstCell: true)))
                        {
                            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn2);
                            hediff.Severity = 1;
                            pawn2.health.AddHediff(hediff);
                        }
                    }
                }
            }

            if (postDestroyHitThing)
            {
                hitThing.Destroy();
            }

        }
        Thing checkNullThing()
        {
            if (hitThing != null)
            {
                return hitThing;
            }

            postDestroyHitThing = true;
            Explosion explosionSight = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, position, map);
            explosionSight.EverSeenByPlayer = false; // TODO do we need this?
            hitThing = explosionSight;
            return hitThing;
        }
    }
}
