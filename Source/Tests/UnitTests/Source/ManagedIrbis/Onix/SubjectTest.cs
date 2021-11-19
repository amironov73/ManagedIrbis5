// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SubjectTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Subject_Construction_1()
        {
            var subject = new Subject();
            Assert.IsNull (subject.Code);
            Assert.IsNull (subject.Main);
            Assert.IsNull (subject.SchemeId);
            Assert.IsNull (subject.SchemeVersion);
        }
    }
}
