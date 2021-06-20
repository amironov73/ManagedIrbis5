// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripPrefixComboBox.cs -- выпадающий список поисковых сценариев ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace WinFormsExample
{
    /// <summary>
    /// Выпадающий список поисковых сценариев ИРБИС64.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public sealed class ToolStripPrefixComboBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Внутренний контрол <see cref="PrefixComboBox"/>.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PrefixComboBox ComboBox => (PrefixComboBox)Control;

        #endregion

        #region Construction

        public ToolStripPrefixComboBox()
            : base(new PrefixComboBox())
        {
        }

        #endregion

    } // class ToolStripPrefixComboBox

} // namespace WinFormsExample
