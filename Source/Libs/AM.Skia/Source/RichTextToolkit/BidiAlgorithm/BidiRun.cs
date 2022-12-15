// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit
{
    internal struct BidiRun
    {
        public Directionality Direction;
        public int Start;
        public int Length;
        public int End => Start + Length;

        public override string ToString()
        {
            return $"{Start} - {End} - {Direction}";
        }

        public static IEnumerable<BidiRun> CoalescLevels(Slice<sbyte> levels)
        {
            if (levels.Length == 0)
                yield break;

            int startRun = 0;
            sbyte runLevel = levels[0];
            for (int i = 1; i < levels.Length; i++)
            {
                if (levels[i] == runLevel)
                    continue;

                // End of this run
                yield return new BidiRun()
                {
                    Direction = (runLevel & 0x01) == 0 ? Directionality.L : Directionality.R,
                    Start = startRun,
                    Length = i - startRun,
                };

                // Move to next run
                startRun = i;
                runLevel = levels[i];
            }

            yield return new BidiRun()
            {
                Direction = (runLevel & 0x01) == 0 ? Directionality.L : Directionality.R,
                Start = startRun,
                Length = levels.Length - startRun,
            };
        }
    }
}
