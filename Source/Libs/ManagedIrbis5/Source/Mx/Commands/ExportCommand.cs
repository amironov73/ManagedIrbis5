// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExportCommand.cs -- экспорт найденных записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Экспорт найденных записей.
/// </summary>
public sealed class ExportCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExportCommand()
        : base ("export")
    {
        // пустое тело конструктора
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute" />
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        // TODO поддержка разных форматов: JSON, XML, ISO
        // TODO поддержка кодировок: ANSI, KOI, UTF-8

        OnBeforeExecute();

        var provider = executive.Provider;
        if (!provider.IsConnected)
        {
            executive.WriteError ("Not connected");
            return false;
        }

        var fileName = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        if (string.IsNullOrEmpty (fileName))
        {
            executive.WriteError ("File name required");
            return false;
        }

        var records = executive.Records;
        if (records.IsNullOrEmpty())
        {
            executive.WriteError ("No records");
        }

        var mfns = records.Select (x => x.Mfn);
        var read = provider.ReadRecords
            (
                provider.EnsureDatabase(),
                mfns
            );
        if (read.IsNullOrEmpty())
        {
            executive.WriteError ("Can't retrieve records");
            return false;
        }

        using var writer = TextWriterUtility.Create (fileName, IrbisEncoding.Utf8);
        foreach (var record in read)
        {
            var text = record.ToPlainText();
            writer.Write (text);
            writer.WriteLine ("*****");
        }

        executive.WriteMessage ($"Records exported {read.Length}");

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string GetShortHelp()
    {
        return "Export of the found records";
    }

    #endregion
}
