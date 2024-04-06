using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class CompProperties_AbilityItem : CompProperties
{
    public readonly List<AbilityDef> Abilities = [];

    public CompProperties_AbilityItem()
    {
        compClass = typeof(CompAbilityItem);
    }
}