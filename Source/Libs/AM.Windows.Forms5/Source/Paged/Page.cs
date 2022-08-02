// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* Page.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#pragma warning disable CS0067

#nullable enable

namespace Manina.Windows.Forms;

/// <summary>
///
/// </summary>
[ToolboxItem (false), DesignTimeVisible (false)]
[Designer (typeof (PageDesigner))]
[Docking (DockingBehavior.Never)]
public class Page
    : Control
{
    #region Properties

    /// <summary>
    /// Gets or sets the background color for the control.
    /// </summary>
    [Category ("Appearance"), DefaultValue (typeof (Color), "Window")]
    [Description ("Gets or sets the background color for the control.")]
    public override Color BackColor
    {
        get => base.BackColor;
        set => base.BackColor = value;
    }

    /// <summary>
    ///
    /// </summary>
    public new PagedControl? Parent => (PagedControl?) base.Parent;

    #endregion

    #region Unused Methods - Hide From User

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override DockStyle Dock
    {
        get => DockStyle.None;
        set { }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override AnchorStyles Anchor
    {
        get => AnchorStyles.None;
        set { }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override Size MinimumSize
    {
        get => new Size (0, 0);
        set { }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override Size MaximumSize
    {
        get => new Size (0, 0);
        set { }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new bool Visible
    {
        get => base.Visible;
        set => base.Visible = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new bool Enabled
    {
        get => base.Enabled;
        set => base.Enabled = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new bool TabStop
    {
        get => true;
        set => base.TabStop = true;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new int TabIndex
    {
        get => 0;
        set => base.TabIndex = 0;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new Padding Margin
    {
        get => base.Margin;
        set { }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), Category ("Layout"), Localizable (true)]
    public new Point Location
    {
        get => base.Location;
        set => base.Location = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), Category ("Layout"), Localizable (true)]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler DockChanged;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler TabIndexChanged;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler TabStopChanged;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler PaddingChanged;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler MarginChanged;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler Move;

    #endregion

    #region Overriden Methods

    /// <inheritdoc cref="Control.CreateControlsInstance"/>
    protected override ControlCollection CreateControlsInstance()
    {
        return new PageControlCollection (this);
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint (PaintEventArgs e)
    {
        if (DesignMode && Visible)
        {
            var rect = ClientRectangle;
            rect.Inflate (-2, -2);
            ControlPaint.DrawBorder (e.Graphics, rect, Color.Black, ButtonBorderStyle.Dashed);
        }

        base.OnPaint (e);

        if (Parent != null)
        {
            Parent.OnPagePaint (new PagePaintEventArgs (e.Graphics, this));
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Page"/> class.
    /// </summary>
    public Page()
    {
        SetStyle (ControlStyles.ResizeRedraw, true);
        BackColor = SystemColors.Window;
    }

    #endregion

    #region ControlCollection

    internal class PageControlCollection : ControlCollection
    {
        public PageControlCollection (Page ownerControl) : base (ownerControl)
        {
        }

        public override void Add (Control value)
        {
            if (value is Page)
            {
                string className = typeof (Page).Name;
                throw new ArgumentException (string.Format ("Cannot add a {0} as a child control of another {0}.",
                    className));
            }

            base.Add (value);
        }
    }

    #endregion

    #region ControlDesigner

    /// <summary>
    ///
    /// </summary>
    protected internal class PageDesigner
        : ParentControlDesigner
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public override SelectionRules SelectionRules => SelectionRules.Locked;

        #endregion

        #region Parent/Child Relation

        /// <summary>
        ///
        /// </summary>
        public override bool CanBeParentedTo (IDesigner parentDesigner)
        {
            return parentDesigner != null && parentDesigner.Component is PagedControl;
        }

        #endregion

        #region Drag Events

        /// <summary>
        ///
        /// </summary>
        public new void OnDragEnter (DragEventArgs de)
        {
            base.OnDragEnter (de);
        }

        /// <summary>
        ///
        /// </summary>
        public new void OnDragOver (DragEventArgs de)
        {
            base.OnDragOver (de);
        }

        /// <summary>
        ///
        /// </summary>
        public new void OnDragLeave (EventArgs e)
        {
            base.OnDragLeave (e);
        }

        /// <summary>
        ///
        /// </summary>
        public new void OnDragDrop (DragEventArgs de)
        {
            base.OnDragDrop (de);
        }

        /// <summary>
        ///
        /// </summary>
        public new void OnGiveFeedback (GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback (e);
        }

        /// <summary>
        ///
        /// </summary>
        public new void OnDragComplete (DragEventArgs de)
        {
            base.OnDragComplete (de);
        }

        #endregion
    }

    #endregion
}
