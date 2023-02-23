using HarmonyLib;
using RimWorld;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
public static class AE_Pawn_ApparelTracker_Notify_ApparelRemoved_CompAbilityItem_Patch
{
    [HarmonyPostfix]
    public static void Notify_ApparelRemovedPostfix(Pawn_EquipmentTracker __instance, Apparel apparel)
    {
        if (apparel.TryGetCompFast<CompAbilityItem>() == null)
        {
            return;
        }

        var pawn = __instance.pawn;
        if (!pawn.RaceProps.Humanlike)
        {
            return;
        }

        foreach (var comp in apparel.GetComps<CompAbilityItem>())
        {
            foreach (var ability in comp.Props.Abilities)
            {
                __instance.pawn.abilities.TryRemoveEquipmentAbility(ability, apparel);
            }
        }
    }
}