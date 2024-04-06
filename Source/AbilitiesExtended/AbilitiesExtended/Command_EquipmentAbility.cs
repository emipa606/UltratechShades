using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class Command_EquipmentAbility(EquipmentAbility ability, Pawn pawn) : Command_Ability(ability, pawn)
{
    public int curTicks = -1;

    public new EquipmentAbility ability => (EquipmentAbility)base.ability;
}