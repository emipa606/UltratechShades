using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AbilitiesExtended;

[StaticConstructorOnStartup]
public class DashingPawn : AbilityPawnFlyer
{
    private static readonly string RopeTexPath = "UI/Overlays/Rope";

    private static readonly Material RopeLineMat =
        MaterialPool.MatFrom(RopeTexPath, ShaderDatabase.Transparent, GenColor.FromBytes(99, 70, 41));

    public bool rope;

    public override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        FlyingPawn.DrawAt(getDrawPos(), flip);
        if (rope && position != Vector3.zero)
        {
            GenDraw.DrawLineBetween(position, target, AltitudeLayer.PawnRope.AltitudeFor(), RopeLineMat);
        }
    }

    public override void Tick()
    {
        base.Tick();
        if (MapHeld == null || Find.TickManager.TicksGame % 2 != 0)
        {
            return;
        }

        var mapHeld = MapHeld;
        var dataStatic = FleckMaker.GetDataStatic(getDrawPos(), mapHeld, FleckDefOf.DustPuffThick);
        dataStatic.rotation = Rand.Range(0f, 360f);
        mapHeld.flecks.CreateFleck(dataStatic);
    }

    private Vector3 getDrawPos()
    {
        var num = ticksFlying / (float)ticksFlightTime;
        var vector = position;
        return vector + (Vector3.forward * (num - Mathf.Pow(num, 2f)) * 1.5f);
    }

    public override void RespawnPawn()
    {
        var flyingPawn = FlyingPawn;
        if (flyingPawn.drafter != null)
        {
        }

        base.RespawnPawn();
        DefDatabase<SoundDef>.GetNamed("Dash_Whoosh").PlayOneShot(flyingPawn);
        FleckMaker.ThrowDustPuffThick(flyingPawn.DrawPos, flyingPawn.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
        flyingPawn.meleeVerbs.TryMeleeAttack(new LocalTargetInfo(target.ToIntVec3()).Pawn, null, true);
    }
}