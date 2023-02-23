using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class Command_EquipmentPsycast : Command_Ability
{
    public int curTicks = -1;

    public Command_EquipmentPsycast(EquipmentAbility ability)
        : base(ability)
    {
        shrinkable = true;
    }

    public new EquipmentAbility ability => (EquipmentAbility)base.ability;

    public override string Label
    {
        get
        {
            if (!ability.pawn.IsCaravanMember())
            {
                return base.Label;
            }

            var pawn = ability.pawn;
            var psychicEntropy = pawn.psychicEntropy;
            var stringBuilder = new StringBuilder($"{base.Label} ({pawn.LabelShort}");
            if (ability.def.PsyfocusCost > float.Epsilon)
            {
                stringBuilder.Append(", " + "PsyfocusLetter".Translate() + ":" +
                                     psychicEntropy.CurrentPsyfocus.ToStringPercent("0"));
            }

            if (ability.def.EntropyGain > float.Epsilon)
            {
                if (ability.def.PsyfocusCost > float.Epsilon)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(string.Concat(" " + "NeuralHeatLetter".Translate() + ":",
                    Mathf.Round(psychicEntropy.EntropyValue).ToString()));
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }

    public override string TopRightLabel
    {
        get
        {
            var def = ability.def;
            var text = "";
            if (def.EntropyGain > float.Epsilon)
            {
                text += "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString() + "\n";
            }

            if (!(def.PsyfocusCost > float.Epsilon))
            {
                return text.TrimEndNewlines();
            }

            var text2 = !def.AnyCompOverridesPsyfocusCost ? def.PsyfocusCostPercent :
                !(def.PsyfocusCostRange.Span > float.Epsilon) ? def.PsyfocusCostPercentMax :
                $"{def.PsyfocusCostRange.min * 100f}-{def.PsyfocusCostPercentMax}";
            text += "PsyfocusLetter".Translate() + ": " + text2;

            return text.TrimEndNewlines();
        }
    }

    public override void DisabledCheck()
    {
        var def = ability.def;
        var pawn = ability.pawn;
        disabled = false;
        if (def.EntropyGain > float.Epsilon)
        {
            if (pawn.GetPsylinkLevel() < def.level)
            {
                DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
            }
            else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain +
                                                              PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
            {
                DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
            }
        }

        base.DisabledCheck();
    }
}