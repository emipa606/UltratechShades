using System.Linq;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class Verb_UseEquipment : Verb_CastAbility
{
    public VerbProperties_EquipmentAbility verbProperties => (VerbProperties_EquipmentAbility)verbProps;

    public new CompAbilityItem EquipmentCompSource =>
        ((EquipmentAbility)ability).sourceEquipment?.TryGetCompFast<CompAbilityItem>();

    public new ThingWithComps EquipmentSource =>
        ability is not EquipmentAbility equipmentAbility ? null : equipmentAbility.sourceEquipment;

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
}