// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Program.cs -- вся логика программы в одном файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using AM.Scripting.Barsik;

#endregion

#nullable enable

namespace BarsorRunner;

/// <summary>
/// Вся логика программы в одном файле.
/// </summary>
class Program
{
    /// <summary>
    /// Точка входа.
    /// </summary>
    public static void Main
        (
            string[] args
        )
    {
        foreach (var fileName in args)
        {
            var barsor = new BarsorParser();
            var templateText = File.ReadAllText (fileName);
            var program = barsor.ParseTemplate (templateText);
            var interpreter = new Interpreter();
            interpreter.Execute (program);
        }
    }
}
