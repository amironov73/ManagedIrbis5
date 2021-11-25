// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.Parameters
{
    [TestClass]
    public sealed class ActualizeRecordParametersTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ActualizeRecordParameters_Construction_1()
        {
            var parameters = new ActualizeRecordParameters();
            Assert.IsNull (parameters.Database);
            Assert.AreEqual (0, parameters.Mfn);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void ActualizeRecordParameters_Construction_2()
        {
            var parameters = new ActualizeRecordParameters
            {
                Database = "IBIS",
                Mfn = 123
            };
            Assert.AreEqual ("IBIS", parameters.Database);
            Assert.AreEqual (123, parameters.Mfn);
        }
    }
}
