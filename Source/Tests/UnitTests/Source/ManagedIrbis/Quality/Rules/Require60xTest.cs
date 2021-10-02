// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Require60xTest
        : RuleTest
    {
        [TestMethod]
        public void Require60x_Construction_1()
        {
            var check = new Require60x();
            Assert.IsNotNull(check);
        }
    }
}
