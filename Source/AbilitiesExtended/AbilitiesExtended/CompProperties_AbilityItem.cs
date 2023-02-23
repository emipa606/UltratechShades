using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class CompProperties_AbilityItem : CompProperties
{
    public List<AbilityDef> Abilities = new List<AbilityDef>();

    public CompProperties_AbilityItem()
    {
        compClass = typeof(CompAbilityItem);
    }
}