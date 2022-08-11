// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* OperativeManager.cs -- менеджер оперативных режимов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

using static ManagedIrbis.Client.OperativeCommandCode;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Менеджер оперативных режимов.
/// </summary>
public class OperativeManager
{
    #region Properties

    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    private IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    public ISyncProvider Connection { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OperativeManager
        (
            IServiceProvider serviceProvider,
            ISyncProvider connection,
            LocalCatalogerIniFile iniFile
        )
    {
        Sure.NotNull (serviceProvider);
        Sure.NotNull (connection);
        Sure.NotNull (iniFile);
        connection.EnsureConnected();

        ServiceProvider = serviceProvider;
        Connection = connection;
        _logger = LoggingUtility.GetLogger (ServiceProvider, typeof (OperativeManager));
        _operHintFileName = "@" + iniFile.Main.GetValue ("OPERHINTPFT", "operhint.pft");
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;
    private readonly string _operHintFileName;

    /// <summary>
    /// Известные нам коды команд
    /// </summary>
    private static readonly Dictionary<string, Action<ClientContext, OperativeMode>?> _knownModes = new()
    {
        {GotoOne,  null},
        {GotoMany,  null},
        {NewDocument,  null},
        {GlobalCorrection,  null},
        {ExecuteBatchJob,  null},
        {ExecuteDll,  null},
        {GotoOne2,  null},
        {GotoMany2,  null},
        {MagazineRegistration,  null},
        {ListIssues,  null},
        {BindMagazines,  null},
        {GotoSummaryRecord,  null},
        {ShowArticles,  null},
        {ListOtherIssues,  null},
        {NewArticle,  null},
        {ShowBoundIssues,  null},
        {GotoSource,  null},
        {ListOtherArticles,  null}
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Определение оперативного режима в текущий момент времени.
    /// </summary>
    public OperativeMode[]? DetermineMode
        (
            ClientContext clientContext
        )
    {
        Sure.NotNull (clientContext);

        var currentRecord = clientContext.CurrentRecord;
        if (currentRecord is null)
        {
            return null;
        }

        var formatted = Connection
            .FormatRecord (_operHintFileName, currentRecord)
            .SafeTrim();
        if (string.IsNullOrEmpty (formatted))
        {
            return null;
        }

        var result = OperativeMode.Parse (formatted);

        return result;
    }

    /// <summary>
    /// Выполнение оперативного режима.
    /// </summary>
    public void ExecuteOperativeMode
        (
            ClientContext clientContext,
            OperativeMode operativeMode
        )
    {
        Sure.NotNull (clientContext);
        Sure.VerifyNotNull (operativeMode);

        var currentRecord = clientContext.CurrentRecord;
        if (currentRecord is null)
        {
            return;
        }

        var commandCode = operativeMode.CommandCode.SafeTrim();
        if (string.IsNullOrEmpty (commandCode))
        {
            return;
        }

        if (!_knownModes.TryGetValue (commandCode, out var action)
            || action is null)
        {
            return;
        }

        try
        {
            action (clientContext, operativeMode);
        }
        catch (Exception exception)
        {
            _logger.LogError (exception, "Error during operative mode");
        }
    }

    #endregion
}
