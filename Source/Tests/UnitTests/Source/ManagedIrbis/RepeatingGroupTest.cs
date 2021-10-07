// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public sealed class RepeatingGroupTest
    {
        [TestMethod]
        public void RepeatingGroup_V_1()
        {
            var output = new StringWriter();
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var result = RepeatingGroup.V (output, record, 910, 'b', after: ", ", skipLast: true);
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual ("N001, N002, N003", actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, 'a', after: ", ", skipLast: true);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, after: ", ", skipLast: true);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_V_2()
        {
            var output = new StringWriter();
            var record = new Record();
            var result = RepeatingGroup.V (output, record, 910, 'b', after: ", ", skipLast: true);
            var actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, after: ", ", skipLast: true);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_V_3()
        {
            var output = new StringWriter();
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var result = RepeatingGroup.V (output, record, 910, 'b', after: ", ", skipLast: false);
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual ("N001, N002, N003, ", actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, 'a', after: ", ", skipLast: false);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, after: ", ", skipLast: false);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_V_4()
        {
            var output = new StringWriter();
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var result = RepeatingGroup.V (output, record, 910, 'b', before: " + ", skipFirst: true);
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual ("N001 + N002 + N003", actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, 'a', before: " + ", skipFirst: true);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, after: " + ", skipFirst: true);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_V_5()
        {
            var output = new StringWriter();
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var result = RepeatingGroup.V (output, record, 910, 'b', before: " + ", skipFirst: false);
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual (" + N001 + N002 + N003", actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, 'a', before: " + ", skipFirst: false);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, after: " + ", skipFirst: false);
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_V_6()
        {
            var output = new StringWriter();
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var result = RepeatingGroup.V (output, record, 910, 'b', prefix: "[", after: ",", skipLast: true, suffix: "]");
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual ("[N001,N002,N003]", actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, 'a', prefix: "[", after: ",", skipLast: true, suffix: "]");
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);

            output = new StringWriter();
            result = RepeatingGroup.V (output, record, 910, prefix: "[", after: ",", skipLast: true, suffix: "]");
            actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void RepeatingGroup_Enumerator_1()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var group = new RepeatingGroup (record, 910);
            var expected = new [] { "N001", "N002", "N003" };
            var countLast = 0;

            foreach (var repeat in group)
            {
                if (repeat.IsLast)
                {
                    ++countLast;
                    Assert.AreEqual (2, repeat.Index);
                }

                Assert.AreEqual (expected [repeat.Index], repeat.FM ('b'));
                Assert.AreEqual (expected [repeat.Index], repeat.FM (910, 'b'));
                Assert.IsNull (repeat.FM ('a'));
                Assert.IsNull (repeat.FM (910));
                Assert.IsNull (repeat.FM ());
            }

            Assert.AreEqual (1, countLast);
        }
    }
}
