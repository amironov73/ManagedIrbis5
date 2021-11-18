// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class CodeDigitTest
    {
        [TestMethod]
        [Description ("Извлечение цифр из идентификатора")]
        public void CodeDigit_ExtractDigits_1()
        {
            CodeDigit[] allowed =
            {
                new ('1', 1),
                new ('2', 2),
                new ('X', 10)
            };
            var identifier = "AXB2C1Z";
            var extracted = CodeDigit.ExtractDigits (identifier, allowed);
            Assert.AreEqual (3, extracted.Length);
            Assert.AreEqual ('X', extracted[0].Digit);
            Assert.AreEqual ('2', extracted[1].Digit);
            Assert.AreEqual ('1', extracted[2].Digit);
        }

        [TestMethod]
        [Description ("Поиск символа среди разрешенных")]
        public void CodeDigit_FindDigit_1()
        {
            CodeDigit[] allowed =
            {
                new ('1', 1),
                new ('2', 2),
                new ('X', 10)
            };
            Assert.AreEqual ('1', CodeDigit.FindDigit ('1', allowed)!.Value.Digit);
            Assert.IsNull (CodeDigit.FindDigit ('Z', allowed));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void CodeDigit_ToString_1()
        {
            var code = new CodeDigit ('1', 1);
            Assert.AreEqual ("1", code.ToString());
        }
    }
}
