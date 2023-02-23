using System.Reflection;
using HarmonyLib;
using Verse;

namespace VerbCooldownFactor;

[StaticConstructorOnStartup]
public static class VCFHarmony
{
    static VCFHarmony()
    {
        new Harmony("flangopink.VerbCooldownFactor").PatchAll(Assembly.GetExecutingAssembly());
    }
}