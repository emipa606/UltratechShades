using Verse;

namespace AbilitiesExtended;

public static class FastGetCompsExtensions
{
    public static T TryGetCompFast<T>(this Thing thing) where T : ThingComp
    {
        return thing is not ThingWithComps thing2 ? null : thing2.getCompFast<T>();
    }

    private static T getCompFast<T>(this ThingWithComps thing) where T : ThingComp
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
}