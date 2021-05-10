// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM;
using AM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

#nullable enable

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class CellCollectionTest
        : Common.CommonUnitTest
    {
        private ReportCell[] _GetCells()
        {
            return new ReportCell[]
            {
                new TextCell("Cell1"),
                new TextCell("Cell2"),
                new TextCell("Cell3")
            };
        }

        [TestMethod]
        public void CellCollection_Construction_1()
        {
            var collection = new CellCollection();
            Assert.IsNull(collection.Band);
            Assert.IsNull(collection.Report);
            Assert.IsFalse(collection.ReadOnly);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Add_1()
        {
            var band = new ReportBand();
            ReportCell cell = new TextCell();
            band.Cells.Add(cell);
            Assert.AreSame(band, cell.Band);
        }

        [TestMethod]
        public void CellCollection_Add_2()
        {
            var report = new IrbisReport();
            var band = new ReportBand();
            report.Body.Add(band);
            ReportCell cell = new TextCell();
            band.Cells.Add(cell);
            Assert.AreSame(report, cell.Report);
        }

        [TestMethod]
        public void CellCollection_AddRange_1()
        {
            var collection = new CellCollection();
            var cells = _GetCells();
            collection.AddRange(cells);
            Assert.AreEqual(cells.Length, collection.Count);
            for (var i = 0; i < cells.Length; i++)
            {
                Assert.AreSame(cells[i], collection[i]);
            }
        }

        [TestMethod]
        public void CellCollection_AsReadOnly_1()
        {
            var first = new CellCollection();
            first.AddRange(_GetCells());
            var second = first.AsReadOnly();
            Assert.IsTrue(second.ReadOnly);
            Assert.AreEqual(first.Count, second.Count);
            for (var i = 0; i < first.Count; i++)
            {
                var cell1 = (TextCell) first[i];
                var cell2 = (TextCell) second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void CellCollection_AsReadOnly_2()
        {
            var first = new CellCollection();
            first.AddRange(_GetCells());
            var second = first.AsReadOnly();
            ReportCell cell = new TextCell();
            second.Add(cell);
        }

        [TestMethod]
        public void CellCollection_Clear_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Clone_1()
        {
            var first = new CellCollection();
            first.AddRange(_GetCells());
            var second = first.Clone();
            Assert.AreEqual(first.Count, second.Count);
            for (var i = 0; i < first.Count; i++)
            {
                var cell1 = (TextCell)first[i];
                var cell2 = (TextCell)second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        public void CellCollection_Dispose_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.Dispose();
        }

        [TestMethod]
        public void CellCollection_Find_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            var found = collection
                .Find(cell => ((TextCell)cell).Text == "Cell2");
            Assert.IsNotNull(found);
            Assert.AreEqual("Cell2", ((TextCell)found!).Text);

            found = collection
                .Find(cell => ((TextCell)cell).Text == "nosuchcell");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void CellCollection_FindAll_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            var found = collection
                .FindAll(cell => string.CompareOrdinal
                    (
                        ((TextCell)cell).Text,
                        "Cell2"
                    ) >= 0);
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("Cell2", ((TextCell)found[0]).Text);
            Assert.AreEqual("Cell3", ((TextCell)found[1]).Text);

            found = collection.FindAll(cell => string.CompareOrdinal
                (
                    ((TextCell)cell).Text,
                    "Cell9"
                ) >= 0);
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void CellCollection_Insert_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            ReportCell cell = new TextCell();
            collection.Insert(0, cell);
            Assert.AreSame(cell, collection[0]);
            Assert.AreEqual(4, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Remove_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            var cell = collection[1];
            collection.Remove(cell);
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void CellCollection_RemoveAt_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.RemoveAt(1);
            Assert.AreEqual(2, collection.Count);
        }

        private void _TestSerialization
            (
                CellCollection first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<CellCollection>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Count, second!.Count);
            for (var i = 0; i < first.Count; i++)
            {
                var cell1 = (TextCell) first[i];
                var cell2 = (TextCell) second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CellCollection_Serialization_1()
        {
            var collection = new CellCollection();
            _TestSerialization(collection);

            //collection.AddRange(_GetCells());
            //_TestSerialization(collection);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CellCollection_Serialization_2()
        {
            var collection = new CellCollection();
            var stream = new MemoryStream();
            var reader = new BinaryReader(stream);
            collection.RestoreFromStream(reader);
        }

        [TestMethod]
        public void CellCollection_SetItem_1()
        {
            var collection = new CellCollection();
            collection.AddRange(_GetCells());
            var cell = new TextCell();
            collection[2] = cell;
            Assert.AreSame(cell, collection[2]);
        }

        [TestMethod]
        public void CellCollection_Verify_1()
        {
            var collection = new CellCollection();
            Assert.IsTrue(collection.Verify(false));

            collection.AddRange(_GetCells());
            Assert.IsTrue(collection.Verify(false));
        }
    }
}
