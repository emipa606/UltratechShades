using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class HediffCompProperties_Ability : HediffCompProperties
{
    public List<AbilityDef> abilities = new List<AbilityDef>();

    public HediffCompProperties_Ability()
    {
        compClass = typeof(HediffComp_Ability);
    }
}