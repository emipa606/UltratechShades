using Verse;

namespace AbilitiesExtended;

[StaticConstructorOnStartup]
public class HediffComp_Ability : HediffComp
{
    public virtual HediffCompProperties_Ability Props => props as HediffCompProperties_Ability;

    public override void CompPostMake()
    {
        base.CompPostMake();
        foreach (var ab in Props.abilities)
        {
            if (!Pawn.abilities.abilities.Any(x => x.def == ab))
            {
                Pawn.abilities.GainAbility(ab);
            }
        }
    }

    public override void CompPostPostAdd(DamageInfo? dinfo)
    {
        base.CompPostPostAdd(dinfo);
        foreach (var ab in Props.abilities)
        {
            if (!Pawn.abilities.abilities.Any(x => x.def == ab))
            {
                Pawn.abilities.GainAbility(ab);
            }
        }
    }

    public override void CompPostPostRemoved()
    {
        base.CompPostPostRemoved();
        foreach (var ab in Pawn.abilities.abilities)
        {
            if (Props.abilities.Any(x => x == ab.def))
            {
                Pawn.abilities.abilities.Remove(ab);
            }
        }
    }
}