﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Parameters;
using AM.Runtime;

// ReSharper disable CheckNamespace

namespace UnitTests.AM.Parameters
{
    [TestClass]
    public class ParameterTest
    {
        [TestMethod]
        public void Parameter_Construction_1()
        {
            var parameter = new Parameter();
            Assert.IsNull(parameter.Name);
            Assert.IsNull(parameter.Value);
        }

        private void _TestSerialization
            (
                Parameter first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Parameter>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void Parameter_Serialize_1()
        {
            var parameter = new Parameter();
            _TestSerialization(parameter);

            parameter = new Parameter("Name", "Value");
            _TestSerialization(parameter);
        }

        [TestMethod]
        public void Parameter_Verify_1()
        {
            var parameter = new Parameter();
            Assert.IsFalse(parameter.Verify(false));

            parameter = new Parameter("Name", "Value");
            Assert.IsTrue(parameter.Verify(false));
        }

        [TestMethod]
        public void Parameter_ToString_1()
        {
            var parameter = new Parameter("Name", "Value");
            Assert.AreEqual
                (
                    "Name=Value",
                    parameter.ToString()
                );
        }
    }
}
