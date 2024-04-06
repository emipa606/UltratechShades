using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended;

public class VerbProperties_EquipmentAbility : VerbProperties
{
    public readonly int ScattershotCount = 0;
    public bool EffectsUser = false;

    public float EffectsUserChance = 0f;

    public DamageDef ForceWeaponEffect = null;

    public HediffDef ForceWeaponHediff = null;

    public float ForceWeaponKillChance = 0f;

    public SoundDef ForceWeaponTriggerSound = null;

    public bool GetsHot = false;

    public bool GetsHotCrit = false;

    public float GetsHotCritChance = 0f;

    public bool GetsHotCritExplosion = false;

    public float GetsHotCritExplosionChance = 0f;

    public float HotDamage = 0f;

    public bool HotDamageWeapon = false;

    public bool Multishot = false;
    public bool RapidFire = false;

    public bool Rending = false;

    public float RendingChance = 0.167f;

    public ResearchProjectDef requiredResearch = null;

    public StatDef ResistEffectStat = null;

    public bool TwinLinked = false;

    public bool TyranidBurstBodySize = false;

    public HediffDef UserEffect = null;

    public List<string> UserEffectImmuneList = [];

    public VerbProperties VerbProps;
}