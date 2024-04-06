using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HediffsExtended;

public class CompProperties_HediffImmunities : CompProperties
{
    public readonly float textDuration = 3f;

    public readonly bool throwText = true;
    public List<HediffDef> hediffDefs;

    public Color textColor = Color.white;

    public CompProperties_HediffImmunities()
    {
        compClass = typeof(CompHediffImmunity);
    }
}