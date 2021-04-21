using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

#nullable enable

namespace UnitTests.AM
{
    [TestClass]
    public class ResultTest
    {
        [TestMethod]
        public void Result_Succeed_1()
        {
            var result = Result<string, int>.Succeed("Hello");
            Assert.AreEqual("Hello", result.Value);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Error);
            Assert.IsTrue(result);
            Assert.AreEqual("Success: Hello", result.ToString());

            string value = result;
            Assert.AreEqual("Hello", value);

            value = result.ValueOr("Other");
            Assert.AreEqual("Hello", value);

            value = result.ValueOr(() => "Other");
            Assert.AreEqual("Hello", value);
        }

        [TestMethod]
        public void Result_Failure_1()
        {
            var result = Result<string, int>.Failure(123);
            Assert.AreEqual(123, result.Error);
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result);
            Assert.AreEqual("Failure: 123", result.ToString());

            string value = result.ValueOr("Other");
            Assert.AreEqual("Other", value);

            value = result.ValueOr(() => "Other");
            Assert.AreEqual("Other", value);
        }

        [TestMethod]
        public void Result_FailureWithInfo_1()
        {
            var result = Result<string, ErrorInfo<int>>.FailureWithInfo
                (
                    123,
                    "Some description"
                );
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(123, result.Error!.Code);
            Assert.AreEqual("Some description", result.Error!.ErrorDescription);
        }
    }
}
