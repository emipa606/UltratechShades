using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class HediffCompProperties_Ability : HediffCompProperties
{
    public readonly List<AbilityDef> abilities = [];

    public HediffCompProperties_Ability()
    {
        compClass = typeof(HediffComp_Ability);
    }
}