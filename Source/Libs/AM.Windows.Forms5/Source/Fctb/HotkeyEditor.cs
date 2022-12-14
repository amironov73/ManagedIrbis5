// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HotkeyEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using KEYS = System.Windows.Forms.Keys;

#endregion

#nullable enable

namespace Fctb;


internal sealed class HotkeyEditor
    : UITypeEditor
{
    /// <inheritdoc cref="UITypeEditor.GetEditStyle(System.ComponentModel.ITypeDescriptorContext)"/>
    public override UITypeEditorEditStyle GetEditStyle
        (
            ITypeDescriptorContext? context
        )
    {
        return UITypeEditorEditStyle.Modal;
    }

    /// <inheritdoc cref="UITypeEditor.EditValue(System.ComponentModel.ITypeDescriptorContext,System.IServiceProvider,object)"/>
    public override object EditValue
        (
            ITypeDescriptorContext? context,
            IServiceProvider? provider,
            object? value
        )
    {
        if ((provider is not null) &&
            (((IWindowsFormsEditorService?) provider.GetService (typeof (IWindowsFormsEditorService))) is not null))
        {
            var form = new HotkeysEditorForm (HotkeyMapping.Parse (value as string));

            if (form.ShowDialog() == DialogResult.OK)
            {
                value = form.GetHotkeys().ToString();
            }
        }

        return value;
    }
}
