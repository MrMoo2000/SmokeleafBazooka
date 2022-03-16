using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MRP_SmokeleafBazooka
{
    internal class Projectile_Bullet_SmokeleafBazooka : Projectile
    {
        //private const int ExtraExplosionRadius = 2;
        //public override bool AnimalsFleeImpact => true;
        private bool destroyHitThing = false;
        private Map map;
        protected override void Impact(Thing hitThing)
        {
            Log.Message("Bazooka Impact Called");
            map = base.Map;
            int PawnNotifyCellCount = GenRadial.NumCellsInRadius(base.def.projectile.explosionRadius);
            List<Room> exploderOverlapRooms = new List<Room>();

            base.Impact(hitThing);
            GenExplosion.DoExplosion(base.Position, map, base.def.projectile.explosionRadius, SmokeleafBazookaDefs.MRP_Concentrated_Smokeleaf, base.launcher, base.DamageAmount, base.ArmorPenetration, null, base.equipmentDef, base.def, postExplosionSpawnThingDef: ThingDefOf.Filth_Ash, intendedTarget: intendedTarget.Thing, postExplosionSpawnChance: 0.2f, postExplosionSpawnThingCount: 1, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0f, preExplosionSpawnThingCount: 1, chanceToStartFire: 0f);

            Add_HeDiff_Area addHeDiff = new Add_HeDiff_Area(map, base.def.projectile.explosionRadius, HediffDef.Named("SmokeleafHigh"), base.Position, hitThing);
            addHeDiff.AddHeDiffToArea();
            /*
            hitThing = checkNullThing(hitThing);
            Log.Message("CHECKING PASSABILITY...");
            if (hitThing.def.passability == Traversability.Impassable || hitThing.def.IsDoor )
            {
                Log.Message("IMPASSIBLE - CALLING EDGE CELL LOOP...");
                foreach (IntVec3 edgeCell in hitThing.OccupiedRect().ExpandedBy(1).EdgeCells)
                {
                    Room room = edgeCell.GetRoom(map);
                    if (!exploderOverlapRooms.Contains(room))
                    {
                        exploderOverlapRooms.Add(room);
                    }
                }
            }
            else
            {
                exploderOverlapRooms.Add(hitThing.GetRoom());
            }

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
                            Hediff hediff = HediffMaker.MakeHediff(HediffDef.Named("SmokeleafHigh"), pawn2); 
                            hediff.Severity = 1;

                            pawn2.health.AddHediff(hediff);
                        }
                    }
                }
            }

            Log.Message("POST ITERATE");
            if(destroyHitThing)
            {
                Log.Message("CALLING DESTROY");
                hitThing.Destroy();
            }
            */
        }

        // Create a thing if hitThing is null 
        Thing checkNullThing(Thing hitThing)
        {
            if(hitThing != null)
            {
                return hitThing;
            }
            
            destroyHitThing = true;
            Explosion explosionSight = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, base.Position, map);
            explosionSight.EverSeenByPlayer = false; // TODO do we need this?
            hitThing = explosionSight;
            return hitThing;
        }
    }
}
