// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex
{
    [TestClass]
    public sealed class KnownTagsTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void KnownTags_ListValues_1()
        {
            var values = KnownTags.ListValues();
            Assert.AreEqual (26, values.Length);
        }
    }
}
