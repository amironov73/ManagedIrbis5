﻿using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

namespace UnitTests.AM
{
    [TestClass]
    public class OneOfTest
    {
        [TestMethod]
        public void OneOf2_Constructor_1()
        {
            var one = new OneOf<int, string>(1);
            Assert.AreEqual(one.Value, 1);
            Assert.IsTrue(one.Is1);
            Assert.IsFalse(one.Is2);
        }

        [TestMethod]
        public void OneOf2_Constructor_2()
        {
            var one = new OneOf<int, string>("Hello");
            Assert.AreEqual(one.Value, "Hello");
            Assert.IsFalse(one.Is1);
            Assert.IsTrue(one.Is2);
        }

        [TestMethod]
        public void OneOf2_Value_1()
        {
            var one = new OneOf<int, string>(1);
            Assert.AreEqual(1, one.Value);
        }

        [TestMethod]
        public void OneOf2_Value_2()
        {
            var one = new OneOf<int, string>("Hello");
            Assert.AreEqual("Hello", one.Value);
        }

        [TestMethod]
        public void OneOf2_Try_1()
        {
            var one = new OneOf<int, string>(1);
            Assert.IsTrue(one.Try1(out var intValue));
            Assert.AreEqual(1, intValue);
            Assert.IsFalse(one.Try2(out var _));
        }

        [TestMethod]
        public void OneOf2_Try_2()
        {
            var one = new OneOf<int, string>("Hello");
            Assert.IsTrue(one.Try2(out var strValue));
            Assert.AreEqual("Hello", strValue);
            Assert.IsFalse(one.Try1(out var _));
        }

        [TestMethod]
        public void OneOf2_As_1()
        {
            var one = new OneOf<int, string>(1);
            var value = one.As1();
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf2_As_2()
        {
            var one = new OneOf<int, string>(1);
            one.As2();
        }

        [TestMethod]
        public void OneOf2_Switch_1()
        {
            var flag = false;
            var one = new OneOf<int, string>(1);
            one.Switch(intValue => { flag = true; }, strValue => { flag = false; });
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void OneOf2_Switch_2()
        {
            var flag = true;
            var one = new OneOf<int, string>("Hello");
            one.Switch(intValue => { flag = true; }, strValue => { flag = false; });
            Assert.IsFalse(flag);
        }

        [TestMethod]
        public void OneOf2_Switch_3()
        {
            var one = new OneOf<int, string>("Hello");
            one.Switch(null, null);
        }

        [TestMethod]
        public void OneOf2_Match_1()
        {
            var one = new OneOf<int, string>(1);
            var value = one.Match(intValue => 123, textValue => 321);
            Assert.AreEqual(123, value);
        }

        [TestMethod]
        public void OneOf2_Match_2()
        {
            var one = new OneOf<int, string>("Hello");
            var value = one.Match(intValue => 123, textValue => 321);
            Assert.AreEqual(321, value);
        }

        [TestMethod]
        public void OneOf2_Operator_1()
        {
            OneOf<int, string> one = 1;
            Assert.AreEqual(one.Value, 1);
            Assert.IsTrue(one.Is1);
            Assert.IsFalse(one.Is2);
        }

        [TestMethod]
        public void OneOf2_Operator_2()
        {
            OneOf<int, string> one = "Hello";
            Assert.AreEqual(one.Value, "Hello");
            Assert.IsTrue(one.Is2);
            Assert.IsFalse(one.Is1);
        }

        [TestMethod]
        public void OneOf2_Operator_3()
        {
            var one = new OneOf<int, string>(1);
            int value = one;
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void OneOf2_Operator_4()
        {
            var one = new OneOf<int, string>("Hello");
            string value = one;
            Assert.AreEqual("Hello", value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf2_Operator_5()
        {
            var one = new OneOf<int, string>(1);
            string _ = one;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf2_Operator_6()
        {
            var one = new OneOf<int, string>("Hello");
            int _ = one;
        }

        [TestMethod]
        public void OneOf2_ToString_1()
        {
            var one = new OneOf<int, string>(1);
            Assert.AreEqual("System.Int32: 1", one.ToString());
        }

        [TestMethod]
        public void OneOf2_ToString_2()
        {
            var one = new OneOf<int, string>("Hello");
            Assert.AreEqual("System.String: Hello", one.ToString());
        }

        //=====================================================================

        [TestMethod]
        public void OneOf3_Constructor_1()
        {
            var one = new OneOf<int, string, bool>(1);
            Assert.AreEqual(one.Value, 1);
            Assert.IsTrue(one.Is1);
            Assert.IsFalse(one.Is2);
        }

        [TestMethod]
        public void OneOf3_Constructor_2()
        {
            var one = new OneOf<int, string, bool>("Hello");
            Assert.AreEqual(one.Value, "Hello");
            Assert.IsFalse(one.Is1);
            Assert.IsTrue(one.Is2);
        }

        [TestMethod]
        public void OneOf3_Value_1()
        {
            var one = new OneOf<int, string, bool>(1);
            Assert.AreEqual(1, one.Value);
        }

        [TestMethod]
        public void OneOf3_Value_2()
        {
            var one = new OneOf<int, string, bool>("Hello");
            Assert.AreEqual("Hello", one.Value);
        }

        [TestMethod]
        public void OneOf3_Value_3()
        {
            var one = new OneOf<int, string, bool>(true);
            Assert.AreEqual(true, one.Value);
        }

        [TestMethod]
        public void OneOf3_Try_1()
        {
            var one = new OneOf<int, string, bool>(1);
            Assert.IsTrue(one.Try1(out var intValue));
            Assert.AreEqual(1, intValue);
            Assert.IsFalse(one.Try2(out var _));
            Assert.IsFalse(one.Try3(out var _));
        }

        [TestMethod]
        public void OneOf3_Try_2()
        {
            var one = new OneOf<int, string, bool>("Hello");
            Assert.IsTrue(one.Try2(out var strValue));
            Assert.AreEqual("Hello", strValue);
            Assert.IsFalse(one.Try1(out var _));
            Assert.IsFalse(one.Try3(out var _));
        }

        [TestMethod]
        public void OneOf3_Try_3()
        {
            var one = new OneOf<int, string, bool>(true);
            Assert.IsTrue(one.Try3(out var boolValue));
            Assert.AreEqual(true, boolValue);
            Assert.IsFalse(one.Try1(out var _));
            Assert.IsFalse(one.Try2(out var _));
        }

        [TestMethod]
        public void OneOf3_As_1()
        {
            var one = new OneOf<int, string, bool>(1);
            var value = one.As1();
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf3_As_2()
        {
            var one = new OneOf<int, string, bool>(1);
            one.As2();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf3_As_3()
        {
            var one = new OneOf<int, string, bool>(1);
            one.As3();
        }

        [TestMethod]
        public void OneOf3_Switch_1()
        {
            var flag = false;
            var one = new OneOf<int, string, bool>(1);
            one.Switch(intValue => { flag = true; },
                strValue => { flag = false; },
                boolValue => { flag = false; });
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void OneOf3_Switch_2()
        {
            var flag = true;
            var one = new OneOf<int, string, bool>("Hello");
            one.Switch(intValue => { flag = true; },
                strValue => { flag = false; },
                boolValue => { flag = false; });
            Assert.IsFalse(flag);
        }

        [TestMethod]
        public void OneOf3_Switch_3()
        {
            var flag = true;
            var one = new OneOf<int, string, bool>(true);
            one.Switch(intValue => { flag = true; },
                strValue => { flag = true; },
                boolValue => { flag = false; });
            Assert.IsFalse(flag);
        }

        [TestMethod]
        public void OneOf3_Switch_4()
        {
            var one = new OneOf<int, string, bool>("Hello");
            one.Switch(null, null, null);
        }

        [TestMethod]
        public void OneOf3_Match_1()
        {
            var one = new OneOf<int, string, bool>(1);
            var value = one.Match(intValue => 123,
                textValue => 321,
                boolValue => 213);
            Assert.AreEqual(123, value);
        }

        [TestMethod]
        public void OneOf3_Match_2()
        {
            var one = new OneOf<int, string, bool>("Hello");
            var value = one.Match(intValue => 123,
                textValue => 321,
                boolValue => 213);
            Assert.AreEqual(321, value);
        }

        [TestMethod]
        public void OneOf3_Operator_1()
        {
            OneOf<int, string, bool> one = 1;
            Assert.AreEqual(one.Value, 1);
            Assert.IsTrue(one.Is1);
            Assert.IsFalse(one.Is2);
            Assert.IsFalse(one.Is3);
        }

        [TestMethod]
        public void OneOf3_Operator_2()
        {
            OneOf<int, string, bool> one = "Hello";
            Assert.AreEqual(one.Value, "Hello");
            Assert.IsTrue(one.Is2);
            Assert.IsFalse(one.Is1);
            Assert.IsFalse(one.Is3);
        }

        [TestMethod]
        public void OneOf3_Operator_3()
        {
            OneOf<int, string, bool> one = true;
            Assert.AreEqual(one.Value, true);
            Assert.IsTrue(one.Is3);
            Assert.IsFalse(one.Is1);
            Assert.IsFalse(one.Is2);
        }

        [TestMethod]
        public void OneOf3_Operator_4()
        {
            var one = new OneOf<int, string, bool>(1);
            int value = one;
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void OneOf3_Operator_5()
        {
            var one = new OneOf<int, string, bool>("Hello");
            string value = one;
            Assert.AreEqual("Hello", value);
        }

        [TestMethod]
        public void OneOf3_Operator_6()
        {
            var one = new OneOf<int, string, bool>(true);
            bool value = one;
            Assert.AreEqual(true, value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf3_Operator_7()
        {
            var one = new OneOf<int, string, bool>(1);
            string _ = one;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf3_Operator_8()
        {
            var one = new OneOf<int, string, bool>("Hello");
            int _ = one;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void OneOf3_Operator_9()
        {
            var one = new OneOf<int, string, bool>(true);
            int _ = one;
        }

        [TestMethod]
        public void OneOf3_ToString_1()
        {
            var one = new OneOf<int, string, bool>(1);
            Assert.AreEqual("System.Int32: 1", one.ToString());
        }

        [TestMethod]
        public void OneOf3_ToString_2()
        {
            var one = new OneOf<int, string, bool>("Hello");
            Assert.AreEqual("System.String: Hello", one.ToString());
        }

        [TestMethod]
        public void OneOf3_ToString_3()
        {
            var one = new OneOf<int, string, bool>(true);
            Assert.AreEqual("System.Boolean: True", one.ToString());
        }
    }
}
