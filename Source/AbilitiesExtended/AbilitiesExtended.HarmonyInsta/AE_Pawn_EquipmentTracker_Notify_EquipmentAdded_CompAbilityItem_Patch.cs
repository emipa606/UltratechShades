using HarmonyLib;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Pawn_EquipmentTracker), "Notify_EquipmentAdded")]
public static class AE_Pawn_EquipmentTracker_Notify_EquipmentAdded_CompAbilityItem_Patch
{
    [HarmonyPostfix]
    public static void Notify_EquipmentAddedPostfix(Pawn_EquipmentTracker __instance, ThingWithComps eq)
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

        foreach (var abilityDef in compAbilityItem.Props.Abilities)
        {
            var ability = (EquipmentAbilityDef)abilityDef;
            __instance.pawn.abilities.TryGainEquipmentAbility(ability, eq);
        }
    }
}