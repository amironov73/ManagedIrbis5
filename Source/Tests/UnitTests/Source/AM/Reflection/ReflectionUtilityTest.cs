// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

#nullable enable

namespace UnitTests.AM.Reflection
{
    class Dummy
    {
        public int Number { get; set; }
        public string? Text { get; set; }

        public bool Status => true;
    }

    [TestClass]
    public class ReflectionUtilityTest
    {
        [TestMethod]
        public void ReflectionUtility_CreateGetter_1()
        {
            var dummy = new Dummy { Number = 1, Text = "Hello" };
            var numberGetter = ReflectionUtility.CreateGetter<Dummy, int>("Number");
            var textGetter = ReflectionUtility.CreateGetter<Dummy, string>("Text");
            var statusGetter = ReflectionUtility.CreateGetter<Dummy, bool>("Status");
            Assert.AreEqual(dummy.Number, numberGetter(dummy));
            Assert.AreEqual(dummy.Text, textGetter(dummy));
            Assert.AreEqual(dummy.Status, statusGetter(dummy));
        }

        [TestMethod]
        public void ReflectionUtility_CreateSetter_1()
        {
            var dummy = new Dummy();
            var numberSetter = ReflectionUtility.CreateSetter<Dummy, int>("Number");
            var textSetter = ReflectionUtility.CreateSetter<Dummy, string>("Text");
            numberSetter(dummy, 1);
            textSetter(dummy, "Hello");
            Assert.AreEqual(1, dummy.Number);
            Assert.AreEqual("Hello", dummy.Text);
        }

        [TestMethod]
        public void ReflectionUtility_CreateUntypedGetter_1()
        {
            var type = typeof(Dummy);
            var dummy = new Dummy { Number = 1, Text = "Hello" };
            var numberGetter = ReflectionUtility.CreateUntypedGetter(type, nameof(dummy.Number));
            var textGetter = ReflectionUtility.CreateUntypedGetter(type, nameof(dummy.Text));
            var statusGetter = ReflectionUtility.CreateUntypedGetter(type, nameof(dummy.Status));
            Assert.AreEqual(dummy.Number, numberGetter(dummy));
            Assert.AreEqual(dummy.Text, textGetter(dummy));
            Assert.AreEqual(dummy.Status, statusGetter(dummy));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_CreateUntypedGetter_2()
        {
            var type = typeof(Dummy);
            ReflectionUtility.CreateUntypedGetter(type, "NoSuchField");
        }

        [TestMethod]
        public void ReflectionUtility_CreateUntypedSetter_1()
        {
            var type = typeof(Dummy);
            var dummy = new Dummy();
            var numberSetter = ReflectionUtility.CreateUntypedSetter(type, nameof(dummy.Number));
            var textSetter = ReflectionUtility.CreateUntypedSetter(type, nameof(dummy.Text));
            numberSetter(dummy, 1);
            textSetter(dummy, "Hello");
            Assert.AreEqual(1, dummy.Number);
            Assert.AreEqual("Hello", dummy.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_CreateUntypedSetter_2()
        {
            var type = typeof(Dummy);
            ReflectionUtility.CreateUntypedSetter(type, "NoSuchField");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_CreateUntypedSetter_3()
        {
            var type = typeof(Dummy);
            ReflectionUtility.CreateUntypedSetter(type, nameof(Dummy.Status));
        }

    }
}
