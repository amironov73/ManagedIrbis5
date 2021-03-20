// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CsFileCommand.cs --
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
    public sealed class CsFileCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsFileCommand()
            : base("CSFile")
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
                    MethodInfo main = SharpRunner.CompileFile
                        (
                            argument,
                            err => executive.WriteLine(err)
                        );

                    if (!ReferenceEquals(main, null))
                    {
                        main.Invoke(null, null);
                    }
                }
            }

#endif

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}

