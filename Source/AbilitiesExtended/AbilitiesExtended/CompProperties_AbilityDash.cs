using RimWorld;

namespace AbilitiesExtended;

public class CompProperties_AbilityDash : CompProperties_EffectWithDest
{
    public bool rope;

    public CompProperties_AbilityDash()
    {
        compClass = typeof(CompAbilityEffect_Dash);
    }
}