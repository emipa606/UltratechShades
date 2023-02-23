using Verse;

namespace HediffsExtended;

public class HediffCompProperties_DamageOverTime : HediffCompProperties
{
    public float armorPenetration = 0f;

    public float damageAmount = 1f;

    public DamageDef damageDef;
    public int damageIntervalTicks = 50;

    public HediffCompProperties_DamageOverTime()
    {
        compClass = typeof(HediffComp_DamageOverTime);
    }
}