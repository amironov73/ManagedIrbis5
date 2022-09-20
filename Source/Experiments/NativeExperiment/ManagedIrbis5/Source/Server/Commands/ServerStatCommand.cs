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

/* ServerStatCommand.cs -- получение статистики работы сервера ИРБИС64
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
/// Получение статистики работы сервера ИРБИС64.
/// </summary>
public sealed class ServerStatCommand
    : ServerCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ServerStatCommand
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
            var context = engine.RequireAdministratorContext (Data);
            Data.Context = context;
            UpdateContext();

            var request = Data.Request.ThrowIfNull();
            request.NotUsed();

            // TODO implement

            var response = Data.Response.ThrowIfNull();

            // Код возврата
            response.WriteInt32 (0).NewLine();
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
                    nameof (ServerStat) + "::" + nameof (Execute)
                );

            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
