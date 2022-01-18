// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal class OperationTimeLimiter
{
    public static OperationTimeLimiter Create (int timeLimitInMs, int queriesToTriggerCheck)
    {
        return new (
            Environment.TickCount,
            queriesToTriggerCheck,
            timeLimitInMs);
    }

    public static OperationTimeLimiter Create (int timeLimitInMs)
    {
        return Create (timeLimitInMs, 0);
    }

    private OperationTimeLimiter (
        long operationStartTime,
        int queriesToTriggerCheck,
        int timeLimitInMs)
    {
#if DEBUG
        if (queriesToTriggerCheck < 0) throw new ArgumentOutOfRangeException (nameof (queriesToTriggerCheck));
#endif

        OperationStartTime = operationStartTime;
        QueriesToTriggerCheck = queriesToTriggerCheck;
        TimeLimitInMs = timeLimitInMs;
        QueryCounter = queriesToTriggerCheck;
        HasExpired = false;
    }

    private long OperationStartTime;

    private int QueriesToTriggerCheck;

    private int TimeLimitInMs;

    public int QueryCounter { get; private set; }

    public bool HasExpired { get; private set; }

    public bool QueryForExpiration()
    {
        if (!HasExpired)
        {
            if (QueryCounter == 0)
                HandleQueryCounterTrigger();
            else
                QueryCounter--;
        }

        return HasExpired;
    }

    public void Reset()
    {
        OperationStartTime = Environment.TickCount;
        QueryCounter = QueriesToTriggerCheck;
        HasExpired = false;
    }

    private void HandleQueryCounterTrigger()
    {
        var currentTicks = Environment.TickCount - OperationStartTime;
        if (currentTicks > TimeLimitInMs) HasExpired = true;

        QueryCounter = QueriesToTriggerCheck;
    }
}
