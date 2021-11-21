// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

#nullable enable

namespace UnitTests.AM.Reflection
{
    // класс для проверки интроспекции
    class Dummy
    {
        #region Константы

        public const string Initial = "Начальное";
        public const string Intermediate = "Промежуточное";
        public const string Final = "Окончательное";

        #endregion

        #region Не-константы, а обычные поля

        public static string Hello = "Привет";
        public static readonly string Goodbye = "Прощай";

        #endregion

        #region Свойства

        public int Number { get; set; }
        public string? Text { get; set; }

        public bool Status => true;

        #endregion
    }

    [TestClass]
    public sealed class ReflectionUtilityTest
    {
        [TestMethod]
        public void ReflectionUtility_CreateGetter_1()
        {
            var dummy = new Dummy { Number = 1, Text = "Hello" };
            var numberGetter = ReflectionUtility.CreateGetter<Dummy, int> ("Number");
            var textGetter = ReflectionUtility.CreateGetter<Dummy, string> ("Text");
            var statusGetter = ReflectionUtility.CreateGetter<Dummy, bool> ("Status");
            Assert.AreEqual (dummy.Number, numberGetter (dummy));
            Assert.AreEqual (dummy.Text, textGetter (dummy));
            Assert.AreEqual (dummy.Status, statusGetter (dummy));
        }

        [TestMethod]
        public void ReflectionUtility_CreateSetter_1()
        {
            var dummy = new Dummy();
            var numberSetter = ReflectionUtility.CreateSetter<Dummy, int> ("Number");
            var textSetter = ReflectionUtility.CreateSetter<Dummy, string> ("Text");
            numberSetter (dummy, 1);
            textSetter (dummy, "Hello");
            Assert.AreEqual (1, dummy.Number);
            Assert.AreEqual ("Hello", dummy.Text);
        }

        [TestMethod]
        public void ReflectionUtility_CreateUntypedGetter_1()
        {
            var type = typeof (Dummy);
            var dummy = new Dummy { Number = 1, Text = "Hello" };
            var numberGetter = ReflectionUtility.CreateUntypedGetter (type, nameof (dummy.Number));
            var textGetter = ReflectionUtility.CreateUntypedGetter (type, nameof (dummy.Text));
            var statusGetter = ReflectionUtility.CreateUntypedGetter (type, nameof (dummy.Status));
            Assert.AreEqual (dummy.Number, numberGetter (dummy));
            Assert.AreEqual (dummy.Text, textGetter (dummy));
            Assert.AreEqual (dummy.Status, statusGetter (dummy));
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void ReflectionUtility_CreateUntypedGetter_2()
        {
            var type = typeof (Dummy);
            ReflectionUtility.CreateUntypedGetter (type, "NoSuchField");
        }

        [TestMethod]
        public void ReflectionUtility_CreateUntypedSetter_1()
        {
            var type = typeof (Dummy);
            var dummy = new Dummy();
            var numberSetter = ReflectionUtility.CreateUntypedSetter (type, nameof (dummy.Number));
            var textSetter = ReflectionUtility.CreateUntypedSetter (type, nameof (dummy.Text));
            numberSetter (dummy, 1);
            textSetter (dummy, "Hello");
            Assert.AreEqual (1, dummy.Number);
            Assert.AreEqual ("Hello", dummy.Text);
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void ReflectionUtility_CreateUntypedSetter_2()
        {
            var type = typeof (Dummy);
            ReflectionUtility.CreateUntypedSetter (type, "NoSuchField");
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void ReflectionUtility_CreateUntypedSetter_3()
        {
            var type = typeof (Dummy);
            ReflectionUtility.CreateUntypedSetter (type, nameof (Dummy.Status));
        }

        [TestMethod]
        [Description ("Получение массива имен констант")]
        public void ReflectionUtility_ListConstantNames_1()
        {
            var names = ReflectionUtility.ListConstantNames (typeof (Dummy));
            Assert.AreEqual (3, names.Length);
            // интроспекция может перечислить константы в произвольном порядке
            Array.Sort (names);
            Assert.AreEqual ("Final", names[0]);
            Assert.AreEqual ("Initial", names[1]);
            Assert.AreEqual ("Intermediate", names[2]);
        }

        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void ReflectionUtility_ListConstantValues_1()
        {
            var values = ReflectionUtility.ListConstantValues<string> (typeof (Dummy));
            Assert.AreEqual (3, values.Length);
            // интроспекция может перечислить константы в произвольном порядке
            Array.Sort (values);
            Assert.AreEqual ("Начальное", values[0]);
            Assert.AreEqual ("Окончательное", values[1]);
            Assert.AreEqual ("Промежуточное", values[2]);
        }

        [TestMethod]
        [Description ("Получение массива констант")]
        public void ReflectionUtility_ListConstants_1()
        {
            var constants = ReflectionUtility.ListConstants<string> (typeof (Dummy));
            Assert.AreEqual (3, constants.Length);
            // интроспекция может перечислить константы в произвольном порядке
            constants = constants.OrderBy (one => one.Name).ToArray();
            Assert.AreEqual ("Final", constants[0].Name);
            Assert.AreEqual ("Окончательное", constants[0].Value);
            Assert.AreEqual ("Initial", constants[1].Name);
            Assert.AreEqual ("Начальное", constants[1].Value);
            Assert.AreEqual ("Intermediate", constants[2].Name);
            Assert.AreEqual ("Промежуточное", constants[2].Value);
        }
    }
}
