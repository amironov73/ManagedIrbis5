﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text.Output;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.AM.Text.Output
{
    [TestClass]
    public class NullOutputTest
    {
        [TestMethod]
        public void NullOutput_Construction_1()
        {
            using var output = new NullOutput();
            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void NullOutput_Clear_1()
        {
            using var output = new NullOutput();
            output.Clear();
            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void NullOutput_Configure_1()
        {
            using var output = new NullOutput();
            output.Configure(string.Empty);
            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void NullOutput_Write_1()
        {
            using var output = new NullOutput();
            output.Write("Hello");
            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void NullOutput_WriteError_1()
        {
            using var output = new NullOutput();
            output.WriteError("Hello");
            Assert.IsTrue(output.HaveError);

            output.Clear();
            Assert.IsFalse(output.HaveError);
        }
    }
}
