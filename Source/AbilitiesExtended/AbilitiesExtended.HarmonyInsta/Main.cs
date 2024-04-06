using System.Reflection;
using HarmonyLib;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        new Harmony("com.ogliss.rimworld.mod.AbilitiesExtended").PatchAll(Assembly.GetExecutingAssembly());
    }
}