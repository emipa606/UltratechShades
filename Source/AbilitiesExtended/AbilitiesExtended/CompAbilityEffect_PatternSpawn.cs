using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class CompAbilityEffect_PatternSpawn : CompAbilityEffect
{
    private new CompProperties_AbilityPatternSpawn Props => (CompProperties_AbilityPatternSpawn)props;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        base.Apply(target, dest);
        var map = parent.pawn.Map;
        var list = new List<Thing>();
        list.AddRange(affectedCells(target, map).SelectMany(c => from t in c.GetThingList(map)
            where t.def.category == ThingCategory.Item
            select t));
        if (Props.despawnAffectedThings)
        {
            foreach (var item in list)
            {
                item.DeSpawn();
            }
        }

        foreach (var item2 in affectedCells(target, map))
        {
            GenSpawn.Spawn(Props.thingToSpawn, item2, map);
            if (Props.throwDust)
            {
                FleckMaker.ThrowDustPuffThick(item2.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), Props.dustColor);
            }
        }

        foreach (var item3 in list)
        {
            var intVec = IntVec3.Invalid;
            for (var i = 0; i < 9; i++)
            {
                var intVec2 = item3.Position + GenRadial.RadialPattern[i];
                if (!intVec2.InBounds(map) || !intVec2.Walkable(map) ||
                    map.thingGrid.ThingsListAtFast(intVec2).Count > 0)
                {
                    continue;
                }

                intVec = intVec2;
                break;
            }

            if (intVec != IntVec3.Invalid)
            {
                GenSpawn.Spawn(item3, intVec, map);
            }
            else
            {
                GenPlace.TryPlaceThing(item3, item3.Position, map, ThingPlaceMode.Near);
            }
        }
    }

    public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
    {
        return Valid(target, true);
    }

    public override void DrawEffectPreview(LocalTargetInfo target)
    {
        GenDraw.DrawFieldEdges(affectedCells(target, parent.pawn.Map).ToList(),
            Valid(target) ? Color.white : Color.red);
    }

    private IEnumerable<IntVec3> affectedCells(LocalTargetInfo target, Map map)
    {
        foreach (var item in Props.pattern)
        {
            var intVec = target.Cell + new IntVec3(item.x, 0, item.z);
            if (intVec.InBounds(map))
            {
                yield return intVec;
            }
        }
    }

    public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
    {
        if (Props.dontCareIfOccupied && affectedCells(target, parent.pawn.Map).Any(c => c.Filled(parent.pawn.Map)))
        {
            if (throwMessages)
            {
                Messages.Message("AbilityOccupiedCells".Translate(parent.def.LabelCap),
                    target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
            }

            return false;
        }

        if (affectedCells(target, parent.pawn.Map).All(c => c.Standable(parent.pawn.Map)))
        {
            return true;
        }

        if (throwMessages)
        {
            Messages.Message("AbilityUnwalkable".Translate(parent.def.LabelCap),
                target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
        }

        return false;
    }
}