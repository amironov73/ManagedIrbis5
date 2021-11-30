// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public sealed class AthrbGuidelinesTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthrbGuidelines_Construction_1()
        {
            var athrb = new AthrbGuidelines();
            Assert.IsNull (athrb.Guidelines);
        }
    }
}
