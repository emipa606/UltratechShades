using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class EquipmentAbilityDef : AbilityDef
{
    public readonly float cooldown = 0f;
    public readonly bool hasCooldown = true;

    public readonly bool requirePsycast = false;

    public int requiredPsycastLevel = 0;

    public virtual string GetDescription()
    {
        var basics = GetBasics();
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(description);
        if (basics != "")
        {
            stringBuilder.AppendLine(basics);
        }

        return stringBuilder.ToString();
    }

    public string GetBasics()
    {
        var result = "";
        var properties = verbProperties;
        if (properties == null)
        {
            return result;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{StringsToTranslate.AU_Cooldown}{cooldown:N0} " + "SecondsLower".Translate());
        if (comps.Any(x => x.GetType() == typeof(CompProperties_EffectWithDest)))
        {
            _ = (CompProperties_EffectWithDest)comps.Find(x =>
                x.compClass == typeof(CompProperties_EffectWithDest));
            if (verbProperties.defaultProjectile != null)
            {
                if (verbProperties.defaultProjectile.projectile.explosionRadius > 0f)
                {
                    stringBuilder.AppendLine(StringsToTranslate.AU_Type + StringsToTranslate.AU_TargetAoE);
                }
                else
                {
                    stringBuilder.AppendLine(StringsToTranslate.AU_Type + StringsToTranslate.AU_TargetThing);
                }
            }
            else
            {
                stringBuilder.AppendLine(StringsToTranslate.AU_Type + StringsToTranslate.AU_TargetThing);
            }
        }
        else
        {
            stringBuilder.AppendLine(StringsToTranslate.AU_Type + StringsToTranslate.AU_TargetSelf);
        }

        if (properties.defaultProjectile is { projectile: not null } &&
            properties.defaultProjectile.projectile.GetDamageAmount(1f) > 0)
        {
            stringBuilder.AppendLine(string.Concat("Damage".Translate() + ": ",
                properties.defaultProjectile.projectile.GetDamageAmount(1f).ToString()));
            stringBuilder.AppendLine("Damage".Translate() + " " + StringsToTranslate.AU_Type +
                                     properties.defaultProjectile.projectile.damageDef.LabelCap);
        }

        if (comps.Any(x => x.GetType() == typeof(CompProperties_AbilityGiveMentalState)))
        {
            var mentalStatesToApply = new List<CompProperties_AbilityGiveMentalState>();
            comps.FindAll(x => x.compClass == typeof(CompProperties_AbilityGiveMentalState)).ForEach(
                delegate(AbilityCompProperties x)
                {
                    mentalStatesToApply.Add((CompProperties_AbilityGiveMentalState)x);
                });
            if (mentalStatesToApply is { Count: > 0 })
            {
                if (mentalStatesToApply.Count == 1)
                {
                    stringBuilder.AppendLine($"{StringsToTranslate.AU_MentalStateChance}: " +
                                             mentalStatesToApply[0].stateDef.LabelCap);
                }
                else
                {
                    stringBuilder.AppendLine(StringsToTranslate.AU_MentalStateChance);
                    foreach (var item in mentalStatesToApply)
                    {
                        stringBuilder.AppendLine("\t" + item.stateDef.LabelCap);
                    }
                }
            }
        }

        if (comps.Any(x => x.GetType() == typeof(CompProperties_AbilityGiveHediff)))
        {
            var hediffsToApply = new List<CompProperties_AbilityGiveHediff>();
            comps.FindAll(x => x.compClass == typeof(CompProperties_AbilityGiveHediff)).ForEach(
                delegate(AbilityCompProperties x) { hediffsToApply.Add((CompProperties_AbilityGiveHediff)x); });
            if (hediffsToApply is { Count: > 0 })
            {
                if (hediffsToApply.Count == 1)
                {
                    stringBuilder.AppendLine(StringsToTranslate.AU_EffectChance +
                                             hediffsToApply[0].hediffDef.LabelCap);
                }
                else
                {
                    stringBuilder.AppendLine(StringsToTranslate.AU_EffectChance);
                    foreach (var item2 in hediffsToApply)
                    {
                        var num = 0f;
                        if (item2.hediffDef.comps != null && item2.hediffDef.HasComp(typeof(HediffComp_Disappears)))
                        {
                            var max =
                                ((HediffCompProperties_Disappears)item2.hediffDef.CompPropsFor(
                                    typeof(HediffComp_Disappears))).disappearsAfterTicks.max;
                            num = max.TicksToSeconds();
                        }

                        if (num == 0f)
                        {
                            stringBuilder.AppendLine("\t" + item2.hediffDef.LabelCap);
                        }
                        else
                        {
                            stringBuilder.AppendLine($"{"\t" + item2.hediffDef.LabelCap + " " + " "}{num} " +
                                                     "SecondsToLower".Translate());
                        }
                    }
                }
            }

            if (properties.burstShotCount > 1)
            {
                stringBuilder.AppendLine($"{StringsToTranslate.AU_BurstShotCount} {properties.burstShotCount}");
            }
        }

        result = stringBuilder.ToString();

        return result;
    }
}