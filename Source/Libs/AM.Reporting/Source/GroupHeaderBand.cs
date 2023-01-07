// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using AM.Reporting.Utils;
using AM.Reporting.Data;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Specifies a sort order.
/// </summary>
/// <remarks>
/// This enumeration is used in the group header and in the "Matrix" object.
/// </remarks>
public enum SortOrder
{
    /// <summary>
    /// Specifies no sort (natural order).
    /// </summary>
    None,

    /// <summary>
    /// Specifies an ascending sort order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Specifies a descending sort order.
    /// </summary>
    Descending
}

/// <summary>
/// Represents a group header band.
/// </summary>
/// <remarks>
/// A simple group consists of one <b>GroupHeaderBand</b> and the <b>DataBand</b> that is set
/// to the <see cref="Data"/> property. To create the nested groups, use the <see cref="NestedGroup"/> property.
/// <note type="caution">
/// Only the last nested group can have data band.
/// </note>
/// <para/>Use the <see cref="Condition"/> property to set the group condition. The <see cref="SortOrder"/>
/// property can be used to set the sort order for group's data rows. You can also use the <b>Sort</b>
/// property of the group's <b>DataBand</b> to specify additional sort.
/// </remarks>
/// <example>This example shows how to create nested groups.
/// <code>
/// ReportPage page = report.Pages[0] as ReportPage;
///
/// // create the main group
/// GroupHeaderBand mainGroup = new GroupHeaderBand();
/// mainGroup.Height = Units.Millimeters * 10;
/// mainGroup.Name = "MainGroup";
/// mainGroup.Condition = "[Orders.CustomerName]";
/// // add a group to the page
/// page.Bands.Add(mainGroup);
///
/// // create the nested group
/// GroupHeaderBand nestedGroup = new GroupHeaderBand();
/// nestedGroup.Height = Units.Millimeters * 10;
/// nestedGroup.Name = "NestedGroup";
/// nestedGroup.Condition = "[Orders.OrderDate]";
/// // add it to the main group
/// mainGroup.NestedGroup = nestedGroup;
///
/// // create a data band
/// DataBand dataBand = new DataBand();
/// dataBand.Height = Units.Millimeters * 10;
/// dataBand.Name = "GroupData";
/// dataBand.DataSource = report.GetDataSource("Orders");
/// // connect the databand to the nested group
/// nestedGroup.Data = dataBand;
/// </code>
/// </example>
public partial class GroupHeaderBand
    : HeaderFooterBandBase
{
    #region Fields

    private GroupHeaderBand? _nestedGroup;
    private DataBand _dataBand;
    private GroupFooterBand _groupFooter;
    private DataHeaderBand? _header;
    private DataFooterBand? _footer;
    private object? _groupValue;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a nested group.
    /// </summary>
    /// <remarks>
    /// Use this property to create nested groups.
    /// <note type="caution">
    /// Only the last nested group can have data band.
    /// </note>
    /// </remarks>
    /// <example>
    /// This example demonstrates how to create a group with nested group.
    /// <code>
    /// ReportPage page;
    /// GroupHeaderBand group = new GroupHeaderBand();
    /// group.NestedGroup = new GroupHeaderBand();
    /// group.NestedGroup.Data = new DataBand();
    /// page.Bands.Add(group);
    /// </code>
    /// </example>
    [Browsable (false)]
    public GroupHeaderBand? NestedGroup
    {
        get => _nestedGroup;
        set
        {
            SetProp (_nestedGroup, value);
            _nestedGroup = value;
        }
    }

    /// <summary>
    /// Gets or sets the group data band.
    /// </summary>
    /// <remarks>
    /// Use this property to add a data band to a group. Note: only the last nested group can have Data band.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to add a data band to a group.
    /// <code>
    /// ReportPage page;
    /// GroupHeaderBand group = new GroupHeaderBand();
    /// group.Data = new DataBand();
    /// page.Bands.Add(group);
    /// </code>
    /// </example>
    [Browsable (false)]
    public DataBand Data
    {
        get => _dataBand;
        set
        {
            SetProp (_dataBand, value);
            _dataBand = value;
        }
    }

    /// <summary>
    /// Gets or sets a group footer.
    /// </summary>
    [Browsable (false)]
    public GroupFooterBand GroupFooter
    {
        get => _groupFooter;
        set
        {
            SetProp (_groupFooter, value);
            _groupFooter = value;
        }
    }

    /// <summary>
    /// Gets or sets a header band.
    /// </summary>
    [Browsable (false)]
    public DataHeaderBand? Header
    {
        get => _header;
        set
        {
            SetProp (_header, value);
            _header = value;
        }
    }

    /// <summary>
    /// Gets or sets a footer band.
    /// </summary>
    /// <remarks>
    /// To access a group footer band, use the <see cref="GroupFooter"/> property.
    /// </remarks>
    [Browsable (false)]
    public DataFooterBand? Footer
    {
        get => _footer;
        set
        {
            SetProp (_footer, value);
            _footer = value;
        }
    }

    /// <summary>
    /// Gets or sets the group condition.
    /// </summary>
    /// <remarks>
    /// This property can contain any valid expression. When running a report, this expression is calculated
    /// for each data row. When the value of this condition is changed, AM.Reporting starts a new group.
    /// </remarks>
    [Category ("Data")]
    public string Condition { get; set; }

    /// <summary>
    /// Gets or sets the sort order.
    /// </summary>
    /// <remarks>
    /// AM.Reporting can sort data rows automatically using the <see cref="Condition"/> value.
    /// </remarks>
    [DefaultValue (SortOrder.Ascending)]
    [Category ("Behavior")]
    public SortOrder SortOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the group should be printed together on one page.
    /// </summary>
    [DefaultValue (false)]
    [Category ("Behavior")]
    public bool KeepTogether { get; set; }

    /// <summary>
    /// Gets or sets a value that determines whether to reset the page numbers when this group starts print.
    /// </summary>
    /// <remarks>
    /// Typically you should set the <see cref="BandBase.StartNewPage"/> property to <b>true</b> as well.
    /// </remarks>
    [DefaultValue (false)]
    [Category ("Behavior")]
    public bool ResetPageNumber { get; set; }

    internal DataSourceBase DataSource
    {
        get
        {
            var dataBand = GroupDataBand;
            return dataBand == null ? null : dataBand.DataSource;
        }
    }

    internal DataBand? GroupDataBand
    {
        get
        {
            var group = this;
            while (group != null)
            {
                if (group.Data != null)
                {
                    return group.Data;
                }

                group = group.NestedGroup;
            }

            return null;
        }
    }

    #endregion

    #region IParent

    /// <inheritdoc/>
    public override void GetChildObjects (ObjectCollection list)
    {
        base.GetChildObjects (list);
        if (!IsRunning)
        {
            list.Add (_header);
            list.Add (_nestedGroup);
            list.Add (_dataBand);
            list.Add (_groupFooter);
            list.Add (_footer);
        }
    }

    /// <inheritdoc/>
    public override bool CanContain (Base child)
    {
        return base.CanContain (child) ||
               (child is DataBand && _nestedGroup == null && _dataBand == null) ||
               (child is GroupHeaderBand && _nestedGroup is null or GroupHeaderBand &&
                _dataBand == null) ||
               child is GroupFooterBand or DataHeaderBand or DataFooterBand;
    }

    /// <inheritdoc/>
    public override void AddChild (Base child)
    {
        if (IsRunning)
        {
            base.AddChild (child);
            return;
        }

        if (child is GroupHeaderBand band)
        {
            NestedGroup = band;
        }
        else if (child is DataBand dataBand)
        {
            Data = dataBand;
        }
        else if (child is GroupFooterBand footerBand)
        {
            GroupFooter = footerBand;
        }
        else if (child is DataHeaderBand headerBand)
        {
            Header = headerBand;
        }
        else if (child is DataFooterBand dataFooterBand)
        {
            Footer = dataFooterBand;
        }
        else
        {
            base.AddChild (child);
        }
    }

    /// <inheritdoc/>
    public override void RemoveChild (Base child)
    {
        base.RemoveChild (child);
        if (IsRunning)
        {
            return;
        }

        if (child is GroupHeaderBand && _nestedGroup == child)
        {
            NestedGroup = null;
        }

        if (child is DataBand band && _dataBand == band)
        {
            Data = null;
        }

        if (child is GroupFooterBand && _groupFooter == child)
        {
            GroupFooter = null;
        }

        if (child is DataHeaderBand && _header == child)
        {
            Header = null;
        }

        if (child is DataFooterBand && _footer == child)
        {
            Footer = null;
        }
    }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override void Assign (Base source)
    {
        base.Assign (source);

        var src = source as GroupHeaderBand;
        Condition = src.Condition;
        SortOrder = src.SortOrder;
        KeepTogether = src.KeepTogether;
        ResetPageNumber = src.ResetPageNumber;
    }

    /// <inheritdoc/>
    public override void Serialize (ReportWriter writer)
    {
        var c = writer.DiffObject as GroupHeaderBand;
        base.Serialize (writer);
        if (writer.SerializeTo == SerializeTo.Preview)
        {
            return;
        }

        if (Condition != c.Condition)
        {
            writer.WriteStr ("Condition", Condition);
        }

        if (SortOrder != c.SortOrder)
        {
            writer.WriteValue ("SortOrder", SortOrder);
        }

        if (KeepTogether != c.KeepTogether)
        {
            writer.WriteBool ("KeepTogether", KeepTogether);
        }

        if (ResetPageNumber != c.ResetPageNumber)
        {
            writer.WriteBool ("ResetPageNumber", ResetPageNumber);
        }
    }

    /// <inheritdoc/>
    public override string[] GetExpressions()
    {
        return new string[] { Condition };
    }

    internal override bool IsEmpty()
    {
        if (NestedGroup != null)
        {
            return NestedGroup.IsEmpty();
        }
        else if (Data != null)
        {
            return Data.IsEmpty();
        }

        return base.IsEmpty();
    }

    internal void InitDataSource()
    {
        var dataBand = GroupDataBand;
        var group = this;
        var index = 0;

        // insert group sort to the databand
        while (group != null)
        {
            if (group.SortOrder != SortOrder.None)
            {
                dataBand.Sort.Insert (index, new Sort (group.Condition, group.SortOrder == SortOrder.Descending));
                index++;
            }

            group = group.NestedGroup;
        }

        dataBand.InitDataSource();
    }

    internal void FinalizeDataSource()
    {
        var dataBand = GroupDataBand;
        var group = this;

        // remove group sort from the databand
        while (group != null)
        {
            if (group.SortOrder != SortOrder.None)
            {
                dataBand.Sort.RemoveAt (0);
            }

            group = group.NestedGroup;
        }
    }

    internal void ResetGroupValue()
    {
        if (!string.IsNullOrEmpty (Condition))
        {
            _groupValue = Report.Calc (Condition);
        }
        else
        {
            throw new GroupHeaderHasNoGroupCondition (Name);
        }
    }

    internal bool GroupValueChanged()
    {
        object? value = null;
        if (!string.IsNullOrEmpty (Condition))
        {
            value = Report.Calc (Condition);
        }
        else
        {
            throw new GroupHeaderHasNoGroupCondition (Name);
        }

        if (_groupValue == null)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }

        return !_groupValue.Equals (value);
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupHeaderBand"/> class with default settings.
    /// </summary>
    public GroupHeaderBand()
    {
        Condition = "";
        SortOrder = SortOrder.Ascending;
    }
}
