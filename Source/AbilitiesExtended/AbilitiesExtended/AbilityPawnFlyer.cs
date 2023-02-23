using RimWorld;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class AbilityPawnFlyer : PawnFlyer
{
    public Ability ability;

    public Rot4 direction;

    public bool pawnCanFireAtWill = true;

    protected Vector3 position;

    public Vector3 target;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        direction = startVec.x > target.ToIntVec3().x ? Rot4.West :
            startVec.x < target.ToIntVec3().x ? Rot4.East :
            startVec.y < target.ToIntVec3().y ? Rot4.North : Rot4.South;
    }

    public override void Tick()
    {
        var t = ticksFlying / (float)ticksFlightTime;
        position = Vector3.Lerp(startVec, target, t);
        var pairs = FlyingPawn.Drawer.renderer.effecters.pairs;
        foreach (var item in pairs)
        {
            item.effecter.EffectTick(new TargetInfo(position.ToIntVec3(), MapHeld), TargetInfo.Invalid);
        }

        base.Tick();
    }

    public override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        FlyingPawn.Drawer.renderer.RenderPawnAt(position, direction);
    }

    public override void RespawnPawn()
    {
        Position = target.ToIntVec3();
        base.RespawnPawn();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref ability, "ability");
        Scribe_Values.Look(ref direction, "direction");
    }
}