// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConsoleControlOutput.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Output to ConsoleControl.
    /// </summary>
    public sealed class ConsoleControlOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Console control.
        /// </summary>
        public ConsoleControl ConsoleControl { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControlOutput
            (
                ConsoleControl console
            )
        {
            ConsoleControl = console;
        }

        #endregion

        #region AbstractOutput members

        /// <inheritdoc />
        public override bool HaveError { get; set; }

        /// <inheritdoc />
        public override AbstractOutput Clear()
        {
            ConsoleControl.Clear();
            HaveError = false;

            return this;
        }

        /// <inheritdoc />
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override AbstractOutput Write
            (
                string text
            )
        {
            ConsoleControl.Write(text);

            return this;
        }

        /// <inheritdoc />
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            ConsoleControl.Write(Color.Red, text);
            HaveError = true;

            return this;
        }

        #endregion
    }
}
