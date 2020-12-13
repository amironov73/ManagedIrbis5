using System.IO;
using System.Text;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable RedundantCast

namespace UnitTests.AM.IO
{
    [TestClass]
    public class ValueByteNavigatorTest
        : Common.CommonUnitTest
    {
        private byte[] _GetData()
        {
            // 9 bytes
            return new byte[] {3, 14, 15, 9, 26, 5, 35, 89, 79};
        }

        private ValueByteNavigator _GetNavigator()
        {
            return new ValueByteNavigator(_GetData());
        }

        [TestMethod]
        public void ValueByteNavigator_Construction_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(9, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(Encoding.Default, navigator.Encoding);
        }

        [TestMethod]
        public void ValueByteNavigator_Clone_1()
        {
            ValueByteNavigator first = _GetNavigator();
            ValueByteNavigator second = first.Clone();
            Assert.AreEqual(first.Position, second.Position);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreSame(first.Encoding, second.Encoding);
        }

        [TestMethod]
        public void ValueByteNavigator_FromFile_1()
        {
            string fileName = Path.Combine(TestDataPath, "EMPTY.MST");
            ValueByteNavigator navigator = ValueByteNavigator.FromFile(fileName);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(36, navigator.Length);
            Assert.AreEqual(Encoding.Default, navigator.Encoding);
        }

        [TestMethod]
        public void ValueByteNavigator_GetRemainingData_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(2);
            var data = navigator.GetRemainingData();
            Assert.AreEqual(7, data.Length);
            Assert.AreEqual((byte)15, data[0]);
        }

        [TestMethod]
        public void ValueByteNavigator_GetRemainingData_2()
        {
            ValueByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(9);
            var data = navigator.GetRemainingData();
            Assert.IsTrue(data.IsEmpty);
        }

        [TestMethod]
        public void ValueByteNavigator_IsControl_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsTrue(navigator.IsControl());
        }

        [TestMethod]
        public void ValueByteNavigator_IsDigit_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsDigit());
        }

        [TestMethod]
        public void ValueByteNavigator_IsEof_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsEOF);
            navigator.MoveAbsolute(navigator.Length);
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void ValueByteNavigator_IsLetter_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsLetter());
        }

        [TestMethod]
        public void ValueByteNavigator_IsLetterOrDigit_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsLetterOrDigit());
        }

        [TestMethod]
        public void ValueByteNavigator_IsNumber_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsNumber());
        }

        [TestMethod]
        public void ValueByteNavigator_IsPunctuation_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsPunctuation());
        }

        [TestMethod]
        public void ValueByteNavigator_IsSeparator_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSeparator());
        }

        [TestMethod]
        public void ValueByteNavigator_IsSurrogate_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSurrogate());
        }

        [TestMethod]
        public void ValueByteNavigator_IsSymbol_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSymbol());
        }

        [TestMethod]
        public void ValueByteNavigator_IsWhiteSpace_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsWhiteSpace());
        }

        [TestMethod]
        public void ValueByteNavigator_MoveAbsolute_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(1);
            Assert.AreEqual(1, navigator.Position);

            navigator.MoveAbsolute(9);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveAbsolute(-9);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveAbsolute(99);
            Assert.AreEqual(9, navigator.Position);
        }

        [TestMethod]
        public void ValueByteNavigator_MoveRelative_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            navigator.MoveRelative(1);
            Assert.AreEqual(1, navigator.Position);

            navigator.MoveRelative(8);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveRelative(1);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveRelative(-9);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveRelative(-1);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveRelative(99);
            Assert.AreEqual(9, navigator.Position);
        }

        [TestMethod]
        public void ValueByteNavigator_PeekByte_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.AreEqual((byte)3, navigator.PeekByte());
            Assert.AreEqual((byte)3, navigator.PeekByte());

            navigator.MoveRelative(1);
            Assert.AreEqual((byte)14, navigator.PeekByte());
            Assert.AreEqual((byte)14, navigator.PeekByte());

            navigator.MoveAbsolute(99);
            Assert.AreEqual(ValueByteNavigator.EOF, navigator.PeekByte());
            Assert.AreEqual(ValueByteNavigator.EOF, navigator.PeekByte());
        }

        [TestMethod]
        public void ValueByteNavigator_PeekChar_1()
        {
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            Assert.AreEqual('H', navigator.PeekChar());
            Assert.AreEqual('H', navigator.PeekChar());
            navigator.MoveAbsolute(99);
            Assert.AreEqual('\0', navigator.PeekChar());
            Assert.AreEqual('\0', navigator.PeekChar());
        }

        [TestMethod]
        public void ValueByteNavigator_ReadByte_1()
        {
            ValueByteNavigator navigator = _GetNavigator();
            Assert.AreEqual(3, navigator.ReadByte());
            Assert.AreEqual(14, navigator.ReadByte());
            navigator.MoveAbsolute(99);
            Assert.AreEqual(-1, navigator.ReadByte());
            Assert.AreEqual(-1, navigator.ReadByte());
        }

        [TestMethod]
        public void ValueByteNavigator_ReadChar_1()
        {
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            Assert.AreEqual('H', navigator.ReadChar());
            Assert.AreEqual('e', navigator.ReadChar());
            navigator.MoveAbsolute(99);
            Assert.AreEqual('\0', navigator.ReadChar());
            Assert.AreEqual('\0', navigator.ReadChar());
        }

        /*
        [TestMethod]
        public void ValueByteNavigator_ReadLine_1()
        {
            // CR + LF
            byte[] data = {72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100};
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            Assert.AreEqual("Hello", navigator.ReadLine().ToString());
            Assert.AreEqual("World", navigator.ReadLine().ToString());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        }

        [TestMethod]
        public void ValueByteNavigator_ReadLine_2()
        {
            // LF only
            byte[] data = {72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100};
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            Assert.AreEqual("Hello", navigator.ReadLine().ToString());
            Assert.AreEqual("World", navigator.ReadLine().ToString());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        }

        [TestMethod]
        public void ValueByteNavigator_SkipLine_1()
        {
            // CR + LF
            byte[] data = { 72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100 };
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            navigator.ReadChar();
            navigator.SkipLine();
            Assert.AreEqual("World", navigator.ReadLine().ToString());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        }

        [TestMethod]
        public void ValueByteNavigator_SkipLine_2()
        {
            // LF only
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ValueByteNavigator navigator = new ValueByteNavigator(data);
            navigator.ReadChar();
            navigator.SkipLine();
            Assert.AreEqual("World", navigator.ReadLine().ToString());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        }
        */

        [TestMethod]
        public void ValueByteNavigator_SkipLine_3()
        {
            ValueByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(99);
            navigator.SkipLine();
            Assert.IsTrue(navigator.IsEOF);
        }
    }
}
