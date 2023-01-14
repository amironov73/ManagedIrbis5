// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ComponentBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Class that implements some object's properties such as location, size and visibility.
/// </summary>
public abstract partial class ComponentBase
    : Base
{
    #region Fields

    private AnchorStyles _anchor;
    private DockStyle _dock;
    private float _height;
    private float _left;
    private float _top;
    private bool _visible;
    private string _visibleExpression;
    private float _width;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets the absolute bottom coordinate of the object.
    /// </summary>
    [Browsable (false)]
    public float AbsBottom => AbsTop + Height;

    /// <summary>
    /// Gets the absolute bounding rectangle of the object.
    /// </summary>
    [Browsable (false)]
    public RectangleF AbsBounds => new (AbsLeft, AbsTop, Width, Height);

    /// <summary>
    /// Gets the absolute left coordinate of the object.
    /// </summary>
    [Browsable (false)]
    public virtual float AbsLeft => Parent is ComponentBase ? Left + ((ComponentBase) Parent).AbsLeft : Left;

    /// <summary>
    /// Gets the absolute right coordinate of the object.
    /// </summary>
    [Browsable (false)]
    public float AbsRight => AbsLeft + Width;

    /// <summary>
    /// Gets the absolute top coordinate of the object.
    /// </summary>
    [Browsable (false)]
    public virtual float AbsTop => Parent is ComponentBase ? Top + ((ComponentBase) Parent).AbsTop : Top;

    /// <summary>
    /// Gets or sets the edges of the container to which a control is bound and determines how a control
    /// is resized with its parent.
    /// </summary>
    /// <remarks>
    /// <para>Use the Anchor property to define how a control is automatically resized as its parent control
    /// is resized. Anchoring a control to its parent control ensures that the anchored edges remain in the
    /// same position relative to the edges of the parent control when the parent control is resized.</para>
    /// <para>You can anchor a control to one or more edges of its container. For example, if you have a band
    /// with a <b>TextObject</b> whose <b>Anchor</b> property value is set to <b>Top, Bottom</b>, the <b>TextObject</b> is stretched to
    /// maintain the anchored distance to the top and bottom edges of the band as the height of the band
    /// is increased.</para>
    /// </remarks>
    [DefaultValue (AnchorStyles.Left | AnchorStyles.Top)]
    public virtual AnchorStyles Anchor
    {
        get => _anchor;
        set => _anchor = value;
    }

    /// <summary>
    /// Gets the bottom coordinate of the object in relation to its container.
    /// </summary>
    /// <remarks>
    /// To change the bottom coordinate, change the <see cref="Top"/> and/or <see cref="Height"/> properties.
    /// </remarks>
    [Browsable (false)]
    public float Bottom => Top + Height;

    /// <summary>
    /// Gets or sets the bounding rectangle of the object.
    /// </summary>
    /// <remarks>
    /// Assigning a value to this property is equal to assigning values to the <see cref="Left"/>,
    /// <see cref="Top"/>, <see cref="Width"/>, <see cref="Height"/> properties.
    /// </remarks>
    [Browsable (false)]
    public RectangleF Bounds
    {
        get => new RectangleF (Left, Top, Width, Height);
        set
        {
            Left = value.Left;
            Top = value.Top;
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    /// Gets or sets the size of client area of the object.
    /// </summary>
    /// <remarks>
    /// This property is used in the <see cref="AM.Reporting.Dialog.DialogPage"/> class.
    /// </remarks>
    [Browsable (false)]
    public virtual SizeF ClientSize
    {
        get => new SizeF (Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    /// Gets or sets which control borders are docked to its parent control and determines how a control
    /// is resized with its parent.
    /// </summary>
    /// <remarks>
    /// <para>Use the <b>Dock</b> property to define how a control is automatically resized as its parent control is
    /// resized. For example, setting Dock to <c>DockStyle.Left</c> causes the control to align itself with the
    /// left edges of its parent control and to resize as the parent control is resized.</para>
    /// <para>A control can be docked to one edge of its parent container or can be docked to all edges and
    /// fill the parent container.</para>
    /// </remarks>
    [DefaultValue (DockStyle.None)]
    [Category ("Layout")]
    public virtual DockStyle Dock
    {
        get => _dock;
        set
        {
            if (_dock != value)
            {
                _dock = value;
                if (Parent != null)
                {
                    ((IParent) Parent).UpdateLayout (0, 0);
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets a group index.
    /// </summary>
    /// <remarks>
    /// Group index is used to group objects in the designer (using "Group" button). When you select
    /// any object in a group, entire group becomes selected. To reset a group, set the <b>GroupIndex</b>
    /// to 0 (default value).
    /// </remarks>
    [Browsable (false)]
    public int GroupIndex { get; set; }

    /// <summary>
    /// Gets or sets the height of the object.
    /// </summary>
    /// <remarks>
    /// This property value is measured in the screen pixels. Use <see cref="Units"/> class to
    /// convert a value to desired units.
    /// </remarks>
    /// <example>The following example demonstrates how to convert between pixels and units:<code>
    /// TextObject text1;
    /// // set Height to 10mm
    /// text1.Height = Units.Millimeters * 10;
    /// // convert a value to millimeters
    /// MessageBox.Show("Height = " + (text1.Height / Units.Millimeters).ToString() + "mm");
    /// </code></example>
    [Category ("Layout")]
    public virtual float Height
    {
        get => _height;
        set
        {
            value = (float)Math.Round (value, 2);
            if (FloatDiff (_height, value))
            {
                if (!IsDesigning || !HasRestriction (Restrictions.DontResize))
                {
                    if (this is IParent parent)
                    {
                        parent.UpdateLayout (0, value - _height);
                    }

                    _height = value;
                    if (Dock != DockStyle.None && Parent != null)
                    {
                        ((IParent) Parent).UpdateLayout (0, 0);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the left coordinate of the object in relation to its container.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///         This property value is measured in the screen pixels. Use
    ///         <see cref="Units"/> class to convert a value to desired units.
    ///     </para>
    ///   <para>
    ///         To obtain absolute coordinate, use <see cref="AbsLeft"/> property.
    ///     </para>
    /// </remarks>
    /// <example>The following example demonstrates how to convert between pixels and units:<code>
    /// TextObject text1;
    /// // set Left to 10mm
    /// text1.Left = Units.Millimeters * 10;
    /// // convert a value to millimeters
    /// MessageBox.Show("Left = " + (text1.Left / Units.Millimeters).ToString() + "mm");
    /// </code></example>
    [Category ("Layout")]
    public virtual float Left
    {
        get => _left;
        set
        {
            value = (float)Math.Round (value, 2);
            if (!IsDesigning || !HasRestriction (Restrictions.DontMove))
            {
                _left = value;
                if (Dock != DockStyle.None && Parent != null)
                {
                    ((IParent) Parent).UpdateLayout (0, 0);
                }
            }
        }
    }

    /// <summary>
    /// Gets the right coordinate of the object in relation to its container.
    /// </summary>
    /// <remarks>
    /// To change the right coordinate, change the <see cref="Left"/> and/or <see cref="Width"/> properties.
    /// </remarks>
    [Browsable (false)]
    public float Right => Left + Width;

    /// <summary>
    /// Gets or sets the Tag string for this component.
    /// </summary>
    [Category ("Design")]
    public string Tag { get; set; }

    /// <summary>
    /// Gets or sets the top coordinate of the object in relation to its container.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///         This property value is measured in the screen pixels. Use
    ///         <see cref="Units"/> class to convert a value to desired units.
    ///     </para>
    ///   <para>
    ///         To obtain absolute coordinate, use <see cref="AbsTop"/> property.
    ///     </para>
    /// </remarks>
    /// <example>The following example demonstrates how to convert between pixels and units:<code>
    /// TextObject text1;
    /// // set Top to 10mm
    /// text1.Top = Units.Millimeters * 10;
    /// // convert a value to millimeters
    /// MessageBox.Show("Top = " + (text1.Top / Units.Millimeters).ToString() + "mm");
    /// </code></example>
    [Category ("Layout")]
    public virtual float Top
    {
        get => _top;
        set
        {
            value = (float)Math.Round (value, 2);
            if (!IsDesigning || !HasRestriction (Restrictions.DontMove))
            {
                _top = value;
                if (Dock != DockStyle.None && Parent != null)
                {
                    ((IParent) Parent).UpdateLayout (0, 0);
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the object is displayed in the preview window.
    /// </summary>
    /// <remarks>
    /// Setting this property to <b>false</b> will hide the object in the preview window.
    /// </remarks>
    /// <example>The following report script will control the Text1 visibility depending on the value of the
    /// data column:<code>
    /// private void Data1_BeforePrint(object sender, EventArgs e)
    /// {
    ///   Text1.Visible = [Orders.Shipped] == true;
    /// }
    /// </code></example>
    [DefaultValue (true)]
    public virtual bool Visible
    {
        get => _visible;
        set => _visible = value;
    }

    /// <summary>
    /// Gets or sets a string containing expression that determines should be object displayed in the preview window.
    /// </summary>
    [DefaultValue ("")]
    public virtual string VisibleExpression
    {
        get => _visibleExpression;
        set => _visibleExpression = value;
    }

    /// <summary>
    /// Gets or sets a value that determines if the object can be printed on the printer.
    /// </summary>
    /// <remarks>
    /// Object with Printable = <b>false</b> is still visible in the preview window, but not on the printout.
    /// If you want to hide an object in the preview, set the <see cref="ComponentBase.Visible"/> property to <b>false</b>.
    /// </remarks>
    [DefaultValue (true)]
    public bool Printable { get; set; }

    /// <summary>
    /// Gets or sets a string containing expression that determines should be object printed on the printer.
    /// </summary>
    [DefaultValue ("")]
    public string PrintableExpression { get; set; }

    /// <summary>
    /// Gets or sets the width of the object.
    /// </summary>
    /// <remarks>
    /// This property value is measured in the screen pixels. Use <see cref="Units"/> class to
    /// convert a value to desired units.
    /// </remarks>
    /// <example>The following example demonstrates how to convert between pixels and units:<code>
    /// TextObject text1;
    /// // set Width to 10mm
    /// text1.Width = Units.Millimeters * 10;
    /// // convert a value to millimeters
    /// MessageBox.Show("Width = " + (text1.Width / Units.Millimeters).ToString() + "mm");
    /// </code></example>
    public virtual float Width
    {
        get => _width;
        set
        {
            value = (float)Math.Round (value, 2);
            if (FloatDiff (_width, value))
            {
                if (!IsDesigning || !HasRestriction (Restrictions.DontResize))
                {
                    if (this is IParent parent)
                    {
                        parent.UpdateLayout (value - _width, 0);
                    }

                    _width = value;
                    if (Dock != DockStyle.None && Parent != null)
                    {
                        ((IParent) Parent).UpdateLayout (0, 0);
                    }
                }
            }
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentBase"/> class with default settings.
    /// </summary>
    protected ComponentBase()
    {
        _anchor = AnchorStyles.Left | AnchorStyles.Top;
        _visible = true;
        _visibleExpression = string.Empty;
        Printable = true;
        PrintableExpression = string.Empty;
        SetFlags (Flags.CanWriteBounds | Flags.HasGlobalName, true);
        Tag = "";
    }

    #endregion Constructors

    #region Public Methods

    /// <inheritdoc/>
    public override void Assign (Base source)
    {
        base.Assign (source);

        var src = (ComponentBase) source;
        Left = src.Left;
        Top = src.Top;
        Width = src.Width;
        Height = src.Height;
        Dock = src.Dock;
        Anchor = src.Anchor;
        Visible = src.Visible;
        VisibleExpression = src.VisibleExpression;
        Printable = src.Printable;
        PrintableExpression = src.PrintableExpression;
        Tag = src.Tag;
    }

    /// <inheritdoc/>
    public override void Serialize (ReportWriter writer)
    {
        var c = (ComponentBase) writer.DiffObject!;
        base.Serialize (writer);

        if (Printable != c.Printable)
        {
            writer.WriteBool ("Printable", Printable);
        }

        if (PrintableExpression != c.PrintableExpression)
        {
            writer.WriteStr ("PrintableExpression", PrintableExpression);
        }

        if (HasFlag (Flags.CanWriteBounds))
        {
            if (FloatDiff (Left, c.Left))
            {
                writer.WriteFloat ("Left", Left);
            }

            if (FloatDiff (Top, c.Top))
            {
                writer.WriteFloat ("Top", Top);
            }

            if (FloatDiff (Width, c.Width))
            {
                writer.WriteFloat ("Width", Width);
            }

            if (FloatDiff (Height, c.Height))
            {
                writer.WriteFloat ("Height", Height);
            }
        }

        if (writer.SerializeTo != SerializeTo.Preview)
        {
            if (Dock != c.Dock)
            {
                writer.WriteValue ("Dock", Dock);
            }

            if (Anchor != c.Anchor)
            {
                writer.WriteValue ("Anchor", Anchor);
            }

            if (Visible != c.Visible)
            {
                writer.WriteBool ("Visible", Visible);
            }

            if (VisibleExpression != c.VisibleExpression)
            {
                writer.WriteStr ("VisibleExpression", VisibleExpression);
            }

            if (GroupIndex != c.GroupIndex)
            {
                writer.WriteInt ("GroupIndex", GroupIndex);
            }
        }

        if (Tag != c.Tag)
        {
            writer.WriteStr ("Tag", Tag);
        }
    }

    #endregion Public Methods

    #region Report Engine

    /// <inheritdoc/>
    public override string[] GetExpressions()
    {
        var expressions = new List<string>();
        var baseExpressions = base.GetExpressions();
        if (baseExpressions != null)
        {
            expressions.AddRange (baseExpressions);
        }

        if (!string.IsNullOrEmpty (VisibleExpression))
        {
            var expression = Code.CodeUtils.FixExpressionWithBrackets (VisibleExpression);
            if (expression.ToLower() == "true" || expression.ToLower() == "false")
            {
                expression = expression.ToLower();
            }

            expressions.Add (expression);
        }

        if (!string.IsNullOrEmpty (PrintableExpression))
        {
            var expression = Code.CodeUtils.FixExpressionWithBrackets (PrintableExpression);
            if (expression.ToLower() == "true" || expression.ToLower() == "false")
            {
                expression = expression.ToLower();
            }

            expressions.Add (expression);
        }

        return expressions.ToArray();
    }

    #endregion Report Engine
}
