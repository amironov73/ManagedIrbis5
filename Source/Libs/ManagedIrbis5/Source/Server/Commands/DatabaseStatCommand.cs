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

/* DatabaseStatCommand.cs -- статистика по базе данных
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
    /// Статистика по базе данных.
    /// </summary>
    public class DatabaseStatCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DatabaseStatCommand
            (
                WorkData data
            )
            : base(data)
        {
        } // constructor

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            var engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                var context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

                var request = Data.Request.ThrowIfNull();

                // TODO implement

                var response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(DatabaseStatCommand) + "::" + nameof(Execute), exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);

        } // method Execute

        #endregion

    } // class DatabaseStatCommands

} // namespace ManagedIrbis.Server.Commands
