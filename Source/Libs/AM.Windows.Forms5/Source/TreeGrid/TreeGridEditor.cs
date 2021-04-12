// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridEditor.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TreeGridEditor
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public abstract Control Control { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void SetValue(string value);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public abstract string GetValue();

        /// <summary>
        /// Selects the text.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public abstract void SelectText(int start, int length);

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public virtual void Dispose()
        {
            Control control = Control;
            if (control != null)
            {
                control.Dispose();
            }
        }

        #endregion
    }
}
