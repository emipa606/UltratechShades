using HarmonyLib;
using RimWorld;
using Verse;

namespace VerbCooldownFactor;

[HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.AdjustedCooldown), typeof(Verb), typeof(Pawn))]
public static class VerbProperties_AdjustedCooldown
{
    public static void Postfix(ref float __result, Verb ownerVerb)
    {
        var casterPawn = ownerVerb.CasterPawn;
        if (casterPawn != null)
        {
            __result *= casterPawn.GetStatValue(VCFDefOf.VerbCooldownFactor);
        }
    }
}