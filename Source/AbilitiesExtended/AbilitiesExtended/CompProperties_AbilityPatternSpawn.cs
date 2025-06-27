using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class CompProperties_AbilityPatternSpawn : CompProperties_AbilityEffect
{
    public readonly bool despawnAffectedThings = true;

    public readonly bool dontCareIfOccupied = false;

    public readonly bool throwDust = true;

    public Color dustColor = new(0.55f, 0.55f, 0.55f, 4f);
    public List<IntVec2> pattern;

    public ThingDef thingToSpawn;

    public CompProperties_AbilityPatternSpawn()
    {
        compClass = typeof(CompAbilityEffect_PatternSpawn);
    }
}