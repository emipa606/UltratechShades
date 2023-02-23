using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class CompAbilityEffect_Dash : CompAbilityEffect_WithDest
{
    public CompProperties_AbilityDash CompProp => (CompProperties_AbilityDash)props;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        var map = Caster.Map;
        var dashingPawn = (DashingPawn)PawnFlyer.MakeFlyer(DefDatabase<ThingDef>.GetNamed("DashingPawn"), CasterPawn,
            target.Cell, DefDatabase<EffecterDef>.GetNamedSilentFail("JumpFlightEffect"),
            SoundDef.Named("JumpPackLand"));
        dashingPawn.ability = parent;
        dashingPawn.target = target.Thing == null ? target.CenterVector3 : target.Thing.InteractionCell.ToVector3();

        if (CompProp.rope)
        {
            dashingPawn.rope = true;
        }

        GenSpawn.Spawn(dashingPawn, Caster.Position, map);
        base.Apply(target, dest);
    }
}