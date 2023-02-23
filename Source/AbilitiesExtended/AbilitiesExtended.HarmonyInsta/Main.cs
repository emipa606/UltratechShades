using System.Reflection;
using HarmonyLib;
using Verse;

namespace AbilitiesExtended.HarmonyInstance;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        var harmony = new Harmony("com.ogliss.rimworld.mod.AbilitiesExtended");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}