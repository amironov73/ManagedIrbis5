// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* TestUtility.cs -- полезные методы для тестирования Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Полезные методы для тестирования Барсика.
/// </summary>
public static class TestUtility
{
    #region Constants

    /// <summary>
    /// Если в папке с данными для теста находится
    /// файл с таким именем, тест вызовет отладчик
    /// перед началом выполнения.
    /// </summary>
    public const string DebugBreakFileName = "debug.break";

    /// <summary>
    /// Имя файла с описанием теста.
    /// </summary>
    public const string DescriptionFileName = "description.txt";

    /// <summary>
    /// Имя файла с ожидаемым результатом вывода.
    /// Если файла с таким именем нет,
    /// от теста требуется просто не упасть.
    /// </summary>
    public const string OutputFileName = "output.txt";

    /// <summary>
    /// Если в папке с данными для теста находится
    /// файл с таким именем, тест пропускается.
    /// </summary>
    public const string IgnoreFileName = "test.ignore";

    /// <summary>
    /// Имя файла с входными данными.
    /// Если такого файла нет, входных данных не требуется.
    /// </summary>
    public const string InputFileName = "input.txt";

    /// <summary>
    /// Имя файла, в котором содержится тип исключения,
    /// с которым должен упасть тест.
    /// </summary>
    public const string ExceptionFileName = "exception.txt";

    /// <summary>
    /// Имя файла с исходным кодом.
    /// </summary>
    public const string SourceFileName = "source.barsik";

    #endregion

    #region Private members

    private static void DumpAt
        (
            TestContext context,
            string name,
            byte[] bytes,
            int start
        )
    {
        context.Output.Write ($"{name}:");

        if (start != 0)
        {
            --start;
        }

        for (var i = 0; i < 6; i++)
        {
            var j = start + i;
            if (j >= bytes.Length)
            {
                break;
            }

            context.Output.Write ($" {bytes[j]:X2}");
        }

        context.Output.WriteLine();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Содержит ли папка тест?
    /// </summary>
    public static bool IsDirectoryContainsTest
        (
            string directory
        )
    {
        Sure.NotNullNorEmpty (directory);

        return File.Exists (Path.Combine (directory, DescriptionFileName))
               && File.Exists (Path.Combine (directory, SourceFileName));
    }

    /// <summary>
    /// Запуск всех тестов и сохранение результатов.
    /// </summary>
    public static bool RunTests
        (
            string inputFolder,
            string outputFolder
        )
    {
        Sure.NotNullNorEmpty (inputFolder);
        Sure.NotNullNorEmpty (outputFolder);

        if (!Directory.Exists (inputFolder))
        {
            throw new DirectoryNotFoundException (inputFolder);
        }

        if (!Directory.Exists (outputFolder))
        {
            throw new DirectoryNotFoundException (outputFolder);
        }

        var context = new TestContext (Console.Out);
        var tester = new Tester (context);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var testResults = tester.DiscoverAndRunTests (inputFolder);

        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;

        var totalTestCount = testResults.Length;
        var failedTestCount = testResults.Count (t => t.Failed);
        var ignoredTestCount = testResults.Count (t => t.Ignored);
        Console.WriteLine();
        Console.WriteLine
            (
                $"Total tests: {totalTestCount}, failed: {failedTestCount}, ignored: {ignoredTestCount}, elapsed: {elapsed.ToAutoString()}"
            );
        Console.WriteLine();

        var fileName = "barsik-tests-" + DateTime.Now.ToString ("yyyy-MM-dd HH-mm-ss") + ".json";
        fileName = Path.Combine (outputFolder, fileName);

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        var content = JsonSerializer.Serialize (testResults, options);
        File.WriteAllText (fileName, content);

        return failedTestCount == 0;
    }

    /// <summary>
    /// Показ разницы между двумя строками.
    /// </summary>
    public static void ShowDifference
        (
            TestContext context,
            string expected,
            string actual
        )
    {
        var expectedBytes = Encoding.UTF8.GetBytes (expected);
        var actualBytes = Encoding.UTF8.GetBytes (actual);

        var index = 0;
        while (index < expectedBytes.Length && index < actualBytes.Length)
        {
            if (expectedBytes[index] != actualBytes[index])
            {
                break;
            }

            ++index;
        }

        if (index == expectedBytes.Length && index == actualBytes.Length)
        {
            return;
        }

        context.Output.WriteLine ($"Difference at index {index}");
        DumpAt (context, "expected ", expectedBytes, index);
        DumpAt (context, "actual   ", actualBytes, index);
    }

    #endregion
}
