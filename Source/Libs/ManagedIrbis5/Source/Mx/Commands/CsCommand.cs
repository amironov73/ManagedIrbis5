﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reflection;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class CsCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsCommand()
            : base("CS")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc/>
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

#if CLASSIC || NETCORE

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                if (!string.IsNullOrEmpty(argument))
                {
                    MethodInfo method = SharpRunner.CompileSnippet
                        (
                            argument,
                            "MxExecutive executive",
                            err => executive.WriteLine(err)
                        );
                    if (!ReferenceEquals(method, null))
                    {
                        method.Invoke(null, new object[]{executive});
                    }
                }
            }

#endif

            OnAfterExecute();

            return true;
        }

        #endregion

    }
}

