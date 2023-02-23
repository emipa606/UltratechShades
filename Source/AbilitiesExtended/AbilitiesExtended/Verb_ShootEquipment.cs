using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class Verb_ShootEquipment : Verb_EquipmentLaunchProjectile
{
    private int PropjectilesPerShot => verbProperties.ScattershotCount + 1;

    public override int ShotsPerBurst => verbProps.burstShotCount;

    public override void WarmupComplete()
    {
        base.WarmupComplete();
        if (currentTarget.Thing is not Pawn { Downed: false } pawn || !CasterIsPawn || CasterPawn.skills == null)
        {
            return;
        }

        var num = pawn.HostileTo(caster) ? 170f : 20f;
        var num2 = verbProps.AdjustedFullCycleTime(this, CasterPawn);
        CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
    }

    public override bool TryCastShot()
    {
        var castShot = false;
        for (var i = 0; i < PropjectilesPerShot; i++)
        {
            if (base.TryCastShot())
            {
                castShot = true;
            }
        }

        if (castShot && CasterIsPawn)
        {
            CasterPawn.records.Increment(RecordDefOf.ShotsFired);
        }

        return castShot;
    }
}