// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* PrintTableCommand.cs -- формирование таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands;

/// <summary>
/// Формирование таблицы.
/// </summary>
public sealed class PrintTableCommand
    : ServerCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PrintTableCommand
        (
            WorkData data
        )
        : base (data)
    {
    }

    #endregion

    #region ServerCommand members

    /// <inheritdoc cref="ServerCommand.Execute" />
    public override void Execute()
    {
        var engine = Data.Engine.ThrowIfNull();
        engine.OnBeforeExecute (Data);

        try
        {
            var context = engine.RequireContext (Data);
            Data.Context = context;
            UpdateContext();

            var request = Data.Request.ThrowIfNull();
            var database = request.RequireAnsiString();
            database.NotUsed();
            var table = request.RequireAnsiString();
            table.NotUsed();
            var headers = request.GetUtfString();
            headers.NotUsed();
            var mode = request.GetAnsiString();
            mode.NotUsed();
            var searchQuery = request.GetUtfString();
            searchQuery.NotUsed();
            var minMfn = request.GetInt32();
            minMfn.NotUsed();
            var maxMfn = request.GetInt32();
            maxMfn.NotUsed();
            var sequentialQuery = request.GetUtfString();
            sequentialQuery.NotUsed();
            var mfnList = Array.Empty<int>(); // TODO get mfnList
            mfnList.NotUsed();

            // TODO implement

            var response = Data.Response.ThrowIfNull (nameof (Data.Response));

            // Код возврата не отправляется
            response.WriteAnsiString (string.Empty).NewLine();
            SendResponse();
        }
        catch (IrbisException exception)
        {
            SendError (exception.ErrorCode);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PrintTableCommand) + "::" + nameof (Execute)
                );

            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
