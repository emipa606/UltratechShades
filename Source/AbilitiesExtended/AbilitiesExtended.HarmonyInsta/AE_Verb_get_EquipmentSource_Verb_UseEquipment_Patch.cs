using HarmonyLib;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(Verb), nameof(Verb.EquipmentSource), MethodType.Getter)]
public static class AE_Verb_get_EquipmentSource_Verb_UseEquipment_Patch
{
    [HarmonyPrefix]
    [HarmonyPriority(200)]
    public static bool Prefix(ref Verb __instance, ref ThingWithComps __result)
    {
        if (__instance.GetType() != typeof(Verb_ShootEquipment))
        {
            return true;
        }

        var verb_ShootEquipment = (Verb_ShootEquipment)__instance;
        if (verb_ShootEquipment.ability is not EquipmentAbility equipmentAbility)
        {
            return true;
        }

        if (equipmentAbility.sourceEquipment != null)
        {
            __result = equipmentAbility.sourceEquipment;
        }


        return true;
    }
}