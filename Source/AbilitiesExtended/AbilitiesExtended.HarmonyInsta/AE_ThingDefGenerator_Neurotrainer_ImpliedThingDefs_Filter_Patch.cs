using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[HarmonyPatch(typeof(ThingDefGenerator_Neurotrainer), "ImpliedThingDefs")]
public static class AE_ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Filter_Patch
{
    public static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> list)
    {
        foreach (var item in list)
        {
            var compProperties = item.GetCompProperties<CompProperties_Neurotrainer>();
            if (compProperties.ability != null)
            {
                if (compProperties.ability.GetType() != typeof(EquipmentAbilityDef))
                {
                    yield return item;
                }
            }
            else
            {
                yield return item;
            }
        }
    }
}