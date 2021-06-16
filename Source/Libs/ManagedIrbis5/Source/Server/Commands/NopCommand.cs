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

/* NopCommand.cs -- пустая команда
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
    /// Пустая команда.
    /// </summary>
    public sealed class NopCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NopCommand
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
            var engine = Data.Engine.ThrowIfNull(nameof(Data.Engine));
            engine.OnBeforeExecute(Data);

            try
            {
                var context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

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
                Magna.TraceException(nameof(NopCommand) + "::" + nameof(Execute), exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);

        } // method Execute

        #endregion

    } // class NopCommand

} // namespace ManagedIrbis.Server.Commands
