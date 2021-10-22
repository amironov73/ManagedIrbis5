using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Magazines;

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class BindingSpecificationTest
        : CommonMagazineTest
    {
        [TestMethod]
        public void BindingSpecification_Construction_1()
        {
            var specification = new BindingSpecification();
            Assert.IsNull(specification.MagazineIndex);
            Assert.IsNull(specification.Year);
            Assert.IsNull(specification.IssueNumbers);
            Assert.IsNull(specification.Description);
            Assert.IsNull(specification.BindingNumber);
            Assert.IsNull(specification.Inventory);
            Assert.IsNull(specification.Place);
            Assert.IsNull(specification.Complect);
        }
    }
}
