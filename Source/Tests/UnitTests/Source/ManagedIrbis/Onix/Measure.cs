// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class MeasureTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Addressee_Construction_1()
        {
            var measure = new Measure();
            Assert.IsNull (measure.Measurement);
            Assert.IsNull (measure.Type);
            Assert.IsNull (measure.Unit);
        }
    }
}
