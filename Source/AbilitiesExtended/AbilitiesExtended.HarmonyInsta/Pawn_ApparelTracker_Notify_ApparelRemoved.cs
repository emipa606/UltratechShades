using HarmonyLib;
using RimWorld;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Notify_ApparelRemoved))]
public static class Pawn_ApparelTracker_Notify_ApparelRemoved
{
    public static void Postfix(Pawn_ApparelTracker __instance, Apparel apparel)
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