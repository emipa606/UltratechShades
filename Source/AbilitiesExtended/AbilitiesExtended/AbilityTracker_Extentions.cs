using System;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public static class AbilityTracker_Extentions
{
    extension(Pawn_AbilityTracker tracker)
    {
        public void TryGainEquipmentAbility(AbilityDef abilityDef,
            ThingWithComps thing)
        {
            if (abilityDef is not EquipmentAbilityDef def)
            {
                return;
            }

            if (tracker.abilities.FirstOrFallback(x =>
                    x?.def == def && x is EquipmentAbility equipmentAbility2 &&
                    equipmentAbility2.sourceEquipment == thing) is EquipmentAbility)
            {
                return;
            }

            if (Activator.CreateInstance(def.abilityClass, tracker.pawn, def, thing) is EquipmentAbility
                equipmentAbility)
            {
                equipmentAbility.sourceEquipment = thing;
                tracker.abilities.Add(equipmentAbility);
            }

            tracker.Notify_TemporaryAbilitiesChanged();
        }

        public void TryRemoveEquipmentAbility(AbilityDef def, ThingWithComps thing)
        {
            if (def is not EquipmentAbilityDef || tracker.abilities.FirstOrFallback(x =>
                        x?.def == def && x is EquipmentAbility equipmentAbility &&
                        equipmentAbility.sourceEquipment == thing) is
                    not EquipmentAbility item)
            {
                return;
            }

            tracker.abilities.Remove(item);
            tracker.Notify_TemporaryAbilitiesChanged();
        }
    }
}