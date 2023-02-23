namespace AbilitiesExtended;

public class Verb_UseEquipment_Thrown : Verb_EquipmentLaunchProjectile
{
    public void PostCastShot(bool inResult, out bool outResult)
    {
        outResult = inResult;
        var primary = CasterPawn.equipment.Primary;
        CasterPawn.equipment.Notify_EquipmentRemoved(primary);
    }

    public override bool TryCastShot()
    {
        var outResult = base.TryCastShot();
        if (outResult)
        {
            PostCastShot(true, out outResult);
        }

        return outResult;
    }
}