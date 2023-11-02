// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Calculator.cs -- канареечный класс для генерации методов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.SourceGeneration;

#endregion

namespace SourceGenerationTests;

/// <summary>
/// Канареечный класс для генерации методов.
/// </summary>
internal partial class Calculator
{
    [Stub]
    internal partial int Add (int left, int right);

    public void Run()
    {
        var x = 1;
        var y = 2;
        var z = Add (x, y);
        Console.WriteLine ($"{x} + {y} = {z}");
    }
}
