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

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking
{
    public partial class DockPanel
    {
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelSkin")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Obsolete("Use Theme.Skin instead.")]
        public DockPanelSkin Skin
        {
            get { return null;  }
        }

        private ThemeBase m_dockPanelTheme = new DefaultTheme();

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelTheme")]
        public ThemeBase Theme
        {
            get { return m_dockPanelTheme; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (m_dockPanelTheme.GetType() == value.GetType())
                {
                    return;
                }

                m_dockPanelTheme?.CleanUp(this);
                m_dockPanelTheme = value;
                m_dockPanelTheme.ApplyTo(this);
                m_dockPanelTheme.PostApply(this);
            }
        }
    }
}
