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

using System;
using System.ComponentModel;
using System.IO;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// This class contains a hyperlink settings.
/// </summary>
[TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
public class Hyperlink
{
    #region Fields

    private string _saveValue;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the kind of hyperlink.
    /// </summary>
    /// <remarks>
    /// <para>Use the <b>Kind</b> property to define hyperlink's behavior.
    /// The hyperlink may be used to navigate to the external url, the page number,
    /// the bookmark defined by other report object, the external report, the other page of this report,
    /// and custom hyperlink.</para>
    /// </remarks>
    [DefaultValue (HyperlinkKind.URL)]
    public HyperlinkKind Kind { get; set; }

    /// <summary>
    /// Gets or sets the expression which value will be used for navigation.
    /// </summary>
    /// <remarks>
    /// <para>Normally you should set the <b>Expression</b> property to
    /// any valid expression that will be calculated when this object is about to print.
    /// The value of an expression will be used for navigation.</para>
    /// <para>If you want to navigate to some fixed data (URL or page number, for example),
    /// use the <see cref="Value"/> property instead.</para>
    /// </remarks>
    public string Expression { get; set; }

    /// <summary>
    /// Gets or sets a value that will be used for navigation.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the fixed data (such as URL, page number etc). If you want to
    /// navigate to some dynamically calculated value, use the <see cref="Expression"/> property instead.
    /// </remarks>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets a value that indicate should be links open in new tab or not.
    /// </summary>
    /// <remarks>
    /// It works for HTML-export only!
    /// </remarks>
    public bool OpenLinkInNewTab { get; set; }

    /// <summary>
    /// Gets or sets an external report file name.
    /// </summary>
    /// <remarks>
    /// <para>Use this property if <see cref="Kind"/> is set to <b>DetailReport</b>. </para>
    /// <para>When you follow the hyperlink, this report will be loaded and run.
    /// You also may specify the report's parameter in the <see cref="ReportParameter"/> property.</para>
    /// </remarks>
    public string DetailReportName { get; set; }

    /// <summary>
    /// Gets or sets the name of this report's page.
    /// </summary>
    /// <remarks>
    /// <para>Use this property if <see cref="Kind"/> is set to <b>DetailPage</b>. </para>
    /// <para>When you follow the hyperlink, the specified page will be executed. It may contain the
    /// detailed report. You also may specify the report's parameter in the
    /// <see cref="ReportParameter"/> property.</para>
    /// </remarks>
    public string DetailPageName { get; set; }

    /// <summary>
    /// Gets or sets a parameter's name that will be set to hyperlink's value.
    /// </summary>
    /// <remarks>
    /// Use this property if <see cref="Kind"/> is set to <b>DetailReport</b> or <b>DetailPage</b>.
    /// <para>If you want to pass the hyperlink's value to the report's parameter, specify the
    /// parameter name in this property. This parameter will be set to the hyperlink's value
    /// before running a report. It may be used to display detailed information about clicked item.</para>
    /// <para>It is also possible to pass multiple values to several parameters. If hyperlink's value
    /// contains separators (the separator string can be set in the <see cref="ValuesSeparator"/>
    /// property), it will be splitted to several values. That values will be passed to nested parameters
    /// of the <b>ReportParameter</b> (you should create nested parameters by youself). For example, you have
    /// the <b>ReportParameter</b> called "SelectedValue" which has two nested parameters: the first one is
    /// "Employee" and the second is "Category". The hyperlink's value is "Andrew Fuller;Beverages".
    /// It will be splitted to two values: "Andrew Fuller" and "Beverages". The first nested parameter
    /// of the <b>ReportParameter</b> that is "Employee" in our case will be set to "Andrew Fuller";
    /// the second nested parameter ("Category") will be set to "Beverages".</para>
    /// <para>Note: when you create a parameter in the detailed report, don't forget to set
    /// its <b>DataType</b> property. It is used to convert string values to actual data type.
    /// </para>
    /// </remarks>
    public string ReportParameter { get; set; }

    /// <summary>
    /// Gets or sets a string that will be used as a separator to pass several values
    /// to the external report parameters.
    /// </summary>
    public string ValuesSeparator { get; set; }

    internal ReportComponentBase Parent { get; private set; }

    internal Report Report => Parent.Report;

    #endregion

    #region Private Methods

    private bool ShouldSerializeValuesSeparator()
    {
        return ValuesSeparator != ";";
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Assigns values from another source.
    /// </summary>
    /// <param name="source">Source to assign from.</param>
    public void Assign (Hyperlink source)
    {
        Kind = source.Kind;
        Expression = source.Expression;
        Value = source.Value;
        DetailReportName = source.DetailReportName;
        ReportParameter = source.ReportParameter;
        DetailPageName = source.DetailPageName;
        OpenLinkInNewTab = source.OpenLinkInNewTab;
    }

    internal bool Equals (Hyperlink h)
    {
        return h != null && Kind == h.Kind && Expression == h.Expression &&
               DetailReportName == h.DetailReportName && ReportParameter == h.ReportParameter &&
               DetailPageName == h.DetailPageName;
    }

    internal void SetParent (ReportComponentBase parent)
    {
        this.Parent = parent;
    }

    internal void Calculate()
    {
        if (!string.IsNullOrEmpty (Expression))
        {
            var value = Report.Calc (Expression);
            Value = value == null ? "" : value.ToString();
        }
    }

    internal Report GetReport (bool updateParameter)
    {
        var report = Report.FromFile (DetailReportName);
        Report.Dictionary.ReRegisterData (report.Dictionary);

        if (updateParameter)
        {
            SetParameters (report);
        }

        return report;
    }

    internal void SetParameters (Report report)
    {
        if (!string.IsNullOrEmpty (ReportParameter))
        {
            var param = report.GetParameter (ReportParameter);
            if (param != null)
            {
                if (Value.IndexOf (ValuesSeparator) != -1)
                {
                    string[] values = Value.Split (new string[] { ValuesSeparator },
                        StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < values.Length; i++)
                    {
                        if (i < param.Parameters.Count)
                        {
                            param.Parameters[i].AsString = values[i];
                        }
                    }
                }
                else
                {
                    param.AsString = Value;
                }
            }
        }
    }

    internal void Serialize (ReportWriter writer, Hyperlink hyperlink)
    {
        if (Kind != hyperlink.Kind)
        {
            writer.WriteValue ("Hyperlink.Kind", Kind);
        }

        if (Expression != hyperlink.Expression)
        {
            writer.WriteStr ("Hyperlink.Expression", Expression);
        }

        if (Value != hyperlink.Value)
        {
            writer.WriteStr ("Hyperlink.Value", Value);
        }

        if (DetailReportName != hyperlink.DetailReportName)
        {
            // when saving to the report file, convert absolute path to the external report to relative path
            // (based on the main report path).
            var value = DetailReportName;
            if (writer.SerializeTo == SerializeTo.Report && Report != null &&
                !string.IsNullOrEmpty (Report.FileName))
            {
                value = FileUtils.GetRelativePath (DetailReportName, Path.GetDirectoryName (Report.FileName));
            }

            writer.WriteStr ("Hyperlink.DetailReportName", value);
        }

        if (DetailPageName != hyperlink.DetailPageName)
        {
            writer.WriteStr ("Hyperlink.DetailPageName", DetailPageName);
        }

        if (ReportParameter != hyperlink.ReportParameter)
        {
            writer.WriteStr ("Hyperlink.ReportParameter", ReportParameter);
        }

        if (ValuesSeparator != hyperlink.ValuesSeparator)
        {
            writer.WriteStr ("Hyperlink.ValuesSeparator", ValuesSeparator);
        }

        if (OpenLinkInNewTab != hyperlink.OpenLinkInNewTab)
        {
            writer.WriteBool ("Hyperlink.OpenLinkInNewTab", OpenLinkInNewTab);
        }
    }

    internal void OnAfterLoad()
    {
        // convert relative path to the external report to absolute path (based on the main report path).
        if (string.IsNullOrEmpty (DetailReportName) || string.IsNullOrEmpty (Report.FileName))
        {
            return;
        }

        if (!Path.IsPathRooted (DetailReportName))
        {
            DetailReportName = Path.GetDirectoryName (Report.FileName) + Path.DirectorySeparatorChar +
                               DetailReportName;
        }
    }

    internal void SaveState()
    {
        _saveValue = Value;
    }

    internal void RestoreState()
    {
        Value = _saveValue;
    }

    #endregion

    internal Hyperlink (ReportComponentBase parent)
    {
        SetParent (parent);
        Expression = "";
        Value = "";
        DetailReportName = "";
        DetailPageName = "";
        ReportParameter = "";
        ValuesSeparator = ";";
    }
}
