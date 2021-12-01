// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public sealed class BookSellingIndexTest
    {
        [TestMethod]
        public void BookSellingIndex_Construction_1()
        {
            var index = new BookSellingIndex();
            Assert.IsNull (index.AuthorSign);
            Assert.IsNull (index.Classification);
            Assert.IsNull (index.Number);
            Assert.IsNull (index.Publisher);
            Assert.IsNull (index.Department);
            Assert.IsNull (index.Year);
            Assert.IsNull (index.Announcement);
        }
    }
}
