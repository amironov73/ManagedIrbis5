// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchParametersTest
    {
        private void _TestSerialization
            (
                SearchParameters first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes.RestoreObjectFromMemory<SearchParameters>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Database, second!.Database);
            Assert.AreEqual(first.FirstRecord, second.FirstRecord);
            Assert.AreEqual(first.Format, second.Format);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            Assert.AreEqual(first.MinMfn, second.MinMfn);
            Assert.AreEqual(first.NumberOfRecords, second.NumberOfRecords);
            Assert.AreEqual(first.Expression, second.Expression);
            Assert.AreEqual(first.Sequential, second.Sequential);
        }

        [TestMethod]
        public void TestSearchParameters_Serialization()
        {
            var parameters = new SearchParameters();
            _TestSerialization(parameters);

            parameters = new SearchParameters
            {
                Database = "IBIS",
                Expression = "T=A$",
                Format = "@brief"
            };
            _TestSerialization(parameters);
        }

        [TestMethod]
        public void TestSearchParameters_Clone()
        {
            var expected = new SearchParameters
            {
                Database = "IBIS",
                Expression = "T=A$",
                Format = "@brief"
            };
            var actual = expected.Clone();

            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.FirstRecord, actual.FirstRecord);
            Assert.AreEqual(expected.Format, actual.Format);
            Assert.AreEqual(expected.MaxMfn, actual.MaxMfn);
            Assert.AreEqual(expected.MinMfn, actual.MinMfn);
            Assert.AreEqual(expected.NumberOfRecords, actual.NumberOfRecords);
            Assert.AreEqual(expected.Expression, actual.Expression);
            Assert.AreEqual(expected.Sequential, actual.Sequential);
        }

        [TestMethod]
        public void TestSearchParameters_Verify()
        {
            var parameters = new SearchParameters
            {
                Database = "IBIS",
                Expression = "T=A$",
                Format = "@brief"
            };
            Assert.IsTrue(parameters.Verify(false));
        }
    }
}
