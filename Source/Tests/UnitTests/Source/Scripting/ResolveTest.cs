// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Sprache;

#nullable enable

namespace UnitTests.Source.Scripting
{
    [TestClass]
    public sealed class ResolveTest
    {
        [TestMethod]
        [Description ("Знак минус")]
        public void Resolve_Minus_1()
        {
            Assert.AreEqual ('-', Resolve.Minus.Parse ("-"));
            Assert.ThrowsException<ParseException> (() => Resolve.Minus.Parse ("+"));
        }

        [TestMethod]
        [Description ("Арабские цифры")]
        public void Resolve_Arabic_1()
        {
            Assert.AreEqual ('0', Resolve.Arabic.Parse ("0"));
            Assert.ThrowsException<ParseException> (() => Resolve.Arabic.Parse ("-"));
        }

        [TestMethod]
        [Description ("Целое без знака")]
        public void Resolve_UnsignedInteger_1()
        {
            Assert.AreEqual (0U, Resolve.UnsignedInteger.End().Parse ("0"));
            Assert.AreEqual (123U, Resolve.UnsignedInteger.End().Parse ("123"));
            Assert.ThrowsException<ParseException> (() => Resolve.UnsignedInteger.End().Parse ("-0"));
        }

        [TestMethod]
        [Description ("Целое со знаком")]
        public void Resolve_SignedInteger_1()
        {
            Assert.AreEqual (0, Resolve.SignedInteger.End().Parse ("0"));
            Assert.AreEqual (123, Resolve.SignedInteger.End().Parse ("123"));
            Assert.AreEqual (-123, Resolve.SignedInteger.End().Parse ("-123"));
            Assert.ThrowsException<ParseException> (() => Resolve.SignedInteger.End().Parse (""));
        }

        [TestMethod]
        [Description ("Длинное целое без знака")]
        public void Resolve_UnsignedLong_1()
        {
            Assert.AreEqual (0UL, Resolve.UnsignedLong.End().Parse ("0"));
            Assert.AreEqual (123UL, Resolve.UnsignedLong.End().Parse ("123"));
            Assert.ThrowsException<ParseException> (() => Resolve.UnsignedLong.End().Parse ("-0"));
        }

        [TestMethod]
        [Description ("Длинное целое со знаком")]
        public void Resolve_SignedLong_1()
        {
            Assert.AreEqual (0L, Resolve.SignedLong.End().Parse ("0"));
            Assert.AreEqual (123L, Resolve.SignedLong.End().Parse ("123"));
            Assert.AreEqual (-123L, Resolve.SignedLong.End().Parse ("-123"));
            Assert.ThrowsException<ParseException> (() => Resolve.SignedLong.End().Parse (""));
        }

        [TestMethod]
        [Description ("Число с плавающей точкой одинарной точности")]
        public void Resolve_Float_1()
        {
            Assert.AreEqual (0.0f, Resolve.Float.End().Parse ("0"));
            Assert.AreEqual (1.0f, Resolve.Float.End().Parse ("1"));
            Assert.AreEqual (-1.0f, Resolve.Float.End().Parse ("-1"));

            Assert.AreEqual (0.0f, Resolve.Float.End().Parse ("0.0"));
            Assert.AreEqual (1.0f, Resolve.Float.End().Parse ("1.0"));
            Assert.AreEqual (-1.0f, Resolve.Float.End().Parse ("-1.0"));

            Assert.AreEqual (0.1f, Resolve.Float.End().Parse ("0.1"));
            Assert.AreEqual (1.1f, Resolve.Float.End().Parse ("1.1"));
            Assert.AreEqual (-1.1f, Resolve.Float.End().Parse ("-1.1"));

            Assert.AreEqual (1.1e3f, Resolve.Float.End().Parse ("1.1e3"));
            Assert.AreEqual (1.1e-3f, Resolve.Float.End().Parse ("1.1e-3"));
            Assert.AreEqual (-1.1f, Resolve.Float.End().Parse ("-1.1"));
            Assert.AreEqual (-1.1e3f, Resolve.Float.End().Parse ("-1.1e3"));
            Assert.AreEqual (-1.1e-3f, Resolve.Float.End().Parse ("-1.1e-3"));
        }

        [TestMethod]
        [Description ("Число с плавающей точкой двойной точности")]
        public void Resolve_Double_1()
        {
            Assert.AreEqual (0.0, Resolve.Double.End().Parse ("0"));
            Assert.AreEqual (1.0, Resolve.Double.End().Parse ("1"));
            Assert.AreEqual (-1.0, Resolve.Double.End().Parse ("-1"));

            Assert.AreEqual (0.0, Resolve.Double.End().Parse ("0.0"));
            Assert.AreEqual (1.0, Resolve.Double.End().Parse ("1.0"));
            Assert.AreEqual (-1.0, Resolve.Double.End().Parse ("-1.0"));

            Assert.AreEqual (0.1, Resolve.Double.End().Parse ("0.1"));
            Assert.AreEqual (1.1, Resolve.Double.End().Parse ("1.1"));
            Assert.AreEqual (-1.1, Resolve.Double.End().Parse ("-1.1"));

            Assert.AreEqual (1.1e3, Resolve.Double.End().Parse ("1.1e3"));
            Assert.AreEqual (1.1e-3, Resolve.Double.End().Parse ("1.1e-3"));
            Assert.AreEqual (-1.1, Resolve.Double.End().Parse ("-1.1"));
            Assert.AreEqual (-1.1e3, Resolve.Double.End().Parse ("-1.1e3"));
            Assert.AreEqual (-1.1e-3, Resolve.Double.End().Parse ("-1.1e-3"));
        }

        [TestMethod]
        [Description ("Идентификатор")]
        public void Resolve_Identifier_1()
        {
            Assert.AreEqual ("a", Resolve.Identifier.End ().Parse ("a"));
            Assert.AreEqual ("a1", Resolve.Identifier.End ().Parse ("a1"));
            Assert.AreEqual ("_a1", Resolve.Identifier.End ().Parse ("_a1"));
            Assert.AreEqual ("_", Resolve.Identifier.End ().Parse ("_"));
            Assert.AreEqual ("a_1", Resolve.Identifier.End ().Parse ("a_1"));
            Assert.AreEqual ("_1", Resolve.Identifier.End ().Parse ("_1"));
        }

    }
}
