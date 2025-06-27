using RimWorld;
using Verse;

namespace HediffsExtended;

public class HediffComp_DamageOverTime : HediffComp
{
    private int ticksCounter;

    private HediffCompProperties_DamageOverTime Props => (HediffCompProperties_DamageOverTime)props;

    public override void CompExposeData()
    {
        Scribe_Values.Look(ref ticksCounter, "ticksCounter");
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        ticksCounter++;
        if (ticksCounter <= Props.damageIntervalTicks)
        {
            return;
        }

        var compHediffImmunity = parent.pawn.TryGetComp<CompHediffImmunity>();
        if (compHediffImmunity != null && compHediffImmunity.Props.hediffDefs.Contains(Def))
        {
            ticksCounter = 0;
            parent.pawn.health.RemoveHediff(parent);
            if (compHediffImmunity.Props.throwText)
            {
                MoteMaker.ThrowText(parent.pawn.Position.ToVector3(), parent.pawn.Map, "HE_Immune".Translate(Def.label),
                    compHediffImmunity.Props.textColor, compHediffImmunity.Props.textDuration);
            }
        }
        else
        {
            parent.pawn.TakeDamage(new DamageInfo(Props.damageDef, Props.damageAmount, Props.armorPenetration));
            ticksCounter = 0;
        }
    }

    public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
    {
        parent.pawn.health.RemoveHediff(parent);
    }
}