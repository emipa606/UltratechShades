using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class CompProperties_AbilityPatternSpawn : CompProperties_AbilityEffect
{
    public bool despawnAffectedThings = true;

    public bool dontCareIfOccupied = false;

    public Color dustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
    public List<IntVec2> pattern;

    public ThingDef thingToSpawn;

    public bool throwDust = true;

    public CompProperties_AbilityPatternSpawn()
    {
        compClass = typeof(CompAbilityEffect_PatternSpawn);
    }
}