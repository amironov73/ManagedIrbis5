// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* HammerContext.cs -- контекст выполнения команд
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using IrbisHammer.Commands;
using IrbisHammer.Source.Commands;

using ManagedIrbis;

#endregion

#nullable enable

namespace IrbisHammer;

/// <summary>
/// Контекст выполнения команд.
/// </summary>
public sealed class HammerContext
{
    #region Properties

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public string[] Arguments { get; }

    /// <summary>
    /// Имя исполняемого файла.
    /// </summary>
    public string ToolFileName { get; set; }

    /// <summary>
    /// Список известных баз данных.
    /// </summary>
    public List<DatabaseInfo> KnownDatabases { get; }

    /// <summary>
    /// Список известных команд.
    /// </summary>
    public List<ToolCommand> KnownCommands { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="arguments">
    /// Аргументы командной строки.
    /// </param>
    public HammerContext
        (
            string[] arguments
        )
    {
        Sure.NotNull (arguments);

        ToolFileName = "irbistool.exe";
        Arguments = arguments;
        KnownCommands = new ()
        {
            new Actualize(),
            new BuildFacetCache(),
            new BuildFullTextCache(),
            new CheckFormat(),
            new CleanPdf(),
            new ClearFacetCache(),
            new CreateDitionary(),
            new DiagnoseFullText(),
            new Diagnostics(),
            new DropDatabase(),
            new EmptyDatabase(),
            new EmptyFullText(),
            new ExecuteBatch(),
            new ExportInvertedFile(),
            new ExportRecords(),
            new Fst2Ifs(),
            new GlobalCorrection(),
            new ImportRecords(),
            new LoadFullText(),
            new LockDatabase(),
            new MergeDatabases(),
            new NewDatabase(),
            new Prettify(),
            new ReorganizeInvertedFile(),
            new ReorganizeMasterFile(),
            new UnlockDatabase(),
            new UnlockMfn()
        };

        KnownDatabases = new ();
    }

    #endregion
}
