﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BangCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BangCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BangCommand()
            : base("!")
        {
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
            OnBeforeExecute();

            string? source = null;
            if (arguments.Length != 0)
            {
                source = arguments[0].Text;
            }

            if (!string.IsNullOrEmpty(source))
            {
                var context = executive.Context;
                context.ClearAll();
                var formatter = new PftFormatter(context);
                formatter.ParseProgram(source);
                var output = formatter.FormatRecord(new Record());
                executive.WriteLine(output);
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    }
}
