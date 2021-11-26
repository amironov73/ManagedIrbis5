// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

/* ReloadMasterFileCommand.cs -- реорганизация файла документов
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
    /// Реорганизация файла документов.
    /// </summary>
    public sealed class ReloadMasterFileCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReloadMasterFileCommand
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

                // TODO implement

                ServerResponse response = Data.Response.ThrowIfNull();

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
                        nameof (ReloadMasterFileCommand) + "::" + nameof (Execute),
                        exception
                    );
                SendError (-8888);
            }

            engine.OnAfterExecute (Data);
        }

        #endregion
    }
}
