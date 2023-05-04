// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ParsingTutorial;

internal static class Program
{
    public static void Main (string[] args)
    {
        Workshop01.Step1();
        Workshop02.Step1();
    }
}
