// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HeaderlessTabControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A TabControl-style control that does not have headers.
/// </summary>
/// <remarks>
/// Instead of using the usual approach (suppressing the TCM_ADJUSTRECT message),
/// I redid the TabControl completely to eliminate any bugs and to make
/// it work on every platform.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Headerless Tab Control")]
[Description ("A TabControl-style control that does not have headers.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (HeaderlessTabControl))]
public class HeaderlessTabControl
    : Control
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderlessTabControl"/> class.
    /// </summary>
    public HeaderlessTabControl()
    {
        TabPages = new ObservableCollection<HeaderlessTabPage>
        {
            new ()
        };
    }

    private ObservableCollection<HeaderlessTabPage> _tabPages;

    /// <summary>
    /// Returns the collection of tab pages in this tab control.
    /// </summary>
    /// <value>
    /// The tab pages.
    /// </value>
    [Category ("Appearance")]
    [Description ("The tab pages in this tab control.")]
    [Localizable (true)]
    [Bindable (true)]
    public ObservableCollection<HeaderlessTabPage> TabPages
    {
        get => _tabPages;
        private set
        {
            if (_tabPages != null)
            {
                _tabPages.CollectionChanged -= OnTabPageCollectionChanged;
            }

            _tabPages = value;
            _tabPages.CollectionChanged += OnTabPageCollectionChanged;
        }
    }

    /// <summary>
    /// Called when the tab page collection has been changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnTabPageCollectionChanged
        (
            object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs eventArgs
        )
    {
        UpdateTabPage();
    }

    /// <summary>
    /// Gets or sets the update tab page.
    /// </summary>
    /// <value>
    /// The update tab page.
    /// </value>
    protected virtual void UpdateTabPage()
    {
        //Remove old tab page
        Controls.OfType<HeaderlessTabPage>().FirstOrDefault()
            .DoIf (tab => tab != null, tab => Controls.Remove (tab));

        //Add tab to form if one is selected
        SelectedTab.DoIf (tab => tab != null, tab =>
        {
            tab.Dock = DockStyle.Fill;
            Controls.Add (tab);
        });
    }

    private int _selectedIndex = -1;

    /// <summary>
    /// Gets the index of the selected tab.
    /// </summary>
    /// <value>
    /// The index of the selected tab.
    /// </value>
    public virtual int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            UpdateTabPage();
        }
    }

    /// <summary>
    /// Returns the selected tab.
    /// </summary>
    /// <value>
    /// The selected tab.
    /// </value>
    /// <exception cref="System.Exception">SelectedIndex returned an invalid index.</exception>
    public virtual HeaderlessTabPage? SelectedTab
    {
        get
        {
            if (SelectedIndex == -1)
            {
                return null;
            }

            if (SelectedIndex >= TabPages.Count)
            {
                throw new Exception ("SelectedIndex returned an invalid index.");
            }
            return TabPages[SelectedIndex];
        }
        set => SelectedIndex = TabPages.IndexOf (value);
    }

    /// <summary>
    /// Extends the design mode behaviour of a <see cref="HeaderlessTabControl"/>.
    /// </summary>
    internal class HeaderlessTabControlDesigner
        : ParentControlDesigner
    {
        /// <summary>
        /// Initializes the designer with the specified component.
        /// </summary>
        /// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to associate with the designer.</param>
        public override void Initialize
            (
                IComponent component
            )
        {
            base.Initialize (component);

            if (component is HeaderlessTabControl tabControl)
            {
                EnableDesignMode (tabControl.SelectedTab, "SelectedTab");
            }
        }
    }
}
