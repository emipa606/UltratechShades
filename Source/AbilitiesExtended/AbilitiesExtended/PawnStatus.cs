using Verse;

namespace AbilitiesExtended;

public class PawnStatus
{
    public readonly Pawn Blocker;
    public readonly Pawn Shooter;

    private int _expiryTime;

    public PawnStatus(Pawn shooter, Pawn blocker)
    {
        Shooter = shooter;
        Blocker = blocker;
        Refresh();
    }

    public void Refresh()
    {
        _expiryTime = Find.TickManager.TicksGame + 20;
    }

    public bool IsExpired()
    {
        return Find.TickManager.TicksGame >= _expiryTime;
    }
}