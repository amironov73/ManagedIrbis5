// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public class FieldCollectionTest
    {
        [TestMethod]
        public void FieldCollection_Constructor_1()
        {
            var collection = new FieldCollection();
            Assert.IsNull(collection.Record);
            Assert.AreEqual(0, collection.Count);
            Assert.IsFalse(collection.ReadOnly);
        }

        [TestMethod]
        public void FieldCollection_Add_1()
        {
            var collection = new FieldCollection();
            var field1001 = new Field(100);
            var field1002 = new Field(100);
            var field2001 = new Field(200);
            var field2002 = new Field(200);
            collection.Add(field1001);
            collection.Add(field1002);
            collection.Add(field2001);
            collection.Add(field2002);
            Assert.IsNull(collection.Record);
            Assert.AreEqual(4, collection.Count);
            Assert.IsFalse(collection.ReadOnly);
            Assert.AreEqual(1, field1001.Repeat);
            Assert.AreEqual(2, field1002.Repeat);
            Assert.AreEqual(1, field2001.Repeat);
            Assert.AreEqual(2, field1002.Repeat);
        }

        [TestMethod]
        public void FieldCollection_AddCapacity_1()
        {
            var collection = new FieldCollection();
            collection.AddCapacity(100);
            Assert.IsNull(collection.Record);
            Assert.AreEqual(0, collection.Count);
            Assert.IsFalse(collection.ReadOnly);
        }

        [TestMethod]
        public void FieldCollection_AddRange_1()
        {
            var collection = new FieldCollection();
            collection.AddRange(new Field[] { new (100), new (200), new (300)});
            Assert.IsNull(collection.Record);
            Assert.AreEqual(3, collection.Count);
            Assert.IsFalse(collection.ReadOnly);
        }

        [TestMethod]
        public void FieldCollection_AsReadOnly_1()
        {
            var originalCollection = new FieldCollection();
            originalCollection.Add(new Field(100));
            originalCollection.Add(new Field(200));
            originalCollection.Add(new Field(300));
            var readOnlyCollection = originalCollection.AsReadOnly();
            Assert.IsNull(readOnlyCollection.Record);
            Assert.IsTrue(readOnlyCollection.ReadOnly);
            Assert.AreEqual(originalCollection.Count, readOnlyCollection.Count);
        }

        [TestMethod]
        public void FieldCollection_BeginUpdate_1()
        {
            var collection = new FieldCollection();
            collection.BeginUpdate();
            var field1001 = new Field(100);
            var field1002 = new Field(100);
            var field2001 = new Field(200);
            var field2002 = new Field(200);
            collection.Add(field1001);
            collection.Add(field1002);
            collection.Add(field2001);
            collection.Add(field2002);
            Assert.AreEqual(0, field1001.Repeat);
            Assert.AreEqual(0, field1002.Repeat);
            Assert.AreEqual(0, field2001.Repeat);
            Assert.AreEqual(0, field1002.Repeat);

            collection.EndUpdate();
            Assert.AreEqual(1, field1001.Repeat);
            Assert.AreEqual(2, field1002.Repeat);
            Assert.AreEqual(1, field2001.Repeat);
            Assert.AreEqual(2, field1002.Repeat);
        }

    }
}
