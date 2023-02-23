using RimWorld;

namespace AbilitiesExtended;

public class Command_EquipmentAbility : Command_Ability
{
    public int curTicks = -1;

    public Command_EquipmentAbility(EquipmentAbility ability)
        : base(ability)
    {
    }

    public new EquipmentAbility ability => (EquipmentAbility)base.ability;
}