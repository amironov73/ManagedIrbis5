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

/* CreateDatabaseCommand.cs -- создание базы данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Создание базы данных.
    /// </summary>
    public sealed class CreateDatabaseCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CreateDatabaseCommand
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
                var database = request.RequireAnsiString();
                database.NotUsed();
                var description = request.GetAnsiString();
                description.NotUsed();
                var readerAccess = request.GetInt32();
                readerAccess.NotUsed();

                // Response is (ANSI):
                // 0
                // NewDB NEWDB,New database,0 - Создана новая БД NEWDB
                // CloseDB -
                // Exit C:\IRBIS64_2015\workdir\1126_0.ibf

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
                Magna.TraceException
                    (
                        nameof (CreateDatabaseCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);
        }

        #endregion
    }
}
