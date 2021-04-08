// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftUiDebugger.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Pft
{
    /// <summary>
    /// PFT debugger for WinForms.
    /// </summary>
    public sealed class PftUiDebugger
        : PftDebugger
    {
        #region Properties

        /// <summary>
        /// Form.
        /// </summary>
        public PftDebuggerForm Form { get { return _form; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUiDebugger
            (
                PftContext context
            )
            : base(context)
        {
            _form = new PftDebuggerForm();
        }

        #endregion

        #region Private members

        private readonly PftDebuggerForm _form;

        #endregion

        #region PftDebugger members

        /// <inheritdoc/>
        public override void Activate
            (
                PftDebugEventArgs eventArgs
            )
        {
            _form.ShowDialog();
        }

        #endregion
    }
}
