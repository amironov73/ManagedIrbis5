// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class FieldFilterTest
    {
        private FieldFilter _GetFilter()
        {
            var provider = new NullProvider();
            var result = new FieldFilter
                (
                    provider,
                    "v200^a = 'Второе'"
                );

            return result;
        }

        [TestMethod]
        public void FieldFilter_Constructor_1()
        {
            var filter = _GetFilter();

            Assert.IsNotNull(filter.Formatter);
            Assert.IsNotNull(filter.Formatter.Program);
        }

        private Record _GetRecord()
        {
            var result = new Record();
            result.Fields.Add(new Field(100, "^aПервое"));
            result.Fields.Add(new Field(100, "^aВторое"));
            result.Fields.Add(new Field(100, "^aТретье"));
            result.Fields.Add(new Field(200, "^aПервое"));
            result.Fields.Add(new Field(200, "^aВторое"));
            result.Fields.Add(new Field(200, "^aТретье"));
            result.Fields.Add(new Field(300, "^aПервое"));
            result.Fields.Add(new Field(300, "^aВторое"));
            result.Fields.Add(new Field(300, "^aТретье"));

            return result;
        }

        [TestMethod]
        public void FieldFilter_CheckField_1()
        {
            var filter = _GetFilter();

            var field1001 = new Field(100, "^aПервое");
            var field1002 = new Field(100, "^aВторое");
            var field2001 = new Field(200, "^aПервое");
            var field2002 = new Field(200, "^aВторое");

            Assert.IsFalse(filter.CheckField(field1001));
            Assert.IsFalse(filter.CheckField(field1002));
            Assert.IsFalse(filter.CheckField(field2001));
            Assert.IsTrue(filter.CheckField(field2002));
        }

        [TestMethod]
        public void FieldFilter_AllFields_1()
        {
            var filter = _GetFilter();
            var record = _GetRecord();

            Assert.IsFalse(filter.AllFields(record.Fields));
        }

        [TestMethod]
        public void FieldFilter_AnyField_1()
        {
            var filter = _GetFilter();
            var record = _GetRecord();

            Assert.IsTrue(filter.AnyField(record.Fields));
        }

        [TestMethod]
        public void FieldFilter_FilterFields_1()
        {
            var filter = _GetFilter();
            var record = _GetRecord();

            var filtered = filter.FilterFields
                (
                    record.Fields
                );
            Assert.AreEqual(1, filtered.Length);
            Assert.AreEqual
                (
                    "^aВторое",
                    filtered[0].ToText()
                );
        }

        [TestMethod]
        public void FieldFilter_First_1()
        {
            var filter = _GetFilter();
            var record = _GetRecord();

            var found = filter.First(record.Fields);
            Assert.IsNotNull(found);
            Assert.AreEqual
                (
                    "^aВторое",
                    found!.ToText()
                );
        }

        [TestMethod]
        public void FieldFilter_Last_1()
        {
            var filter = _GetFilter();
            var record = _GetRecord();

            var found = filter.Last(record.Fields);
            Assert.IsNotNull(found);
            Assert.AreEqual
                (
                    "^aВторое",
                    found!.ToText()
                );
        }
    }
}
