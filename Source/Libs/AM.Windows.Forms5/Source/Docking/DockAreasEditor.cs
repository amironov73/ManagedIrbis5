// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

internal class DockAreasEditor
    : UITypeEditor
{
    private class DockAreasEditorControl : UserControl
    {
        private readonly CheckBox _checkBoxFloat;
        private readonly CheckBox _checkBoxDockLeft;
        private readonly CheckBox _checkBoxDockRight;
        private readonly CheckBox _checkBoxDockTop;
        private readonly CheckBox _checkBoxDockBottom;
        private readonly CheckBox _checkBoxDockFill;
        private DockAreas _mOldDockAreas;

        public DockAreas DockAreas
        {
            get
            {
                DockAreas dockAreas = 0;
                if (_checkBoxFloat.Checked)
                {
                    dockAreas |= DockAreas.Float;
                }

                if (_checkBoxDockLeft.Checked)
                {
                    dockAreas |= DockAreas.DockLeft;
                }

                if (_checkBoxDockRight.Checked)
                {
                    dockAreas |= DockAreas.DockRight;
                }

                if (_checkBoxDockTop.Checked)
                {
                    dockAreas |= DockAreas.DockTop;
                }

                if (_checkBoxDockBottom.Checked)
                {
                    dockAreas |= DockAreas.DockBottom;
                }

                if (_checkBoxDockFill.Checked)
                {
                    dockAreas |= DockAreas.Document;
                }

                if (dockAreas == 0)
                {
                    return _mOldDockAreas;
                }
                else
                {
                    return dockAreas;
                }
            }
        }

        public DockAreasEditorControl()
        {
            _checkBoxFloat = new CheckBox();
            _checkBoxDockLeft = new CheckBox();
            _checkBoxDockRight = new CheckBox();
            _checkBoxDockTop = new CheckBox();
            _checkBoxDockBottom = new CheckBox();
            _checkBoxDockFill = new CheckBox();

            SuspendLayout();

            _checkBoxFloat.Appearance = Appearance.Button;
            _checkBoxFloat.Dock = DockStyle.Top;
            _checkBoxFloat.Height = 24;
            _checkBoxFloat.Text = Strings.DockAreaEditor_FloatCheckBoxText;
            _checkBoxFloat.TextAlign = ContentAlignment.MiddleCenter;
            _checkBoxFloat.FlatStyle = FlatStyle.System;

            _checkBoxDockLeft.Appearance = Appearance.Button;
            _checkBoxDockLeft.Dock = DockStyle.Left;
            _checkBoxDockLeft.Width = 24;
            _checkBoxDockLeft.FlatStyle = FlatStyle.System;

            _checkBoxDockRight.Appearance = Appearance.Button;
            _checkBoxDockRight.Dock = DockStyle.Right;
            _checkBoxDockRight.Width = 24;
            _checkBoxDockRight.FlatStyle = FlatStyle.System;

            _checkBoxDockTop.Appearance = Appearance.Button;
            _checkBoxDockTop.Dock = DockStyle.Top;
            _checkBoxDockTop.Height = 24;
            _checkBoxDockTop.FlatStyle = FlatStyle.System;

            _checkBoxDockBottom.Appearance = Appearance.Button;
            _checkBoxDockBottom.Dock = DockStyle.Bottom;
            _checkBoxDockBottom.Height = 24;
            _checkBoxDockBottom.FlatStyle = FlatStyle.System;

            _checkBoxDockFill.Appearance = Appearance.Button;
            _checkBoxDockFill.Dock = DockStyle.Fill;
            _checkBoxDockFill.FlatStyle = FlatStyle.System;

            Controls.AddRange (new Control[]
            {
                _checkBoxDockFill,
                _checkBoxDockBottom,
                _checkBoxDockTop,
                _checkBoxDockRight,
                _checkBoxDockLeft,
                _checkBoxFloat
            });

            Size = new Size (160, 144);
            BackColor = SystemColors.Control;
            ResumeLayout();
        }

        public void SetStates (DockAreas dockAreas)
        {
            _mOldDockAreas = dockAreas;
            if ((dockAreas & DockAreas.DockLeft) != 0)
            {
                _checkBoxDockLeft.Checked = true;
            }

            if ((dockAreas & DockAreas.DockRight) != 0)
            {
                _checkBoxDockRight.Checked = true;
            }

            if ((dockAreas & DockAreas.DockTop) != 0)
            {
                _checkBoxDockTop.Checked = true;
            }

            if ((dockAreas & DockAreas.DockTop) != 0)
            {
                _checkBoxDockTop.Checked = true;
            }

            if ((dockAreas & DockAreas.DockBottom) != 0)
            {
                _checkBoxDockBottom.Checked = true;
            }

            if ((dockAreas & DockAreas.Document) != 0)
            {
                _checkBoxDockFill.Checked = true;
            }

            if ((dockAreas & DockAreas.Float) != 0)
            {
                _checkBoxFloat.Checked = true;
            }
        }
    }

    private DockAreasEditorControl? _ui;

    public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext? context)
    {
        return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue (ITypeDescriptorContext? context, IServiceProvider provider, object? value)
    {
        _ui ??= new DockAreasEditorControl();

        _ui.SetStates ((DockAreas) value!);

        IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService (typeof (IWindowsFormsEditorService))!;
        edSvc.DropDownControl (_ui);

        return _ui.DockAreas;
    }
}
