﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DragMoveUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Provides an extra property for controls
    /// to enable or disable the DragMove behavior.
    /// </summary>
    /// <remarks>
    /// Borrowed from http://www.thomaslevesque.com/2009/05/06/windows-forms-automatically-drag-and-drop-controls-dragmove/
    /// </remarks>
    [ProvideProperty("EnableDragMove", typeof(Control))]
    public partial class DragMoveProvider
        : Component,
        IExtenderProvider
    {
        /// <summary>
        /// Initializes a new instance of the DragMoveProvider
        ///  class without a specified container.
        /// </summary>
        public DragMoveProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the DragMoveProvider
        /// class with a specified container.
        /// <param name="container">An IContainer that
        /// represents the container of the DragMoveProvider.
        /// </param>
        /// </summary>
        public DragMoveProvider
            (
                IContainer container
            )
        {
            container.Add(this);

            InitializeComponent();
        }

        #region IExtenderProvider Members

        /// <summary>
        /// Renvoie true si le DragMoveProvider peut fournir une propriété d'extension à l'objet cible spécifié.
        /// </summary>
        /// <param name="extendee">L'objet cible auquel ajouter une propriété d'extension.</param>
        /// <returns>true si le DragMoveProvider peut fournir une ou plusieurs propriété d'extensions ; sinon, false.</returns>
        public bool CanExtend(object extendee)
        {
            return extendee is Control;
        }

        #endregion

        /// <summary>
        /// Retrieves a value indicating whether
        /// the DragMove behavior is enabled
        /// on the specified control.
        /// </summary>
        /// <param name="control">The Control for which
        /// to retrieve DragMove status</param>
        /// <returns>true if the DragMove behavior
        /// is enabled for this control ; otherwise, false.
        /// </returns>
        [DefaultValue(false)]
        public bool GetEnableDragMove
            (
                Control control
            )
        {
            return control.IsDragMoveEnabled();
        }

        /// <summary>
        /// Enable or disables the DragMove behavior
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control for which
        /// to enable or disable the DragMove behavior.
        /// </param>
        /// <param name="value">A value indicating
        /// if the DragMove behavior must be enabled
        /// or disabled for this control.</param>
        public void SetEnableDragMove
            (
                Control control,
                bool value
            )
        {
            control.EnableDragMove(value);
        }
    }
}
