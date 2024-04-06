using HarmonyLib;
using RimWorld;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Notify_ApparelAdded))]
public static class AE_Pawn_ApparelTracker_Notify_ApparelAdded_CompAbilityItem_Patch
{
    [HarmonyPostfix]
    public static void Notify_Notify_ApparelAddedPostfix(Pawn_EquipmentTracker __instance, Apparel apparel)
    {
        if (apparel.TryGetCompFast<CompAbilityItem>() == null)
        {
            return;
        }

        var compAbilityItem = apparel.TryGetCompFast<CompAbilityItem>();
        if (compAbilityItem == null)
        {
            return;
        }

        var pawn = __instance.pawn;
        if (!pawn.RaceProps.Humanlike || compAbilityItem.Props.Abilities.NullOrEmpty())
        {
            return;
        }

        foreach (var abilityDef in compAbilityItem.Props.Abilities)
        {
            var def = (EquipmentAbilityDef)abilityDef;
            if (!__instance.pawn.abilities.abilities.Any(x => x.def == def))
            {
                __instance.pawn.abilities.TryGainEquipmentAbility(def, apparel);
            }
        }
    }
}