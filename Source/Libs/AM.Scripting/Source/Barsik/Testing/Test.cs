// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Test.cs -- описание одного теста для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Описание одного теста Барсика.
/// </summary>
sealed class Test
{
    #region Properties

    /// <summary>
    /// Имя теста.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Папка, в которой хранятся данные для теста.
    /// </summary>
    public string Folder { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Test
        (
            string folder
        )
    {
        Sure.NotNullNorEmpty (folder);

        Folder = Path.GetFullPath (folder);
        Name = Path.GetFileName (folder).ThrowIfNullOrEmpty();
    }

    #endregion

    #region Private members

    private string GetFullName
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return Path.Combine (Folder, fileName);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Прогон теста.
    /// </summary>
    public TestResult Run
        (
            TestContext context
        )
    {
        var result = new TestResult
        {
            Name = Name,
        };

        string? expectedExceptionType = null;
        var exceptionFileName = GetFullName (TestUtility.ExceptionFileName);
        if (File.Exists (exceptionFileName))
        {
            expectedExceptionType = File.ReadAllText (exceptionFileName).Trim();
        }

        try
        {
            var descriptionFile = GetFullName (TestUtility.DescriptionFileName);
            if (File.Exists (descriptionFile))
            {
                var description = File.ReadAllText (descriptionFile).DosToUnix()!.Trim();
                result.Description = description;
                context.Output.Write ($"{Name,-15}: {description,-55}");
            }

            var ignoreFile = GetFullName (TestUtility.IgnoreFileName);
            if (File.Exists (ignoreFile))
            {
                result.Ignored = true;
                context.Output.WriteLine (" IGNORED");
                goto DONE;
            }

            string? expected = null;
            var expectedFile = GetFullName (TestUtility.OutputFileName);
            if (File.Exists (expectedFile))
            {
                expected = File.ReadAllText (expectedFile).DosToUnix();
                result.Expected = expected;
            }

            var sourceFile = GetFullName (TestUtility.SourceFileName);
            var sourceCode = File.ReadAllText (sourceFile).DosToUnix();
            result.Source = sourceCode;

            var inputStream = TextReader.Null;
            var inputFile = GetFullName (TestUtility.InputFileName);
            if (File.Exists (inputFile))
            {
                var inputText = File.ReadAllText (inputFile).DosToUnix();
                result.Input = inputText;
                inputStream = new StringReader (inputText!);
            }

            var outputStream = new StringWriter();

            var interpreter = new Interpreter
                (
                    input: inputStream,
                    output: outputStream
                )
                .WithStdLib();

            interpreter.ExecuteFile (sourceFile);

            var actualOutput = outputStream.ToString().DosToUnix();
            result.Output = actualOutput;
            if (expected is not null)
            {
                if (actualOutput != expected)
                {
                    result.Failed = true;
                }
            }

            if (expectedExceptionType is not null && !result.Failed)
            {
                result.Failed = true;
                context.Output.WriteLine();
                context.Output.WriteLine ($"Expected exception={exceptionFileName}, no exception thrown");
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Test) + "::" + nameof (Run)
                );

            result.Failed = true;
            result.Exception = exception.ToString();

            if (expectedExceptionType is not null)
            {
                var actualExceptionType = exception.GetType().FullName;
                if (actualExceptionType != expectedExceptionType)
                {
                    context.Output.WriteLine();
                    context.Output.WriteLine
                        (
                            $"Expected exception={expectedExceptionType}, got={actualExceptionType}"
                        );

                    context.Output.WriteLine();
                    context.Output.WriteLine (exception.Message);
                }
                else
                {
                    result.Failed = false;
                }
            }
        }

        context.Output.WriteLine (result.Failed ? " FAILED" : " SUCCESS");

        if (
                result.Expected is not null
                && result.Expected != result.Output
                && expectedExceptionType is null
            )
        {
            context.Output.WriteLine ($"\tEXPECTED <{result.Expected}>");
            context.Output.WriteLine ($"\tACTUAL <{result.Output}>");
            if (result.Output is not null)
            {
                TestUtility.ShowDifference (context, result.Expected, result.Output);
            }
        }

        DONE:
        result.FinishTime = DateTime.Now;
        result.Duration = result.FinishTime - result.StartTime;

        return result;
    }

    #endregion
}
