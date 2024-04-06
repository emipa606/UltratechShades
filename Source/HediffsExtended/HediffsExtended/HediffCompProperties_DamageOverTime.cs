using Verse;

namespace HediffsExtended;

public class HediffCompProperties_DamageOverTime : HediffCompProperties
{
    public readonly float armorPenetration = 0f;

    public readonly float damageAmount = 1f;
    public readonly int damageIntervalTicks = 50;

    public DamageDef damageDef;

    public HediffCompProperties_DamageOverTime()
    {
        compClass = typeof(HediffComp_DamageOverTime);
    }
}