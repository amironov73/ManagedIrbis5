// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

public class DockWindowCollection : ReadOnlyCollection<DockWindow>
{
    internal DockWindowCollection (DockPanel dockPanel)
        : base (new List<DockWindow>())
    {
        Items.Add (dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow (dockPanel, DockState.Document));
        Items.Add (dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow (dockPanel, DockState.DockLeft));
        Items.Add (dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow (dockPanel, DockState.DockRight));
        Items.Add (dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow (dockPanel, DockState.DockTop));
        Items.Add (dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow (dockPanel, DockState.DockBottom));
    }

    public DockWindow this [DockState dockState]
    {
        get
        {
            if (dockState == DockState.Document)
                return Items[0];
            else if (dockState == DockState.DockLeft || dockState == DockState.DockLeftAutoHide)
                return Items[1];
            else if (dockState == DockState.DockRight || dockState == DockState.DockRightAutoHide)
                return Items[2];
            else if (dockState == DockState.DockTop || dockState == DockState.DockTopAutoHide)
                return Items[3];
            else if (dockState == DockState.DockBottom || dockState == DockState.DockBottomAutoHide)
                return Items[4];

            throw (new ArgumentOutOfRangeException());
        }
    }
}