// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public sealed class AthrbHeadingTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthrbHeading_Construction_1()
        {
            var heading = new AthrbHeading();
            Assert.IsNull (heading.Code1);
            Assert.IsNull (heading.Code2);
            Assert.IsNull (heading.Code3);
            Assert.IsNull (heading.Code4);
            Assert.IsNull (heading.Code5);
            Assert.IsNull (heading.Heading);
        }
    }
}
