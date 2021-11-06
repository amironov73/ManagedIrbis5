// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertToLocalFunction
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public sealed class FieldUtilityTest
    {
        private Field _GetField() => new Field (100)
            .Add ('a', "SubA")
            .Add ('b', "SubB")
            .Add ('c', "SubC")
            .Add ('d', "SubD")
            .Add ('e', "SubE");

        private FieldCollection _GetFieldCollection() => new ()
        {
            new Field (100)
                .Add ('a', "SubA")
                .Add ('b', "SubB")
                .Add ('c', "SubC")
                .Add ('d', "SubD")
                .Add ('e', "SubE"),
            new Field (101)
                .Add ('f', "SubF")
                .Add ('g', "SubG")
                .Add ('h', "SubH")
                .Add ('i', "SubI")
                .Add ('j', "SubJ"),
            new Field (200)
                .Add ('k', "SubK")
                .Add ('l', "SubL")
                .Add ('m', "SubM")
                .Add ('n', "SubN")
                .Add ('o', "SubO"),
            new Field (200)
                .Add ('p', "SubP")
                .Add ('q', "SubQ")
                .Add ('r', "SubR")
                .Add ('s', "SubS")
                .Add ('t', "SubT")
        };

        private IEnumerable<Field> _GetFieldEnumeration()
        {
            yield return new Field (100)
                .Add ('a', "SubA")
                .Add ('b', "SubB")
                .Add ('c', "SubC")
                .Add ('d', "SubD")
                .Add ('e', "SubE");
            yield return new Field (101)
                .Add ('f', "SubF")
                .Add ('g', "SubG")
                .Add ('h', "SubH")
                .Add ('i', "SubI")
                .Add ('j', "SubJ");
            yield return new Field (200)
                .Add ('k', "SubK")
                .Add ('l', "SubL")
                .Add ('m', "SubM")
                .Add ('n', "SubN")
                .Add ('o', "SubO");
            yield return new Field (200)
                .Add ('p', "SubP")
                .Add ('q', "SubQ")
                .Add ('r', "SubR")
                .Add ('s', "SubS")
                .Add ('t', "SubT");
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_01()
        {
            var collection = _GetFieldCollection();
            var fields = FieldUtility.GetField (collection, 100);
            Assert.AreEqual (1, fields.Length);

            fields = FieldUtility.GetField (collection, 1000);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_02()
        {
            var collection = _GetFieldCollection();
            var field = FieldUtility.GetField (collection, 100, 0);
            Assert.IsNotNull (field);

            field = FieldUtility.GetField (collection, 100, 1);
            Assert.IsNull (field);

            field = FieldUtility.GetField (collection, 1000, 0);
            Assert.IsNull (field);

            field = FieldUtility.GetField (collection, 200, 0);
            Assert.IsNotNull (field);

            field = FieldUtility.GetField (collection, 200, 1);
            Assert.IsNotNull (field);

            field = FieldUtility.GetField (collection, 200, 2);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_03()
        {
            IEnumerable<Field> enumeration = _GetFieldEnumeration();
            var fields = FieldUtility.GetField (enumeration, 100);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldUtility.GetField (enumeration, 1000);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_04()
        {
            var enumeration = _GetFieldEnumeration();
            var field = FieldUtility.GetField (enumeration, 100, 0);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, 100, 1);
            Assert.IsNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, 1000, 0);
            Assert.IsNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, 200, 0);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, 200, 1);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, 200, 2);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_05()
        {
            var count = 0;
            var enumeration = _GetFieldEnumeration();
            Action<Field> action = _ => { count++; };
            var fields = FieldUtility.GetField (enumeration, action);
            Assert.AreEqual (4, fields.Length);
            Assert.AreEqual (4, count);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_06()
        {
            int fieldCount = 0, subFieldCount = 0;
            var enumeration = _GetFieldEnumeration();
            Action<Field> fieldAction = _ => { fieldCount++; };
            Action<SubField> subFieldAction = _ => { subFieldCount++; };
            Field[] fields = FieldUtility.GetField (enumeration, fieldAction, subFieldAction);
            Assert.AreEqual (4, fields.Length);
            Assert.AreEqual (4, fieldCount);
            Assert.AreEqual (20, subFieldCount);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_07()
        {
            var subFieldCount = 0;
            IEnumerable<Field> enumeration = _GetFieldEnumeration();
            Action<SubField> action = _ => { subFieldCount++; };
            var fields = FieldUtility.GetField (enumeration, action);
            Assert.AreEqual (4, fields.Length);
            Assert.AreEqual (20, subFieldCount);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_08()
        {
            var enumeration = _GetFieldEnumeration();
            Func<SubField, bool> predicate = subField => subField.Code == 'a';
            var fields = FieldUtility.GetField (enumeration, predicate);
            Assert.AreEqual (1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_09()
        {
            var enumeration = _GetFieldEnumeration();
            var codes = new[] { 'a', 'b' };
            Func<SubField, bool> predicate = subField => subField.Value.SafeContains ("Sub");
            Field[] fields = FieldUtility.GetField (enumeration, codes, predicate);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'a', 'x' };
            predicate = subField => subField.Value.SafeContains ("Sub");
            fields = FieldUtility.GetField (enumeration, codes, predicate);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'a', 'x' };
            predicate = subField => subField.Value.SafeContains ("Sib");
            fields = FieldUtility.GetField (enumeration, codes, predicate);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'x', 'z' };
            predicate = subField => subField.Value.SafeContains ("Sub");
            fields = FieldUtility.GetField (enumeration, codes, predicate);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_10()
        {
            var enumeration = _GetFieldEnumeration();
            var codes = new[] { 'a', 'b' };
            var values = new[] { "SubA" };
            var fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'a', 'z' };
            values = new[] { "SubA", "SubZ" };
            fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'a', 'b' };
            values = new[] { "SubX", "SubZ" };
            fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'x', 'z' };
            values = new[] { "SubX", "SubZ" };
            fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_11()
        {
            var enumeration = _GetFieldEnumeration();
            var codes = new[] { 'a', 'b' };
            var values = new[] { "SubA" };
            var fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'a', 'x' };
            values = new[] { "SubA", "SubX" };
            fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'x', 'z' };
            values = new[] { "SubX", "SubZ" };
            fields = FieldUtility.GetField (enumeration, codes, values);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_12()
        {
            var enumeration = _GetFieldEnumeration();
            var fields = FieldUtility.GetField (enumeration, 'a', "SubA");
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldUtility.GetField (enumeration, 'a', "SubB");
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldUtility.GetField (enumeration, 'x', "SubX");
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_13()
        {
            var enumeration = _GetFieldEnumeration();
            var tags = new[] { 100 };
            var codes = new[] { 'a', 'b' };
            var values = new[] { "SubA" };
            var fields = FieldUtility.GetField (enumeration, tags, codes, values);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 300 };
            codes = new[] { 'a', 'b' };
            values = new[] { "SubA" };
            fields = FieldUtility.GetField (enumeration, tags, codes, values);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100 };
            codes = new[] { 'a', 'b' };
            values = new[] { "SubZ" };
            fields = FieldUtility.GetField (enumeration, tags, codes, values);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100 };
            codes = new[] { 'x', 'z' };
            values = new[] { "SubZ" };
            fields = FieldUtility.GetField (enumeration, tags, codes, values);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_14()
        {
            var enumeration = _GetFieldEnumeration();
            Func<Field, bool> fieldPredicate = field => field.Tag < 200;
            Func<SubField, bool> subFieldPredicate = subField => subField.Code < 'c';
            var fields = FieldUtility.GetField (enumeration, fieldPredicate, subFieldPredicate);
            Assert.AreEqual (1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_15()
        {
            var enumeration = _GetFieldEnumeration();
            Func<Field, bool> fieldPredicate = field => field.Tag < 200;
            var fields = FieldUtility.GetField (enumeration, fieldPredicate);
            Assert.AreEqual (2, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_16()
        {
            var enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            var field = FieldUtility.GetField (enumeration, tags, 0);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetField (enumeration, tags, 10);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_17()
        {
            var collection = _GetFieldCollection();
            Func<Field, bool> predicate = field => field.HaveSubField ('a');
            var fields = FieldUtility.GetField (collection, predicate);
            Assert.AreEqual (1, fields.Length);

            predicate = field => field.HaveSubField ('j');
            fields = FieldUtility.GetField (collection, predicate);
            Assert.AreEqual (1, fields.Length);

            predicate = field => field.HaveSubField ('x');
            fields = FieldUtility.GetField (collection, predicate);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_18()
        {
            var collection = _GetFieldCollection();
            int[] tags = { 100, 200 };
            var field = FieldUtility.GetField (collection, tags, 1);
            Assert.IsNotNull (field);

            field = FieldUtility.GetField (collection, tags, 10);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_19()
        {
            var collection = _GetFieldCollection();
            int[] tags = { 100, 200 };
            var fields = FieldUtility.GetField (collection, tags);
            Assert.IsNotNull (fields);
            Assert.AreEqual (3, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldCount_1()
        {
            var collection = _GetFieldCollection();
            Assert.AreEqual (1, FieldUtility.GetFieldCount (collection, 100));
            Assert.AreEqual (2, FieldUtility.GetFieldCount (collection, 200));
            Assert.AreEqual (0, FieldUtility.GetFieldCount (collection, 300));
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldCount_2()
        {
            var enumeration = _GetFieldEnumeration();
            Assert.AreEqual (1, FieldUtility.GetFieldCount (enumeration, 100));

            enumeration = _GetFieldEnumeration();
            Assert.AreEqual (2, FieldUtility.GetFieldCount (enumeration, 200));

            enumeration = _GetFieldEnumeration();
            Assert.AreEqual (0, FieldUtility.GetFieldCount (enumeration, 300));
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_1()
        {
            var enumeration = _GetFieldEnumeration();
            var fields = FieldUtility.GetFieldRegex (enumeration, "^[12]");
            Assert.AreEqual (4, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldUtility.GetFieldRegex (enumeration, "^[89]");
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_2()
        {
            var enumeration = _GetFieldEnumeration();
            var field = FieldUtility.GetFieldRegex (enumeration, "^[12]", 2);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetFieldRegex (enumeration, "^[12]", 5);
            Assert.IsNull (field);

            enumeration = _GetFieldEnumeration();
            field = FieldUtility.GetFieldRegex (enumeration, "^[89]", 0);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_3()
        {
            var enumeration = _GetFieldEnumeration();
            var tags = new[] { 100, 200 };
            var codes = new[] { 'a', 'b' };
            var textRegex = "^Sub";
            var fields = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex);
            Assert.AreEqual (1, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100, 200 };
            codes = new[] { 'a', 'b' };
            textRegex = "^Sib";
            fields = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100, 200 };
            codes = new[] { 'x', 'z' };
            textRegex = "^Sub";
            fields = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex);
            Assert.AreEqual (0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 300, 400 };
            codes = new[] { 'a', 'b' };
            textRegex = "^Sub";
            fields = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_4()
        {
            var enumeration = _GetFieldEnumeration();
            var tags = new[] { 100, 200 };
            var codes = new[] { 'a', 'b' };
            var textRegex = "^Sub";
            var field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 0);
            Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100, 200 };
            codes = new[] { 'a', 'b' };
            textRegex = "^Sib";
            field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 0);
            Assert.IsNull (field);

            // enumeration = _GetFieldEnumeration();
            // tags = new[] { 100, 200 };
            // codes = new[] { 'a', 'b' };
            // textRegex = "^Sub";
            // field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 1);
            // Assert.IsNotNull (field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100, 200 };
            codes = new[] { 'a', 'b' };
            textRegex = "^Sub";
            field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 2);
            Assert.IsNull (field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 100, 200 };
            codes = new[] { 'x', 'z' };
            textRegex = "^Sub";
            field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 0);
            Assert.IsNull (field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { 300, 400 };
            codes = new[] { 'a', 'b' };
            textRegex = "^Sub";
            field = FieldUtility.GetFieldRegex (enumeration, tags, codes, textRegex, 0);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_5()
        {
            var enumeration = _GetFieldEnumeration();
            var tags = new[] { 100, 200 };
            var textRegex = "^Sub";
            var fields = FieldUtility.GetFieldRegex (enumeration, tags, textRegex);
            Assert.AreEqual (0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_6()
        {
            var enumeration = _GetFieldEnumeration();
            var tags = new[] { 100, 200 };
            var textRegex = "^Sub";
            var field = FieldUtility.GetFieldRegex (enumeration, tags, textRegex, 0);
            Assert.IsNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_7()
        {
            var enumeration = new Field[]
            {
                new (100, "Field100"),
                new (200, "TheField200"),
            };
            var tags = new[] { 100, 200 };
            var textRegex = "^Field";
            var fields = FieldUtility.GetFieldRegex (enumeration, tags, textRegex);
            Assert.AreEqual (1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_8()
        {
            var enumeration = new Field[]
            {
                new (100, "Field100"),
                new (200, "TheField200"),
            };
            var tags = new[] { 100, 200 };
            var textRegex = "^Field";
            var field = FieldUtility.GetFieldRegex (enumeration, tags, textRegex, 0);
            Assert.IsNotNull (field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_1()
        {
            var enumeration = new Field[]
            {
                new (100, "The text"),
                new (200, "Other text"),
                new (300),
                new (400, "Text again")
            };
            var values = FieldUtility.GetFieldValue (enumeration);
            Assert.AreEqual (3, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_2()
        {
            var enumeration = _GetFieldCollection();
            var values = FieldUtility.GetFieldValue (enumeration);
            Assert.AreEqual (0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_3()
        {
            var enumeration = _GetFieldEnumeration();
            var values = FieldUtility.GetFieldValue (enumeration, 100);
            Assert.AreEqual (0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_4()
        {
            var enumeration = _GetFieldCollection();
            var values = FieldUtility.GetFieldValue (enumeration, 100);
            Assert.AreEqual (0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_5()
        {
            var enumeration = new FieldCollection
            {
                new (100, "Field100"),
                new (200, "TheField200"),
            };
            var values = FieldUtility.GetFieldValue (enumeration, 100);
            Assert.AreEqual (1, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_6()
        {
            var enumeration = new FieldCollection
            {
                new (100, "Field100"),
                new (200, "TheField200"),
            };
            var values = FieldUtility.GetFieldValue (enumeration);
            Assert.AreEqual (2, values.Length);
        }

        // [TestMethod]
        // public void RecordFieldUtility_GetFieldValue_7()
        // {
        //     Field? field = null;
        //     Assert.IsNull (FieldUtility.GetFieldValue (field));
        //
        //     field = new Field (100);
        //     Assert.IsNull (FieldUtility.GetFieldValue (field));
        //
        //     var value = "Field100";
        //     field = new Field (100, value);
        //     Assert.AreSame (value, FieldUtility.GetFieldValue (field));
        // }
    }
}
