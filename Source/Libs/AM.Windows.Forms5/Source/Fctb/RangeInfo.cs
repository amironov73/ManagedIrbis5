using System;

namespace Fctb;

internal class RangeInfo
{
    public Place Start { get; set; }
    public Place End { get; set; }

    public RangeInfo (TextRange r)
    {
        Start = r.Start;
        End = r.End;
    }

    internal int FromX
    {
        get
        {
            if (End.Line < Start.Line)
            {
                return End.Column;
            }

            if (End.Line > Start.Line)
            {
                return Start.Column;
            }

            return Math.Min (End.Column, Start.Column);
        }
    }
}
