// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.IO;
using AM.Text;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public sealed class LocalCatalogerIniFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void LocalCatalogerIniFile_Construction_1()
        {
            var file = new IniFile();
            var ini = new LocalCatalogerIniFile (file);
            Assert.IsNotNull (ini.Context);
            Assert.IsNotNull (ini.Ini);
            Assert.IsNotNull (ini.Main);
        }

    }
}
