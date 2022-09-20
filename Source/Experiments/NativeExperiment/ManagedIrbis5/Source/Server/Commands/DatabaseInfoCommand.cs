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

/* DatabaseInfoCommand.cs -- получение информации о базе данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands;

/// <summary>
/// Получение информации о базе данных.
/// </summary>
public sealed class DatabaseInfoCommand
    : ServerCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DatabaseInfoCommand
        (
            WorkData data
        )
        : base (data)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private static void _WriteRecords
        (
            ServerResponse response,
            IReadOnlyList<int>? mfns
        )
    {
        if (!mfns.IsNullOrEmpty())
        {
            var line = string.Join (",", mfns);
            response.WriteAnsiString (line);
        }

        response.NewLine();
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
            var database = request.RequireAnsiString();

            DatabaseInfo info;
            using (var direct = engine.GetDatabase (database))
            {
                info = direct.GetDatabaseInfo();
            }

            var response = Data.Response.ThrowIfNull();

            // Код возврата
            response.WriteInt32 (0).NewLine();
            _WriteRecords (response, info.LogicallyDeletedRecords);
            _WriteRecords (response, info.PhysicallyDeletedRecords);
            _WriteRecords (response, info.NonActualizedRecords);
            _WriteRecords (response, info.LockedRecords);
            response.WriteInt32 (info.MaxMfn).NewLine();
            response.WriteInt32 (info.DatabaseLocked ? 1 : 0).NewLine();
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
                    nameof (DatabaseInfoCommand) + "::" + nameof (Execute)
                );

            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
