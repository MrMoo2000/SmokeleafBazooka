using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MRP_SmokeleafBazooka
{
    internal class Projectile_Bullet_SmokeleafBazooka : Projectile
    {
        protected override void Impact(Thing hitThing)
        {
            Log.Message("Bazooka Impact Called");

            Map map = base.Map;

            base.Impact(hitThing);
            GenExplosion.DoExplosion(base.Position, map, base.def.projectile.explosionRadius, DamageDefOf.Smoke, base.launcher, base.DamageAmount, base.ArmorPenetration, null, base.equipmentDef, base.def, postExplosionSpawnThingDef: ThingDefOf.Filth_Ash, intendedTarget: intendedTarget.Thing, postExplosionSpawnChance: 0.2f, postExplosionSpawnThingCount: 1, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0f, preExplosionSpawnThingCount: 1, chanceToStartFire: 0f);
            
            GenExplosion.DoExplosion(base.Position, map, 0.5f, DamageDefOf.Bomb, base.launcher, 25, base.ArmorPenetration, null, base.equipmentDef, base.def, postExplosionSpawnThingDef: ThingDefOf.Filth_Ash, intendedTarget: intendedTarget.Thing, postExplosionSpawnChance: 0.2f, postExplosionSpawnThingCount: 1, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0f, preExplosionSpawnThingCount: 1, chanceToStartFire: 0f);

            Cast_HeDiff_Area castHeDiffArea = new Cast_HeDiff_Area(map, base.def.projectile.explosionRadius, base.Position);
            castHeDiffArea.AddHediff(HediffDef.Named("MRP_Concentrated_SmokeleafHigh"), 1f, 1f);
            castHeDiffArea.AddHediff(HediffDef.Named("SmokeleafHigh"), 1f, 1f);
            castHeDiffArea.AddHediff(HediffDef.Named("SmokeleafAddiction"), 1f, 0.15f); 
            castHeDiffArea.ApplyHediffToArea();
        }
    }
}
