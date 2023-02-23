using System;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public static class AbilityTracker_Extentions
{
    public static void TryGainEquipmentAbility(this Pawn_AbilityTracker tracker, AbilityDef abilityDef,
        ThingWithComps thing)
    {
        if (abilityDef is not EquipmentAbilityDef def)
        {
            return;
        }

        if (tracker.abilities.FirstOrFallback(x =>
                x.def == def && x is EquipmentAbility equipmentAbility2 &&
                equipmentAbility2.sourceEquipment == thing) is EquipmentAbility)
        {
            return;
        }

        if (Activator.CreateInstance(def.abilityClass, tracker.pawn, def, thing) is EquipmentAbility equipmentAbility)
        {
            equipmentAbility.sourceEquipment = thing;
            tracker.abilities.Add(equipmentAbility);
        }

        tracker.Notify_TemporaryAbilitiesChanged();
    }

    public static void TryRemoveEquipmentAbility(this Pawn_AbilityTracker tracker, AbilityDef def, ThingWithComps thing)
    {
        if (def is not EquipmentAbilityDef || tracker.abilities.FirstOrFallback(x =>
                    x.def == def && x is EquipmentAbility equipmentAbility &&
                    equipmentAbility.sourceEquipment == thing) is
                not EquipmentAbility item)
        {
            return;
        }

        tracker.abilities.Remove(item);
        tracker.Notify_TemporaryAbilitiesChanged();
    }

    public static void GainAbility(this Pawn_AbilityTracker tracker, AbilityDef def, Thing source)
    {
        if (!tracker.abilities.Any(a => a.def == def))
        {
            tracker.abilities.Add(Activator.CreateInstance(def.abilityClass, tracker.pawn, def, source) as Ability);
        }
    }
}