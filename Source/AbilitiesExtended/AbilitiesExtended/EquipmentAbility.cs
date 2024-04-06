using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AbilitiesExtended;

public class EquipmentAbility : Ability
{
    public ThingWithComps sourceEquipment;

    private int TicksUntilCasting = -5;

    public EquipmentAbility(Pawn pawn)
        : base(pawn)
    {
    }

    public EquipmentAbility(Pawn pawn, AbilityDef def)
        : base(pawn, def)
    {
    }

    public EquipmentAbility(Pawn pawn, AbilityDef def, Thing source)
        : base(pawn, def)
    {
        sourceEquipment = source as ThingWithComps;
    }

    private EquipmentAbilityDef AbilityDef => (EquipmentAbilityDef)def;

    public CompAbilityItem AbilityItem => sourceEquipment?.TryGetCompFast<CompAbilityItem>();

    public int MaxCastingTicks => (int)(AbilityDef.cooldown * 60f);

    public int CooldownTicksLeft
    {
        get => TicksUntilCasting;
        set => TicksUntilCasting = value;
    }

    public override void ExposeData()
    {
        Scribe_Defs.Look(ref def, "def");
        if (def == null)
        {
            return;
        }

        Scribe_Values.Look(ref Id, "Id", -1);
        if (Scribe.mode == LoadSaveMode.LoadingVars && Id == -1)
        {
            Id = Find.UniqueIDsManager.GetNextAbilityID();
        }

        Scribe_References.Look(ref sourceEquipment, "sourceEquipment");
        Scribe_References.Look(ref sourcePrecept, "sourcePrecept");
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            Initialize();
        }

        Scribe_Values.Look(ref TicksUntilCasting, "EquipmentAbilityTicksUntilcasting", -5);
    }

    public override IEnumerable<Command> GetGizmos()
    {
        var baseGizmos = base.GetGizmos();
        if (baseGizmos.Any())
        {
            foreach (var baseGizmo in baseGizmos)
            {
                yield return baseGizmo;
            }
        }

        if (Prefs.DevMode && AbilityDef.hasCooldown && CooldownTicksLeft > 0)
        {
            yield return new Command_Action
            {
                defaultLabel = "DEV: Reset cooldown",
                order = def.uiOrder + 1f,
                icon = ContentFinder<Texture2D>.Get("UI/ResetIcon"),
                action = delegate
                {
                    CooldownTicksLeft = 0;
                    StartCooldown(0);
                }
            };
        }
    }

    public virtual bool CanCastPowerCheck(string context, out string reason)
    {
        reason = "";
        if (context == "Player" && pawn.Faction != Faction.OfPlayer)
        {
            reason = "CannotOrderNonControlled".Translate();
            return false;
        }

        if (pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Violent) &&
            AbilityDef.verbProperties.violent)
        {
            reason = "AbilityDisabled_IncapableOfWorkTag".Translate(pawn.Named("PAWN"),
                WorkTags.Violent.LabelTranslated());
            return false;
        }

        if (!AbilityDef.hasCooldown || CooldownTicksLeft <= 0)
        {
            return true;
        }

        reason = "AU_PawnAbilityRecharging".Translate(pawn.NameShortColored);
        return false;
    }

    public override void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
    {
        base.QueueCastingJob(target, destination);
        if (!AbilityDef.hasCooldown)
        {
            return;
        }

        CooldownTicksLeft = MaxCastingTicks;
        StartCooldown(MaxCastingTicks);
    }

    public override void AbilityTick()
    {
        if (pawn.IsWorldPawn())
        {
            return;
        }

        base.AbilityTick();
        if (sourceEquipment != null)
        {
            if (sourceEquipment is Apparel apparel && apparel.Wearer != pawn)
            {
                pawn.abilities.TryRemoveEquipmentAbility(AbilityDef, sourceEquipment);
            }
        }
        else
        {
            Log.Warning($"{this} lost source equipment, removing ability");
            pawn.abilities.TryRemoveEquipmentAbility(AbilityDef, sourceEquipment);
        }

        if (CooldownTicksLeft >= 0 && !Find.TickManager.Paused)
        {
            CooldownTicksLeft--;
            if (AbilityItem != null)
            {
            }

            if (!gizmo.disabled)
            {
                gizmo.Disable("AbilityOnCooldown".Translate(CooldownTicksLeft.ToStringSecondsFromTicks()));
            }
        }
        else if (!Find.TickManager.Paused && gizmo is { disabled: true })
        {
            gizmo.disabled = false;
        }
    }
}