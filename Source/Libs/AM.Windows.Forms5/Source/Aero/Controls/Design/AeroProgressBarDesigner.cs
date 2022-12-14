// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AeroProgressBarDesigner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace AeroSuite.Controls.Design;

/// <summary>
/// Provides a ControlDesigner for the <see cref="AeroProgressBar"/> Control.
/// </summary>
internal class AeroProgressBarDesigner
    : ControlDesignerBase<AeroProgressBar>
{
    /// <summary>
    /// Initializes the control designer with the specified target control.
    /// </summary>
    /// <param name="target">The target control.</param>
    protected override void InitializeInternal
        (
            AeroProgressBar target
        )
    {
        Sure.NotNull (target);

        ActionList = new AeroProgressBarActionList (target);
    }
}

/// <summary>
/// Provides an ActionList for the <see cref="AeroProgressBar"/> Control.
/// </summary>
internal class AeroProgressBarActionList
    : DesignerActionListBase<AeroProgressBar>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AeroProgressBarActionList"/> class.
    /// </summary>
    /// <param name="component">A component related to the <see cref="T:System.ComponentModel.Design.DesignerActionList" />.</param>
    public AeroProgressBarActionList
        (
            IComponent component
        )
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
        get
        {
            yield return CreateItem (c => c.Value);
            yield return CreateItem (c => c.State);
            yield return CreateItem (c => c.Style);
        }
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public int Value
    {
        get => Control!.Value;
        set
        {
            Control!.Value = value;
            RefreshControl();
        }
    }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    public ProgressBarState State
    {
        get => Control!.State;
        set
        {
            Control!.State = value;
            RefreshControl();
        }
    }

    /// <summary>
    /// Gets or sets the style.
    /// </summary>
    /// <value>
    /// The style.
    /// </value>
    public ProgressBarStyle Style
    {
        get => Control!.Style;
        set
        {
            Control!.Style = value;
            RefreshControl();
        }
    }
}
