// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Opt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class OptUtilityTest
    {
        private void _TestCompare
        (
            string left,
            string right,
            bool expected
        )
        {
            Assert.AreEqual
            (
                expected,
                OptUtility.CompareString
                (
                    left,
                    right
                )
            );
        }

        [TestMethod]
        public void OptUtility_CompareString_1()
        {
            _TestCompare("PAZK", "pazk", true);
            _TestCompare("PAZK", "PAZ", false);
            _TestCompare("PAZK", "PAZK2", false);
            _TestCompare("PAZ+", "PAZ", true);
            _TestCompare("PAZ+", "PAZK", true);
            _TestCompare("PAZ+", "PAZK2", false);
            _TestCompare("SPEC", "PAZK", false);
            _TestCompare("PA+K", "pazk", true);
            _TestCompare("PA+K", "PARK", true);
            _TestCompare("PA+K", "SPEC", false);
            _TestCompare("PA++", "PAZK", true);
            _TestCompare("+++++", string.Empty, true);
            _TestCompare("+++++", "PAZK", true);
        }

        [TestMethod]
        public void OptUtility_CompareString_2()
        {
            _TestCompare("PAZK", "", false);
            _TestCompare("+AZK", "PAZK", true);
            _TestCompare("P+ZK", "P", false);
            _TestCompare("P+++", "P", true);
        }
    }
}
