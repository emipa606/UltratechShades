using HarmonyLib;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Pawn_EquipmentTracker), "Notify_EquipmentRemoved")]
public static class AE_Pawn_EquipmentTracker_Notify_EquipmentRemoved_CompAbilityItem_Patch
{
    [HarmonyPostfix]
    public static void Notify_EquipmentRemovedPostfix(Pawn_EquipmentTracker __instance, ThingWithComps eq)
    {
        if (eq == null || __instance == null || eq.TryGetCompFast<CompAbilityItem>() == null)
        {
            return;
        }

        var compAbilityItem = eq.TryGetCompFast<CompAbilityItem>();
        if (compAbilityItem == null)
        {
            return;
        }

        var pawn = __instance.pawn;
        if (!pawn.RaceProps.Humanlike || compAbilityItem.Props.Abilities.NullOrEmpty())
        {
            return;
        }

        foreach (var ability in compAbilityItem.Props.Abilities)
        {
            __instance.pawn.abilities.TryRemoveEquipmentAbility(ability, eq);
        }
    }
}