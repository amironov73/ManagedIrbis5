// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

using Moq;

#nullable enable

namespace UnitTests.AM.Text.Output
{
    [TestClass]
    public class DummyOutputTest
    {
        private AbstractOutput GetMock()
        {
            var mock = new Mock<AbstractOutput>();

            return mock.Object;
        }

        [TestMethod]
        public void DummyOutput_Construction_1()
        {
            var inner = GetMock();
            var outer = new DummyOutput(inner);

            Assert.IsFalse(outer.HaveError);
        }
    }
}
