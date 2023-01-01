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
using System.Drawing;
using System.Drawing.Design;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a subreport object.
    /// </summary>
    /// <remarks>
    /// To create a subreport in code, you should create the report page first and
    /// connect it to the subreport using the <see cref="ReportPage"/> property.
    /// </remarks>
    /// <example>The following example shows how to create a subreport object in code.
    /// <code>
    /// // create the main report page
    /// ReportPage reportPage = new ReportPage();
    /// reportPage.Name = "Page1";
    /// report.Pages.Add(reportPage);
    /// // create report title band
    /// reportPage.ReportTitle = new ReportTitleBand();
    /// reportPage.ReportTitle.Name = "ReportTitle1";
    /// reportPage.ReportTitle.Height = Units.Millimeters * 10;
    /// // add subreport on it
    /// SubreportObject subreport = new SubreportObject();
    /// subreport.Name = "Subreport1";
    /// subreport.Bounds = new RectangleF(0, 0, Units.Millimeters * 25, Units.Millimeters * 5);
    /// reportPage.ReportTitle.Objects.Add(subreport);
    /// // create subreport page
    /// ReportPage subreportPage = new ReportPage();
    /// subreportPage.Name = "SubreportPage1";
    /// report.Pages.Add(subreportPage);
    /// // connect the subreport to the subreport page
    /// subreport.ReportPage = subreportPage;
    /// </code>
    /// </example>
    public partial class SubreportObject : ReportComponentBase
    {
        private ReportPage reportPage;

        #region Properties

        /// <summary>
        /// Gets or sets a report page that contains the subreport bands and objects.
        /// </summary>

        //[Browsable(false)]
        [Editor ("AM.Reporting.TypeEditors.SubreportPageEditor, AM.Reporting", typeof (UITypeEditor))]
        [TypeConverter (typeof (TypeConverters.ComponentRefConverter))]
        public ReportPage ReportPage
        {
            get => reportPage;
            set
            {
                if (value == Page)
                {
                    return;
                }

                if (reportPage != null && value != reportPage)
                {
                    RemoveSubReport (false);
                }

                if (value != null)
                {
                    value.Subreport = this;
                    value.PageName = Name;
                }

                reportPage = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that subreport must print its objects on a parent band to which it belongs.
        /// </summary>
        /// <remarks>
        /// Default behavior of the subreport is to print subreport objects they own separate bands.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool PrintOnParent { get; set; }

        #endregion

        private void RemoveSubReport (bool delete)
        {
            if (reportPage != null)
            {
                if (Report != null)
                {
                    foreach (Base obj in Report.AllObjects)
                    {
                        if (obj is SubreportObject subReport && obj != this)
                        {
                            if (subReport.ReportPage == reportPage)
                            {
                                reportPage.Subreport = subReport;
                                reportPage.PageName = subReport.Name;
                                reportPage = null;
                                break;
                            }
                        }
                    }
                }

                if (reportPage != null)
                {
                    if (delete && Report != null)
                    {
                        reportPage.Dispose();
                    }
                    else
                    {
                        reportPage.Subreport = null;
                        reportPage.PageName = reportPage.Name;
                    }

                    reportPage = null;
                }
            }
        }

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as SubreportObject;
            PrintOnParent = src.PrintOnParent;
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as SubreportObject;
            base.Serialize (writer);

            writer.WriteRef ("ReportPage", ReportPage);
            if (PrintOnParent != c.PrintOnParent)
            {
                writer.WriteBool ("PrintOnParent", PrintOnParent);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SubreportObject"/> class with default settings.
        /// </summary>
        public SubreportObject()
        {
            Fill = new SolidFill (SystemColors.Control);
            FlagUseBorder = false;
            FlagUseFill = false;
            FlagPreviewVisible = false;
            SetFlags (Flags.CanCopy, false);
        }
    }
}
