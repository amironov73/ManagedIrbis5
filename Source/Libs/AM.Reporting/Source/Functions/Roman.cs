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
using System.Text;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Functions
{
    /// <summary>
    /// Based on code of Stefan B�ther,  xprocs@hotmail.de
    /// </summary>
    internal static class Roman
    {
        private static int MAX = 3998;

        private static string[,] romanDigits = new string[,]
        {
            { "M", "C", "X", "I" },
            { "MM", "CC", "XX", "II" },
            { "MMM", "CCC", "XXX", "III" },
            { null, "CD", "XL", "IV" },
            { null, "D", "L", "V" },
            { null, "DC", "LX", "VI" },
            { null, "DCC", "LXX", "VII" },
            { null, "DCCC", "LXXX", "VIII" },
            { null, "CM", "XC", "IX" }
        };

        public static string Convert (int value)
        {
            if (value > MAX)
            {
                throw new ArgumentOutOfRangeException ("value");
            }

            var result = new StringBuilder (15);

            for (var index = 0; index < 4; index++)
            {
                var power = (int)Math.Pow (10, 3 - index);
                var digit = value / power;
                value -= digit * power;
                if (digit > 0)
                {
                    result.Append (romanDigits[digit - 1, index]);
                }
            }

            return result.ToString();
        }
    }
}
