using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HediffsExtended;

public class CompProperties_HediffImmunities : CompProperties
{
    public List<HediffDef> hediffDefs;

    public Color textColor = Color.white;

    public float textDuration = 3f;

    public bool throwText = true;

    public CompProperties_HediffImmunities()
    {
        compClass = typeof(CompHediffImmunity);
    }
}