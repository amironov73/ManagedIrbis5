﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CsFileCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */


#region Using directives

#if CLASSIC || NETCORE

using System.Reflection;

#endif

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

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

