// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NavigationButtonDesigner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AeroSuite.Controls.Design;

/// <summary>
/// Provides a ControlDesigner for the <see cref="NavigationButton"/> Control.
/// </summary>
internal class NavigationButtonDesigner
    : ControlDesignerBase<NavigationButton>
{
    /// <summary>
    /// Initializes the control designer with the specified target control.
    /// </summary>
    /// <param name="target">The target control.</param>
    protected override void InitializeInternal
        (
            NavigationButton target
        )
    {
        ActionList = new NavigationButtonActionList (target);
        MainSelectionRules = SelectionRules.Moveable;
    }
}

/// <summary>
/// Provides an ActionList for the <see cref="NavigationButton"/> Control.
/// </summary>
internal class NavigationButtonActionList
    : DesignerActionListBase<NavigationButton>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationButtonActionList"/> class.
    /// </summary>
    /// <param name="component">A component related to the <see cref="T:System.ComponentModel.Design.DesignerActionList" />.</param>
    public NavigationButtonActionList (IComponent component)
        : base (component)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Returns the items of this designer action list.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    protected override IEnumerable<DesignerActionItem> Items
    {
        get { yield return CreateItem (n => n.Type); }
    }

    public NavigationButtonType Type
    {
        get => Control!.Type;
        set
        {
            Control!.Type = value;
            RefreshControl();
        }
    }
}
