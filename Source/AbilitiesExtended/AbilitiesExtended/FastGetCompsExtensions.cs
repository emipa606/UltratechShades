using Verse;

namespace AbilitiesExtended;

public static class FastGetCompsExtensions
{
    public static T TryGetCompFast<T>(this Thing thing) where T : ThingComp
    {
        if (!(thing is ThingWithComps thing2))
        {
            return null;
        }

        return thing2.GetCompFast<T>();
    }

    private static T GetCompFast<T>(this ThingWithComps thing) where T : ThingComp
    {
        var typeFromHandle = typeof(T);
        var allComps = thing.AllComps;
        var i = 0;
        for (var count = allComps.Count; i < count; i++)
        {
            var thingComp = allComps[i];
            if (thingComp.GetType() == typeFromHandle)
            {
                return (T)thingComp;
            }
        }

        return null;
    }

    public static T TryGetCompFast<T>(this Hediff hd) where T : HediffComp
    {
        if (!(hd is HediffWithComps hediffWithComps))
        {
            return null;
        }

        if (hediffWithComps.comps == null)
        {
            return null;
        }

        var typeFromHandle = typeof(T);
        var comps = hediffWithComps.comps;
        foreach (var hediffComp in comps)
        {
            if (hediffComp.GetType() == typeFromHandle)
            {
                return (T)hediffComp;
            }
        }

        return null;
    }
}