// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.Reports;

#nullable enable

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class IrbisReportTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisReport_Constructor_1()
        {
            var report = new IrbisReport();
            Assert.IsNull(report.Header);
            Assert.IsNull(report.Footer);
            Assert.IsNotNull(report.Body);
            Assert.AreEqual(0, report.Body.Count);
        }

        private void _TestSaveJson
            (
                IrbisReport first
            )
        {
            var fileName = Path.GetTempFileName();
            first.SaveJson(fileName);

            var second = IrbisReport.LoadJsonFile(fileName);
            Assert.IsNotNull(second);
        }

        private void _TestSaveShortJson
            (
                IrbisReport first
            )
        {
            var fileName = Path.GetTempFileName();
            first.SaveShortJson(fileName);

            var second = IrbisReport.LoadShortJson(fileName);
            Assert.IsNotNull(second);
        }

        private List<Record> _GetRecords()
        {
            var result = new List<Record>();

            for (var i = 0; i < 10; i++)
            {
                var record = new Record();

                var field = record.Add(200);
                field.Add('a', "Record" + (i + 1));
                field = record.Add(10);
                field.Add('d', (i + 1).ToString("F2", CultureInfo.InvariantCulture));

                result.Add(record);
            }

            return result;
        }

        [TestMethod]
        public void IrbisReport_SaveJson_1()
        {
            var report = new IrbisReport();
            _TestSaveJson(report);

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveJson(report);
        }

        [TestMethod]
        public void IrbisReport_SaveJson_2()
        {
            var report = new IrbisReport();
            _TestSaveJson(report);

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var filterBand = new FilterBand
            {
                FilterExpression = "if v200^a:' ' then '1' else '0' fi"
            };
            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            detailsBand.Cells.Add(new PftCell("'This is a PFT'"));
            filterBand.Body.Add(detailsBand);
            report.Body.Add(filterBand);

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveJson(report);
        }

        [TestMethod]
        public void IrbisReport_SaveShortJson_1()
        {
            var report = new IrbisReport();
            _TestSaveJson(report);

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var filterBand = new FilterBand
            {
                FilterExpression = "if v200^a:' ' then '1' else '0' fi"
            };
            var detailsBand = new DetailsBand();
            ReportCell cell = new TextCell("This is a text");
            cell.SetWidth(100).SetHeight(10);
            detailsBand.Cells.Add(cell);
            cell = new PftCell("'This is a PFT'");
            cell.SetWidth(200).SetHeight(10);
            detailsBand.Cells.Add(cell);
            filterBand.Body.Add(detailsBand);
            report.Body.Add(filterBand);

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveShortJson(report);
        }

        private void _TestEvaluatePlainText
            (
                IrbisReport report
            )
        {
            var provider = new NullProvider();
            var context = new ReportContext(provider);
            context.Records.AddRange(_GetRecords());
            report.Render(context);
            var text = context.Output.Text;
            Assert.IsNotNull(text);
            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, text);
        }

        private void _TestEvaluateHtml
            (
                IrbisReport report
            )
        {
            var client = new NullProvider();
            var context = new ReportContext(client);
            context.Records.AddRange(_GetRecords());
            context.SetDriver(new HtmlDriver());
            report.Render(context);
            var text = context.Output.Text;
            Assert.IsNotNull(text);
            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, text);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_1()
        {
            var report = new IrbisReport();
            _TestEvaluatePlainText(report);

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestEvaluatePlainText(report);

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestEvaluatePlainText(report);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_2()
        {
            var report = new IrbisReport();
            _TestEvaluateHtml(report);

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestEvaluateHtml(report);

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestEvaluateHtml(report);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_3()
        {
            var report = new IrbisReport();

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;

            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new IndexCell());
            detailsBand.Cells.Add(new TextCell("::"));
            detailsBand.Cells.Add(new PftCell("v200^a"));
            report.Body.Add(detailsBand);

            _TestSaveJson(report);
            _TestEvaluateHtml(report);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_4()
        {
            var report = new IrbisReport();

            var headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            var footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;

            var compositeBand = new CompositeBand();

            var detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new IndexCell());
            detailsBand.Cells.Add(new TextCell("::"));
            detailsBand.Cells.Add(new PftCell("v200^a"));
            detailsBand.Cells.Add(new PftCell("v10^d"));
            compositeBand.Body.Add(detailsBand);

            var totalBand = new TotalBand();
            totalBand.Cells.Add(new TextCell());
            totalBand.Cells.Add(new TextCell());
            totalBand.Cells.Add(new TextCell("Sum"));
            totalBand.Cells.Add(new TotalCell(0,3,TotalFunction.Sum,
                "F2"));
            compositeBand.Footer = totalBand;

            report.Body.Add(compositeBand);

            _TestSaveJson(report);
            _TestEvaluatePlainText(report);
        }
    }
}
