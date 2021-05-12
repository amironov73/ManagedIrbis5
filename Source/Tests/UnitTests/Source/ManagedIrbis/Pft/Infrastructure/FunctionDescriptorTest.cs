// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FunctionDescriptorTest
    {
        private FunctionDescriptor _GetDescriptor()
        {
            PftFunction function = (context, node, _) =>
            {
                context.WriteLine(node, "Do something");
            };
            var result = new FunctionDescriptor
            {
                Name = "SuperFunction",
                Description = "Typical God function",
                Signature = new[]
                {
                    FunctionParameter.String,
                    FunctionParameter.Boolean,
                    FunctionParameter.Numeric
                },
                Function = function
            };

            return result;
        }

        [TestMethod]
        public void FunctionDescriptor_Construction_1()
        {
            var descriptor = new FunctionDescriptor();
            Assert.IsNull(descriptor.Name);
            Assert.IsNull(descriptor.Description);
            Assert.IsNull(descriptor.Signature);
            Assert.IsNull(descriptor.Function);
        }

        [TestMethod]
        public void FunctionDescriptor_ToString_1()
        {
            var descriptor = _GetDescriptor();
            Assert.AreEqual("SuperFunction", descriptor.ToString());
        }
    }
}
