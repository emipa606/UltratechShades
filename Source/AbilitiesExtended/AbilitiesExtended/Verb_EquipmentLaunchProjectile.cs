using System.Linq;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class Verb_EquipmentLaunchProjectile : Verb_UseEquipment
{
    protected virtual ThingDef Projectile
    {
        get
        {
            var comp = EquipmentSource?.GetComp<CompChangeableProjectile>();
            return comp is { Loaded: true } ? comp.Projectile : verbProps.defaultProjectile;
        }
    }

    public override bool TryCastShot()
    {
        if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map)
        {
            return false;
        }

        var projectile = Projectile;
        if (projectile == null)
        {
            return false;
        }

        var foundShootLine = TryFindShootLineFromTo(caster.Position, currentTarget, out var resultingLine);
        if (verbProps.stopBurstWithoutLos && !foundShootLine)
        {
            return false;
        }

        if (EquipmentSource != null)
        {
            EquipmentSource.TryGetCompFast<CompChangeableProjectile>()?.Notify_ProjectileLaunched();
            EquipmentSource.TryGetCompFast<CompApparelReloadable>()?.UsedOnce();
        }

        var manningPawn = caster;
        Thing equipmentSource = EquipmentSource;
        var compMannable = caster.TryGetCompFast<CompMannable>();
        if (compMannable is { ManningPawn: not null })
        {
            manningPawn = compMannable.ManningPawn;
            equipmentSource = caster;
        }

        var drawPos = caster.DrawPos;
        var projectile2 = (Projectile)GenSpawn.Spawn(projectile, resultingLine.Source, caster.Map);
        if (verbProps.ForcedMissRadius > 0.5f)
        {
            var num = VerbUtility.CalculateAdjustedForcedMiss(verbProps.ForcedMissRadius,
                currentTarget.Cell - caster.Position);
            if (num > 0.5f)
            {
                var max = GenRadial.NumCellsInRadius(num);
                Rand.PushState();
                var num2 = Rand.Range(0, max);
                Rand.PopState();
                if (num2 > 0)
                {
                    var intVec = currentTarget.Cell + GenRadial.RadialPattern[num2];
                    throwDebugText("ToRadius");
                    throwDebugText("Rad\nDest", intVec);
                    var projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
                    Rand.PushState();
                    if (Rand.Chance(0.5f))
                    {
                        projectileHitFlags = ProjectileHitFlags.All;
                    }

                    Rand.PopState();
                    if (!canHitNonTargetPawnsNow)
                    {
                        projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
                    }

                    projectile2.Launch(manningPawn, drawPos, intVec, currentTarget, projectileHitFlags,
                        preventFriendlyFire, equipmentSource);
                    return true;
                }
            }
        }

        var shotReport = ShotReport.HitReportFor(caster, this, currentTarget);
        var randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
        var targetCoverDef = randomCoverToMissInto?.def;
        if (!Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
        {
            resultingLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget, false, caster.Map);
            throwDebugText($"ToWild{(canHitNonTargetPawnsNow ? "\nchntp" : "")}");
            throwDebugText("Wild\nDest", resultingLine.Dest);
            var projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
            Rand.PushState();
            if (Rand.Chance(0.5f) && canHitNonTargetPawnsNow)
            {
                projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
            }

            Rand.PopState();
            projectile2.Launch(manningPawn, drawPos, resultingLine.Dest, currentTarget, projectileHitFlags2,
                preventFriendlyFire, equipmentSource, targetCoverDef);
            return true;
        }

        Rand.PushState();
        var flag2 = !Rand.Chance(shotReport.PassCoverChance);
        Rand.PopState();
        if (currentTarget.Thing != null && currentTarget.Thing.def.category == ThingCategory.Pawn && flag2)
        {
            throwDebugText($"ToCover{(canHitNonTargetPawnsNow ? "\nchntp" : "")}");
            if (randomCoverToMissInto == null)
            {
                return true;
            }

            throwDebugText("Cover\nDest", randomCoverToMissInto.Position);
            var projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
            if (canHitNonTargetPawnsNow)
            {
                projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
            }

            projectile2.Launch(manningPawn, drawPos, randomCoverToMissInto, currentTarget, projectileHitFlags3,
                preventFriendlyFire, equipmentSource, targetCoverDef);

            return true;
        }

        var projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
        if (canHitNonTargetPawnsNow)
        {
            projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
        }

        if (currentTarget.Thing != null &&
            (!currentTarget.HasThing || currentTarget.Thing.def.Fillage == FillCategory.Full))
        {
            projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
        }

        throwDebugText($"ToHit{(canHitNonTargetPawnsNow ? "\nchntp" : "")}");
        if (currentTarget.Thing != null)
        {
            projectile2.Launch(manningPawn, drawPos, currentTarget, currentTarget, projectileHitFlags4,
                preventFriendlyFire, equipmentSource, targetCoverDef);
            throwDebugText("Hit\nDest", currentTarget.Cell);
        }
        else
        {
            projectile2.Launch(manningPawn, drawPos, resultingLine.Dest, currentTarget, projectileHitFlags4,
                preventFriendlyFire, equipmentSource, targetCoverDef);
            throwDebugText("Hit\nDest", resultingLine.Dest);
        }

        return true;
    }

    private void throwDebugText(string text)
    {
        if (DebugViewSettings.drawShooting)
        {
            MoteMaker.ThrowText(caster.DrawPos, caster.Map, text);
        }
    }

    private void throwDebugText(string text, IntVec3 c)
    {
        if (DebugViewSettings.drawShooting)
        {
            MoteMaker.ThrowText(c.ToVector3Shifted(), caster.Map, text);
        }
    }

    public override void DrawHighlight(LocalTargetInfo target)
    {
        var def = ability.def;
        DrawHighlightFieldRadiusAroundTarget(target);
        if (CanHitTarget(target) && IsApplicableTo(target))
        {
            if (def.HasAreaOfEffect)
            {
                if (target.IsValid)
                {
                    GenDraw.DrawTargetHighlight(target);
                    GenDraw.DrawRadiusRing(target.Cell, def.EffectRadius, RadiusHighlightColor);
                }
            }
            else
            {
                GenDraw.DrawTargetHighlight(target);
            }
        }

        if (target.IsValid)
        {
            ability.DrawEffectPreviews(target);
        }

        verbProps.DrawRadiusRing(caster.Position);
        if (!target.IsValid)
        {
            return;
        }

        GenDraw.DrawTargetHighlight(target);
        var num = HighlightFieldRadiusAroundTarget(out var needLOSToCenter);
        if (!(num > 0.2f) || !TryFindShootLineFromTo(caster.Position, target, out var resultingLine))
        {
            return;
        }

        if (needLOSToCenter)
        {
            GenExplosion.RenderPredictedAreaOfEffect(resultingLine.Dest, num, verbProps.explosionRadiusRingColor);
            return;
        }

        GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(resultingLine.Dest, num, true)
            where x.InBounds(Find.CurrentMap)
            select x).ToList());
    }

    public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
    {
        needLOSToCenter = true;
        return Projectile?.projectile.explosionRadius ?? 0f;
    }

    public override bool Available()
    {
        if (!base.Available())
        {
            return false;
        }

        if (!CasterIsPawn)
        {
            return Projectile != null;
        }

        var casterPawn = CasterPawn;
        if (casterPawn.Faction != Faction.OfPlayer && casterPawn.mindState.MeleeThreatStillThreat &&
            casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
        {
            return false;
        }

        return Projectile != null;
    }
}