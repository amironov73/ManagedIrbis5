// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace PurrCalc;

internal static class Program
{
    public static void Main (string[] args)
    {
        var expression = string.Join (' ', args);
        if (!string.IsNullOrEmpty (expression))
        {
            var result = Grammar.Compute (expression);
            Console.WriteLine (result.ToString (CultureInfo.InvariantCulture));
        }
    }
}
