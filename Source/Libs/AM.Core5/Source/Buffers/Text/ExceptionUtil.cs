// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* ExceptionUtil.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Buffers.Text;

internal static class ExceptionUtil
{
    internal static void ThrowArgumentException (string paramName)
    {
        throw new ArgumentException ("Can't format argument.", paramName);
    }

    internal static void ThrowFormatException()
    {
        throw new FormatException (
            "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
    }

    internal static void ThrowFormatError()
    {
        throw new FormatException ("Input string was not in a correct format.");
    }
}
