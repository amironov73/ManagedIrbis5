// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FormatCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class FormatCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatCommand()
            : base("Format")
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

            string argument = null;

            if (arguments.Length != 0)
            {
                argument = arguments[0].Text;
            }
            if (!string.IsNullOrEmpty(argument))
            {
                executive.DescriptionFormat = argument;

                if (executive.Provider.Connected
                    && executive.Records.Count != 0)
                {
                    int[] mfns = executive.Records.Select(r => r.Mfn)
                        .ToArray();
                    string[] formatted = executive.Provider.FormatRecords
                        (
                            mfns,
                            executive.DescriptionFormat
                        );
                    for (int i = 0; i < mfns.Length; i++)
                    {
                        executive.Records[i].Description = formatted[i];
                    }
                }
            }
            else
            {
                executive.WriteMessage(string.Format
                    (
                        "Format is: {0}",
                        executive.DescriptionFormat
                    ));
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
