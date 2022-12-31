// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BandBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;

using AM.Reporting.Utils;

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Базовый класс для всех полос.
/// </summary>
public abstract partial class BandBase
    : BreakableComponent, IParent
{
    #region Fields

    private ChildBand? _child;
    private int _rowNumber;
    private int _absRowNo;
    private bool _repeated;
    private bool _updatingLayout;
    private bool _flagCheckFreeSpace;
    private int _savedOriginalObjectsCount;

    #endregion

    #region Properties

    /// <summary>
    /// This event occurs before the band layouts its child objects.
    /// </summary>
    public event EventHandler? BeforeLayout;

    /// <summary>
    /// This event occurs after the child objects layout was finished.
    /// </summary>
    public event EventHandler? AfterLayout;

    /// <summary>
    /// Gets or sets a value indicating that the band should be printed from a new page.
    /// </summary>
    /// <remarks>
    /// New page is not generated when printing very first group or data row. This is made to avoid empty
    /// first page.
    /// </remarks>
    [DefaultValue (false)]
    [Category ("Behavior")]
    public bool StartNewPage { get; set; }

    /// <summary>
    /// Gets or sets a value that determines the number of repetitions of the same band.
    /// </summary>
    [Category ("Behavior")]
    [DefaultValue (1)]
    public int RepeatBandNTimes { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating that the first row can start a new report page.
    /// </summary>
    /// <remarks>
    /// Use this property if <see cref="StartNewPage"/> is set to <b>true</b>. Normally the new page
    /// is not started when printing the first data row, to avoid empty first page.
    /// </remarks>
    [DefaultValue (true)]
    [Category ("Behavior")]
    public bool FirstRowStartsNewPage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the band should be printed on the page bottom.
    /// </summary>
    [DefaultValue (false)]
    [Category ("Behavior")]
    public bool PrintOnBottom { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the band should be printed together with its child band.
    /// </summary>
    [DefaultValue (false)]
    [Category ("Behavior")]
    public bool KeepChild { get; set; }

    /// <summary>
    /// Gets or sets an outline expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Outline is a tree control displayed in the preview window. It represents the prepared report structure.
    /// Each outline node can be clicked to navigate to the item in the prepared report.
    /// </para>
    /// <para>
    /// To create the outline, set this property to any valid expression that represents the outline node text.
    /// This expression will be calculated when band is about to print, and its value will be added to the
    /// outline. Thus, nodes' hierarchy in the outline is similar to the bands' hierarchy
    /// in a report. That means there will be the main and subordinate outline nodes, corresponding
    /// to the main and subordinate bands in a report (a report with two levels of data or with groups can
    /// exemplify the point).
    /// </para>
    /// </remarks>
    [Category ("Navigation")]
    public string OutlineExpression { get; set; }

    /// <summary>
    /// Gets or sets a child band that will be printed right after this band.
    /// </summary>
    /// <remarks>
    /// Typical use of child band is to print several objects that can grow or shrink. It also can be done
    /// using the shift feature (via <see cref="ShiftMode"/> property), but in some cases it's not possible.
    /// </remarks>
    [Browsable (false)]
    public ChildBand? Child
    {
        get => _child;
        set
        {
            SetProp (_child, value);
            _child = value;
        }
    }

    /// <summary>
    /// Gets a collection of report objects belongs to this band.
    /// </summary>
    [Browsable (false)]
    public ReportComponentCollection Objects { get; }

    /// <summary>
    /// Gets a value indicating that band is reprinted on a new page.
    /// </summary>
    /// <remarks>
    /// This property is applicable to the <b>DataHeaderBand</b> and <b>GroupHeaderBand</b> only.
    /// It returns <b>true</b> if its <b>RepeatOnAllPages</b> property is <b>true</b> and band is
    /// reprinted on a new page.
    /// </remarks>
    [Browsable (false)]
    public bool Repeated
    {
        get => _repeated;
        set
        {
            _repeated = value;

            // set this flag for child bands as well
            BandBase? child = Child;
            while (child != null)
            {
                child.Repeated = value;
                child = child.Child;
            }
        }
    }

    /// <summary>
    /// Gets or sets a script event name that will be fired before the band layouts its child objects.
    /// </summary>
    [Category ("Build")]
    public string BeforeLayoutEvent { get; set; }

    /// <summary>
    /// Gets or sets a script event name that will be fired after the child objects layout was finished.
    /// </summary>
    [Category ("Build")]
    public string AfterLayoutEvent { get; set; }

    /// <inheritdoc/>
    public override float AbsLeft => IsRunning ? base.AbsLeft : Left;

    /// <inheritdoc/>
    public override float AbsTop => IsRunning ? base.AbsTop : Top;

    /// <summary>
    /// Gets or sets collection of guide lines for this band.
    /// </summary>
    [Browsable (false)]
    public FloatCollection Guides { get; set; }

    /// <summary>
    /// Gets a row number (the same value returned by the "Row#" system variable).
    /// </summary>
    /// <remarks>
    /// This property can be used when running a report. It may be useful to print hierarchical
    /// row numbers in a master-detail report, like this:
    /// <para/>1.1
    /// <para/>1.2
    /// <para/>2.1
    /// <para/>2.2
    /// <para/>To do this, put the Text object on a detail data band with the following text in it:
    /// <para/>[Data1.RowNo].[Data2.RowNo]
    /// </remarks>
    [Browsable (false)]
    public int RowNo
    {
        get => _rowNumber;
        set
        {
            _rowNumber = value;
            if (Child != null)
            {
                Child.RowNo = value;
            }
        }
    }

    /// <summary>
    /// Gets an absolute row number (the same value returned by the "AbsRow#" system variable).
    /// </summary>
    [Browsable (false)]
    public int AbsRowNo
    {
        get => _absRowNo;
        set
        {
            _absRowNo = value;
            if (Child != null)
            {
                Child.AbsRowNo = value;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating that this is the first data row.
    /// </summary>
    [Browsable (false)]
    public bool IsFirstRow { get; set; }

    /// <summary>
    /// Gets a value indicating that this is the last data row.
    /// </summary>
    [Browsable (false)]
    public bool IsLastRow { get; set; }

    internal bool HasBorder => !Border.Equals (new Border());

    internal bool HasFill => !Fill.IsTransparent;

    internal DataBand? ParentDataBand
    {
        get
        {
            var c = Parent;
            while (c != null)
            {
                if (c is DataBand band)
                {
                    return band;
                }

                if (c is ReportPage { Subreport: { } } page)
                {
                    c = page.Subreport;
                }

                c = c.Parent;
            }

            return null;
        }
    }

    internal bool FlagUseStartNewPage { get; set; }

    internal bool FlagCheckFreeSpace
    {
        get => _flagCheckFreeSpace;
        set
        {
            _flagCheckFreeSpace = value;

            // set flag for child bands as well
            BandBase? child = Child;
            while (child != null)
            {
                child.FlagCheckFreeSpace = value;
                child = child.Child;
            }
        }
    }

    internal bool FlagMustBreak { get; set; }

    internal float ReprintOffset { get; set; }

    internal float PageWidth
    {
        get
        {
            if (Page is ReportPage page)
            {
                return page.WidthInPixels - (page.LeftMargin + page.RightMargin) * Units.Millimeters;
            }

            return 0;
        }
    }

    #endregion

    #region IParent Members

    /// <inheritdoc/>
    public virtual void GetChildObjects (ObjectCollection list)
    {
        foreach (ReportComponentBase obj in Objects)
        {
            list.Add (obj);
        }

        if (!IsRunning)
        {
            list.Add (_child!);
        }
    }

    /// <inheritdoc/>
    public virtual bool CanContain (Base child)
    {
        if (IsRunning)
        {
            return child is ReportComponentBase;
        }

        return child is ReportComponentBase and not BandBase or ChildBand;
    }

    /// <inheritdoc/>
    public virtual void AddChild (Base child)
    {
        if (child is ChildBand band && !IsRunning)
        {
            Child = band;
        }
        else
        {
            Objects.Add ((child as ReportComponentBase)!);
        }
    }

    /// <inheritdoc/>
    public virtual void RemoveChild (Base child)
    {
        if (child is ChildBand band && this._child == band)
        {
            Child = null;
        }
        else
        {
            Objects.Remove ((child as ReportComponentBase)!);
        }
    }

    /// <inheritdoc/>
    public virtual int GetChildOrder (Base child)
    {
        return Objects.IndexOf ((child as ReportComponentBase)!);
    }

    /// <inheritdoc/>
    public virtual void SetChildOrder (Base child, int order)
    {
        var oldOrder = child.ZOrder;
        if (oldOrder != -1 && order != -1 && oldOrder != order)
        {
            if (order > Objects.Count)
            {
                order = Objects.Count;
            }

            if (oldOrder <= order)
            {
                order--;
            }

            Objects.Remove ((child as ReportComponentBase)!);
            Objects.Insert (order, (child as ReportComponentBase)!);
            UpdateLayout (0, 0);
        }
    }

    /// <inheritdoc/>
    public virtual void UpdateLayout (float dx, float dy)
    {
        if (_updatingLayout)
        {
            return;
        }

        _updatingLayout = true;
        try
        {
            var remainingBounds = new RectangleF (0, 0, Width, Height);
            remainingBounds.Width += dx;
            remainingBounds.Height += dy;
            foreach (ReportComponentBase c in Objects)
            {
                if ((c.Anchor & AnchorStyles.Right) != 0)
                {
                    if ((c.Anchor & AnchorStyles.Left) != 0)
                    {
                        c.Width += dx;
                    }
                    else
                    {
                        c.Left += dx;
                    }
                }
                else if ((c.Anchor & AnchorStyles.Left) == 0)
                {
                    c.Left += dx / 2;
                }

                if ((c.Anchor & AnchorStyles.Bottom) != 0)
                {
                    if ((c.Anchor & AnchorStyles.Top) != 0)
                    {
                        c.Height += dy;
                    }
                    else
                    {
                        c.Top += dy;
                    }
                }
                else if ((c.Anchor & AnchorStyles.Top) == 0)
                {
                    c.Top += dy / 2;
                }

                switch (c.Dock)
                {
                    case DockStyle.Left:
                        c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Top, c.Width,
                            remainingBounds.Height);
                        remainingBounds.X += c.Width;
                        remainingBounds.Width -= c.Width;
                        break;

                    case DockStyle.Top:
                        c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Top, remainingBounds.Width,
                            c.Height);
                        remainingBounds.Y += c.Height;
                        remainingBounds.Height -= c.Height;
                        break;

                    case DockStyle.Right:
                        c.Bounds = new RectangleF (remainingBounds.Right - c.Width, remainingBounds.Top, c.Width,
                            remainingBounds.Height);
                        remainingBounds.Width -= c.Width;
                        break;

                    case DockStyle.Bottom:
                        c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Bottom - c.Height,
                            remainingBounds.Width, c.Height);
                        remainingBounds.Height -= c.Height;
                        break;

                    case DockStyle.Fill:
                        c.Bounds = remainingBounds;
                        remainingBounds.Width = 0;
                        remainingBounds.Height = 0;
                        break;
                }
            }
        }
        finally
        {
            _updatingLayout = false;
        }
    }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override void Assign (Base source)
    {
        base.Assign (source);

        var src = source as BandBase;
        Guides.Assign (src!.Guides);
        StartNewPage = src.StartNewPage;
        FirstRowStartsNewPage = src.FirstRowStartsNewPage;
        PrintOnBottom = src.PrintOnBottom;
        KeepChild = src.KeepChild;
        OutlineExpression = src.OutlineExpression;
        BeforeLayoutEvent = src.BeforeLayoutEvent;
        AfterLayoutEvent = src.AfterLayoutEvent;
        RepeatBandNTimes = src.RepeatBandNTimes;
    }

    internal virtual void UpdateWidth()
    {
        // update band width. It is needed for anchor/dock
        if (Page is ReportPage page && !(page.UnlimitedWidth && IsDesigning))
        {
            if (page.Columns.Count <= 1 || !IsColumnDependentBand)
            {
                Width = PageWidth;
            }
        }
    }

    internal void FixHeight()
    {
        var maxHeight = Height;
        foreach (ReportComponentBase c in Objects)
        {
            if (c.Bottom > maxHeight)
            {
                maxHeight = c.Bottom;
            }
        }

        if (maxHeight < 0)
        {
            maxHeight = 0;
        }

        Height = maxHeight;

        Validator.ValidateIntersectionAllObjects (this);
    }

    internal void FixHeightWithComponentsShift (float deltaY)
    {
        var minTop = Height;
        float maxBottom = 0;

        // Calculate minimum top of all components on this band.
        foreach (ReportComponentBase component in Objects)
        {
            if (component.Top < minTop)
            {
                minTop = component.Top;
            }
        }

        // Calculate maximum bottom of all components on this band.
        foreach (ReportComponentBase component in Objects)
        {
            if (component.Bottom > maxBottom)
            {
                maxBottom = component.Bottom;
            }
        }

        // Calculate minimum height of band with components shift.
        var minHeight = maxBottom - minTop;

        // Minimum height with compenents shift can't be negative.
        if (minHeight < 0)
        {
            minHeight = 0;
        }

        // Prevent incorrect movement of objects when mouse moves too fast.
        if (minTop < deltaY)
        {
            deltaY = minTop;
        }

        // Size of band should be decreased.
        if (deltaY > 0)
        {
            // There is enough place to move components up.
            if (minTop > 0)
            {
                // Move all components up.
                foreach (ReportComponentBase component in Objects)
                {
                    component.Top -= deltaY;
                }
            }
        }
        else
        {
            // Move all components down.
            foreach (ReportComponentBase component in Objects)
            {
                component.Top -= deltaY;
            }
        }

        // Height can't be less then minHeight.
        if (Height < minHeight)
        {
            Height = minHeight;
        }
    }

    /// <inheritdoc/>
    public override List<ValidationError> Validate()
    {
        return new List<ValidationError>();
    }

    /// <inheritdoc/>
    public override void Serialize (FRWriter writer)
    {
        var c = (writer.DiffObject as BandBase)!;
        base.Serialize (writer);

        if (writer.SerializeTo == SerializeTo.Preview)
        {
            return;
        }

        if (StartNewPage != c.StartNewPage)
        {
            writer.WriteBool ("StartNewPage", StartNewPage);
        }

        if (FirstRowStartsNewPage != c.FirstRowStartsNewPage)
        {
            writer.WriteBool ("FirstRowStartsNewPage", FirstRowStartsNewPage);
        }

        if (PrintOnBottom != c.PrintOnBottom)
        {
            writer.WriteBool ("PrintOnBottom", PrintOnBottom);
        }

        if (KeepChild != c.KeepChild)
        {
            writer.WriteBool ("KeepChild", KeepChild);
        }

        if (OutlineExpression != c.OutlineExpression)
        {
            writer.WriteStr ("OutlineExpression", OutlineExpression);
        }

        if (Guides.Count > 0)
        {
            writer.WriteValue ("Guides", Guides);
        }

        if (BeforeLayoutEvent != c.BeforeLayoutEvent)
        {
            writer.WriteStr ("BeforeLayoutEvent", BeforeLayoutEvent);
        }

        if (AfterLayoutEvent != c.AfterLayoutEvent)
        {
            writer.WriteStr ("AfterLayoutEvent", AfterLayoutEvent);
        }

        if (RepeatBandNTimes != c.RepeatBandNTimes)
        {
            writer.WriteInt ("RepeatBandNTimes", RepeatBandNTimes);
        }
    }

    internal bool IsColumnDependentBand
    {
        get
        {
            var b = this;
            if (b is ChildBand)
            {
                while (b is ChildBand)
                {
                    b = b.Parent as BandBase;
                }
            }

            if (b is DataHeaderBand or Reporting.DataBand or DataFooterBand or GroupHeaderBand or GroupFooterBand or ColumnHeaderBand or ColumnFooterBand or ReportSummaryBand)
            {
                return true;
            }

            return false;
        }
    }

    #endregion

    #region Report Engine

    internal void SetUpdatingLayout (bool value)
    {
        _updatingLayout = value;
    }

    /// <inheritdoc/>
    public override string[] GetExpressions()
    {
        List<string> expressions = new List<string>();
        expressions.AddRange (base.GetExpressions());

        if (!string.IsNullOrEmpty (OutlineExpression))
        {
            expressions.Add (OutlineExpression);
        }

        return expressions.ToArray();
    }

    /// <inheritdoc/>
    public override void SaveState()
    {
        base.SaveState();
        _savedOriginalObjectsCount = Objects.Count;
        SetRunning (true);
        SetDesigning (false);
        OnBeforePrint (EventArgs.Empty);

        foreach (ReportComponentBase obj in Objects)
        {
            obj.SaveState();
            obj.SetRunning (true);
            obj.SetDesigning (false);
            obj.OnBeforePrint (EventArgs.Empty);
        }

        //Report.Engine.TranslatedObjectsToBand(this);

        // apply even style
        if (RowNo % 2 == 0)
        {
            ApplyEvenStyle();

            foreach (ReportComponentBase obj in Objects)
            {
                obj.ApplyEvenStyle();
            }
        }
    }

    /// <inheritdoc/>
    public override void RestoreState()
    {
        OnAfterPrint (EventArgs.Empty);
        base.RestoreState();
        while (Objects.Count > _savedOriginalObjectsCount)
        {
            Objects[^1].Dispose();
        }

        SetRunning (false);

        var collection_clone = new ReportComponentCollection();
        Objects.CopyTo (collection_clone);
        foreach (ReportComponentBase obj in collection_clone)
        {
            obj.OnAfterPrint (EventArgs.Empty);
            obj.RestoreState();
            obj.SetRunning (false);
        }
    }

    /// <inheritdoc/>
    public override float CalcHeight()
    {
        OnBeforeLayout (EventArgs.Empty);

        // sort objects by Top
        var sortedObjects = Objects.SortByTop();

        // calc height of each object
        var heights = new float[sortedObjects.Count];
        for (var i = 0; i < sortedObjects.Count; i++)
        {
            var obj = sortedObjects[i];
            var height = obj.Height;
            if (obj.Visible && (obj.CanGrow || obj.CanShrink))
            {
                var height1 = obj.CalcHeight();
                if ((obj.CanGrow && height1 > height) || (obj.CanShrink && height1 < height))
                {
                    height = height1;
                }
            }

            heights[i] = height;
        }

        // calc shift amounts
        var shifts = new float[sortedObjects.Count];
        for (var i = 0; i < sortedObjects.Count; i++)
        {
            var parent = sortedObjects[i];
            var shift = heights[i] - parent.Height;
            if (shift == 0)
            {
                continue;
            }

            for (var j = i + 1; j < sortedObjects.Count; j++)
            {
                var child = sortedObjects[j];
                if (child.ShiftMode == ShiftMode.Never)
                {
                    continue;
                }

                if (child.Top >= parent.Bottom - 1e-4)
                {
                    if (child.ShiftMode == ShiftMode.WhenOverlapped &&
                        (child.Left > parent.Right - 1e-4 || parent.Left > child.Right - 1e-4))
                    {
                        continue;
                    }

                    var parentShift = shifts[i];
                    var childShift = shifts[j];
                    childShift = shift > 0
                        ? Math.Max (shift + parentShift, childShift)
                        : Math.Min (shift + parentShift, childShift);

                    shifts[j] = childShift;
                }
            }
        }

        // update location and size of each component, calc max height
        float maxHeight = 0;
        for (var i = 0; i < sortedObjects.Count; i++)
        {
            var obj = sortedObjects[i];
            var saveDock = obj.Dock;
            obj.Dock = DockStyle.None;
            obj.Height = heights[i];
            obj.Top += shifts[i];
            if (obj.Visible && obj.Bottom > maxHeight)
            {
                maxHeight = obj.Bottom;
            }

            obj.Dock = saveDock;
        }

        if ((CanGrow && maxHeight > Height) || (CanShrink && maxHeight < Height))
        {
            Height = maxHeight;
        }

        // perform grow to bottom
        foreach (ReportComponentBase obj in Objects)
        {
            if (obj.GrowToBottom)
            {
                obj.Height = Height - obj.Top;
            }
        }

        OnAfterLayout (EventArgs.Empty);
        return Height;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="breakTo"></param>
    public void AddLastToFooter (BreakableComponent breakTo)
    {
        var maxTop = ((AllObjects[0] as ComponentBase)!).Top;
        foreach (ComponentBase obj in AllObjects)
        {
            if (obj.Top > maxTop && obj is not DataFooterBand)
            {
                maxTop = obj.Top;
            }
        }

        var breakLine = maxTop;

        List<ReportComponentBase> pasteList = new List<ReportComponentBase>();
        foreach (ReportComponentBase obj in Objects)
        {
            if (obj.Bottom > breakLine)
            {
                pasteList.Add (obj);
            }
        }


        var itemsBefore = breakTo.AllObjects.Count;


        foreach (var obj in pasteList)
        {
            if (obj.Top < breakLine)
            {
                var breakComp = (Activator.CreateInstance (obj.GetType()) as BreakableComponent)!;
                breakComp.AssignAll (obj);
                breakComp.Parent = breakTo;

                breakComp.CanGrow = true;
                breakComp.CanShrink = false;
                breakComp.Height -= breakLine - obj.Top;
                breakComp.Top = 0;
                obj.Height = breakLine - obj.Top;
                (obj as BreakableComponent)!.Break (breakComp);
            }
            else
            {
                obj.Top -= breakLine;
                obj.Parent = breakTo;
            }
        }


        var minTop = (breakTo.AllObjects[0] as ComponentBase)!.Top;
        float maxBottom = 0;

        for (var i = itemsBefore; i < breakTo.AllObjects.Count; i++)
        {
            if ((breakTo.AllObjects[i] as ComponentBase)!.Top < minTop &&
                breakTo.AllObjects[i] is ReportComponentBase && breakTo.AllObjects[i] is not Table.TableCell)
            {
                minTop = (breakTo.AllObjects[i] as ComponentBase)!.Top;
            }
        }

        for (var i = itemsBefore; i < breakTo.AllObjects.Count; i++)
        {
            if ((breakTo.AllObjects[i] as ComponentBase)!.Bottom > maxBottom &&
                breakTo.AllObjects[i] is ReportComponentBase && breakTo.AllObjects[i] is not Table.TableCell)
            {
                maxBottom = (breakTo.AllObjects[i] as ComponentBase)!.Bottom;
            }
        }

        for (var i = 0; i < itemsBefore; i++)
        {
            (breakTo.AllObjects[i] as ComponentBase)!.Top += maxBottom - minTop;
        }

        breakTo.Height += maxBottom - minTop;

        Height -= maxBottom - minTop;
    }

    /// <inheritdoc/>
    public override bool Break (BreakableComponent? breakTo)
    {
        // first we find the break line. It's a minimum Top coordinate of the object that cannot break.
        var breakLine = Height;
        bool breakLineFound;
        do
        {
            breakLineFound = true;
            foreach (ReportComponentBase obj in Objects)
            {
                var canBreak = true;
                if (obj.Top < breakLine && obj.Bottom > breakLine)
                {
                    canBreak = false;
                    if (obj is BreakableComponent { CanBreak: true } breakable)
                    {
                        using var clone = (Activator.CreateInstance (breakable.GetType()) as BreakableComponent)!;
                        clone.AssignAll (breakable);
                        clone.Height = breakLine - clone.Top;

                        // to allow access to the Report
                        clone.Parent = breakTo;
                        canBreak = clone.Break (null);
                    }
                }

                if (!canBreak)
                {
                    breakLine = Math.Min (obj.Top, breakLine);

                    // enumerate objects again
                    breakLineFound = false;
                    break;
                }
            }
        } while (!breakLineFound);

        // now break the components
        var i = 0;
        while (i < Objects.Count)
        {
            var obj = Objects[i];
            if (obj.Bottom > breakLine)
            {
                if (obj.Top < breakLine)
                {
                    var breakComp = (Activator.CreateInstance (obj.GetType()) as BreakableComponent)!;
                    breakComp.AssignAll (obj);
                    breakComp.Parent = breakTo;

                    breakComp.CanGrow = true;
                    breakComp.CanShrink = false;
                    breakComp.Height -= breakLine - obj.Top;
                    breakComp.Top = 0;
                    obj.Height = breakLine - obj.Top;
                    (obj as BreakableComponent)!.Break (breakComp);
                }
                else
                {
                    // (case: object with Anchor = bottom on a breakable band)
                    // in case of bottom anchor, do not move the object. It will be moved automatically when we decrease the band height
                    if ((obj.Anchor & AnchorStyles.Bottom) == 0)
                    {
                        obj.Top -= breakLine;
                    }

                    obj.Parent = breakTo;
                    continue;
                }
            }

            i++;
        }

        Height = breakLine;
        breakTo!.Height -= breakLine;
        return Objects.Count > 0;
    }

    /// <inheritdoc/>
    public override void GetData()
    {
        base.GetData();

        var list = new FRCollectionBase();
        Objects.CopyTo (list);
        foreach (ReportComponentBase obj in list)
        {
            obj.GetData();
            obj.OnAfterData();

            // break the component if it is of BreakableComponent an has non-empty BreakTo property
            if (obj is BreakableComponent { BreakTo: { } } component &&
                component.BreakTo.GetType() == component.GetType())
            {
                component.Break (component.BreakTo);
            }
        }

        OnAfterData();
    }

    internal virtual bool IsEmpty()
    {
        return true;
    }

    private void AddBookmark (ReportComponentBase obj)
    {
        if (Report != null)
        {
            Report.Engine.AddBookmark (obj.Bookmark);
        }
    }

    internal void AddBookmarks()
    {
        AddBookmark (this);
        foreach (ReportComponentBase obj in Objects)
        {
            AddBookmark (obj);
        }
    }

    /// <inheritdoc/>
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        AbsRowNo = 0;
    }

    /// <summary>
    /// This method fires the <b>BeforeLayout</b> event and the script code connected to the <b>BeforeLayoutEvent</b>.
    /// </summary>
    /// <param name="e">Event data.</param>
    public void OnBeforeLayout (EventArgs e)
    {
        if (BeforeLayout != null)
        {
            BeforeLayout (this, e);
        }

        InvokeEvent (BeforeLayoutEvent, e);
    }

    /// <summary>
    /// This method fires the <b>AfterLayout</b> event and the script code connected to the <b>AfterLayoutEvent</b>.
    /// </summary>
    /// <param name="e">Event data.</param>
    public void OnAfterLayout (EventArgs e)
    {
        if (AfterLayout != null)
        {
            AfterLayout (this, e);
        }

        InvokeEvent (AfterLayoutEvent, e);
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="BandBase"/> class with default settings.
    /// </summary>
    protected BandBase()
    {
        Objects = new ReportComponentCollection (this);
        Guides = new FloatCollection();
        BeforeLayoutEvent = "";
        AfterLayoutEvent = "";
        OutlineExpression = "";
        CanBreak = false;
        ShiftMode = ShiftMode.Never;
        if (BaseName.EndsWith ("Band"))
        {
            BaseName = ClassName.Remove (ClassName.IndexOf ("Band", StringComparison.Ordinal));
        }

        SetFlags (Flags.CanMove | Flags.CanChangeOrder | Flags.CanChangeParent | Flags.CanCopy | Flags.CanGroup,
            false);
        FlagUseStartNewPage = true;
        FlagCheckFreeSpace = true;
    }
}
