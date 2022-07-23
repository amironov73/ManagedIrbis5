// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GblTest.cs -- одиночный тест для подсистемы глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.ConsoleIO;
using AM.Text;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Testing;

/// <summary>
/// Одиночный тест для подсистемы глобальной корректировки.
/// </summary>
public sealed class GblTest
{
    #region Constants

    /// <summary>
    /// Имя файла, содержащего описание теста.
    /// </summary>
    public const string DescriptionFileName = "description.txt";

    /// <summary>
    /// Имя файла, содержащего дамп ожидаемого результата.
    /// </summary>
    public const string ExpectedFileName = "expected.txt";

    /// <summary>
    /// Имя файла, содержащего операторы глобальной корректировки.
    /// </summary>
    public const string InputFileName = "input.txt";

    /// <summary>
    /// Имя файла, содержащего дамп записи, подлежащей корректировке.
    /// </summary>
    public const string RecordFileName = "record.txt";

    #endregion

    #region Properties

    /// <summary>
    /// Синхронный ИРБИС-провайдер.
    /// </summary>
    public ISyncProvider? Provider { get; set; }

    /// <summary>
    /// Имя директории, содержащей тест.
    /// </summary>
    public string? Folder { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GblTest
        (
            string folder
        )
    {
        Sure.NotNullNorEmpty (folder);

        Folder = Path.GetFullPath (folder);
    }

    #endregion

    #region Private members

    private string GetFullName
        (
            string shortName
        )
    {
        Sure.NotNullNorEmpty (shortName);

        return Path.Combine
            (
                Folder.ThrowIfNull(),
                shortName
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Содержит ли указанная директория спецификацию теста?
    /// </summary>
    public static bool IsDirectoryContainsTest
        (
            string directory
        )
    {
        Sure.NotNullNorEmpty (directory);

        var result =
            File.Exists (Path.Combine (directory, DescriptionFileName))
            && File.Exists (Path.Combine (directory, RecordFileName))
            && File.Exists (Path.Combine (directory, InputFileName));

        return result;
    }

    /// <summary>
    /// Прогон теста.
    /// </summary>
    public GblTestResult Run
        (
            string name
        )
    {
        Sure.NotNull (name);

        var result = new GblTestResult
        {
            Name = name,
            StartTime = DateTime.Now
        };

        try
        {
            if (Provider is null)
            {
                throw new GblException ("environment not set");
            }

            var descriptionFile = GetFullName (DescriptionFileName);
            if (File.Exists (descriptionFile))
            {
                var description = File.ReadAllText
                    (
                        descriptionFile,
                        IrbisEncoding.Utf8
                    );
                result.Description = description;
                ConsoleInput.Write (description);
            }

            var recordFile = GetFullName (RecordFileName).ThrowIfNullOrEmpty();
            var record = PlainText.ReadOneRecord (recordFile, IrbisEncoding.Utf8)
                .ThrowIfNull();
            record.Mfn = 1; // TODO some other value?
            var gblFileName = GetFullName (InputFileName)
                .ThrowIfNullOrEmpty();
            var input = File.ReadAllText (gblFileName, IrbisEncoding.Utf8)
                .DosToUnix()
                .ThrowIfNull ("input");
            result.Input = input;

            //ConsoleInput.WriteLine(input);
            //ConsoleInput.WriteLine();

            var program = GblFile.ParseLocalFile (gblFileName);

            //result.Ast = program.DumpToText().DosToUnix();
            //ConsoleInput.WriteLine(result.Ast);
            //ConsoleInput.WriteLine();

            var expectedFile = GetFullName (ExpectedFileName);
            string? expected = null;
            if (File.Exists (expectedFile))
            {
                expected = File.ReadAllText (expectedFile, IrbisEncoding.Utf8)
                    .DosToUnix()
                    .ThrowIfNull (nameof (expected));
                result.Expected = expected;
            }

            var provider = Provider.ThrowIfNull();
            program.Execute (record, provider);
            var output = record.ToPlainText();
            result.Output = output;

            // ConsoleInput.WriteLine(output);

            if (expected != null)
            {
                if (output != expected)
                {
                    result.Failed = true;

                    ConsoleInput.WriteLine();
                    ConsoleInput.WriteLine ("!!! FAILED !!!");
                    ConsoleInput.WriteLine();
                    ConsoleInput.WriteLine ("EXPECTED");
                    ConsoleInput.WriteLine (expected);
                    ConsoleInput.WriteLine();
                    ConsoleInput.WriteLine ("ACTUAL");
                    ConsoleInput.WriteLine (output);
                    ConsoleInput.WriteLine();
                }
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (GblTest) + "::" + nameof (Run)
                );

            result.Failed = true;
            result.Exception = exception.ToString();
        }

        result.FinishTime = DateTime.Now;
        result.Duration = result.FinishTime - result.StartTime;

        return result;
    }

    #endregion
}
