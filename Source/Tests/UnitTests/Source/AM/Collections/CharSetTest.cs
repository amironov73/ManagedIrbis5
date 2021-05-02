// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable EqualExpressionComparison
// ReSharper disable StringLiteralTypo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM.Collections;
using AM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class CharSetTest
    {
        [TestMethod]
        public void CharSet_Construction_1()
        {
            var charSet = new CharSet();

            Assert.AreEqual(0, charSet.Count);
            Assert.IsFalse(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Construction_2()
        {
            var charSet = new CharSet("abc");

            Assert.AreEqual(3, charSet.Count);
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Construction_3()
        {
            var bitArray = new BitArray(CharSet.DefaultCapacity)
            {
                [97] = true,
                [98] = true,
                [99] = true
            };
            var charSet = new CharSet(bitArray);
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Construction_4()
        {
            var list = new List<char> { 'a', 'b', 'c' };
            var charSet = new CharSet(list);
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Construction_5()
        {
            var charSet = new CharSet('a', 'b', 'c');
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Add_1()
        {
            var charSet = new CharSet();
            charSet.Add('a').Add('b').Add('c');
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Add_2()
        {
            var charSet = new CharSet {"\\n"};
            Assert.IsFalse(charSet.Contains('a'));
            Assert.IsFalse(charSet.Contains('\n'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CharSet_Add_2a()
        {
            var charSet = new CharSet {"a\\"};
        }

        [TestMethod]
        public void CharSet_Add_3()
        {
            var charSet = new CharSet {"a-c"};
            Assert.IsTrue(charSet.Contains('a'));
            Assert.IsTrue(charSet.Contains('b'));
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CharSet_Add_3a()
        {
            var charSet = new CharSet {"a-"};
        }

        [TestMethod]
        public void CharSet_AddRange_1()
        {
            var charSet = new CharSet();

            charSet.AddRange('a', 'z');

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('C'));
        }

        [TestMethod]
        public void CharSet_Remove_1()
        {
            var charSet = new CharSet("abcdef");
            charSet.Remove('c').Remove('d');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Remove_2()
        {
            var charSet = new CharSet("abcdef");
            charSet.Remove('c', 'd');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Duplicates_1()
        {
            var charSet = new CharSet("abcabc");

            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Enumeration_1()
        {
            var charSet = new CharSet("abcdef");
            var builder = new StringBuilder();

            foreach (var c in charSet)
            {
                builder.Append(c);
            }

            Assert.AreEqual("abcdef", builder.ToString());
        }

        [TestMethod]
        public void CharSet_Enumeration_2()
        {
            var charSet = new CharSet("abc");
            var enumerator = ((IEnumerable) charSet).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('a', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('b', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('c', enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            enumerator.Reset();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('a', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('b', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('c', enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        // [TestMethod]
        // public void CharSet_JsonSerialization_1()
        // {
        //     var charSet = new CharSet("abcdef");
        //     var actual = JsonConvert.SerializeObject
        //         (
        //             charSet,
        //             Formatting.Indented,
        //             new CharSet.CharSetConverter(typeof(CharSet))
        //         ).Replace("\r", "").Replace("\n", "");
        //     const string expected = "{  \"charset\": \"abcdef\"}";
        //
        //     Assert.AreEqual(expected, actual);
        // }

        [TestMethod]
        public void CharSet_CheckText_1()
        {
            var charSet = new CharSet("abc");

            Assert.IsTrue(charSet.CheckText("aabcc"));
            Assert.IsFalse(charSet.CheckText("abdcc"));
            Assert.IsTrue(charSet.CheckText(string.Empty));
            Assert.IsTrue(charSet.CheckText(null));
        }

        [TestMethod]
        public void CharSet_ToArray_1()
        {
            var charSet = new CharSet("abc");
            var array = charSet.ToArray();

            Assert.AreEqual(3, array.Length);
            Assert.IsTrue(array.Contains('a'));
            Assert.IsTrue(array.Contains('b'));
            Assert.IsTrue(array.Contains('c'));
        }

        [TestMethod]
        public void CharSet_Equality_1()
        {
            var first = new CharSet("abc");
            var second = new CharSet("def");
            var third = new CharSet("abc");

            Assert.IsFalse(first.Equals(second));
            Assert.IsTrue(first.Equals(third));
            Assert.IsFalse(second.Equals(third));
        }

        private void _TestSerialization
            (
                CharSet first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes
                .RestoreObjectFromMemory<CharSet>();

            Assert.IsTrue
                (
                    first.Equals(second)
                );
        }

        [TestMethod]
        public void CharSet_Serialization_1()
        {
            var charSet = new CharSet();
            _TestSerialization(charSet);

            charSet.AddRange('a', 'z');
            _TestSerialization(charSet);
        }

        [TestMethod]
        public void CharSet_Item_1()
        {
            var charSet = new CharSet("abc");
            Assert.IsTrue(charSet['a']);
            Assert.IsTrue(charSet['b']);
            Assert.IsTrue(charSet['c']);
            Assert.IsFalse(charSet['d']);
        }

        [TestMethod]
        public void CharSet_Item_2()
        {
            var charSet = new CharSet("abc")
            {
                ['a'] = false,
                ['d'] = true
            };
            Assert.IsFalse(charSet['a']);
            Assert.IsTrue(charSet['b']);
            Assert.IsTrue(charSet['c']);
            Assert.IsTrue(charSet['d']);
        }

        [TestMethod]
        public void CharSet_Equals_1()
        {
            var first = new CharSet("abc");
            object? second = new CharSet("abc");
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second = new CharSet("bcd");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));

            second = 123;
            Assert.IsFalse(first.Equals(second));

            second = "abc";
            Assert.IsTrue(first.Equals(second));

            second = "bcd";
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void CharSet_GetHashCode_1()
        {
            var charSet = new CharSet();
            Assert.AreEqual(0, charSet.GetHashCode());

            charSet.Add('a');
            Assert.AreEqual(98, charSet.GetHashCode());

            charSet.Add('b');
            Assert.AreEqual(1765, charSet.GetHashCode());

            charSet.Add('c');
            Assert.AreEqual(30105, charSet.GetHashCode());
        }

        [TestMethod]
        public void CharSet_Addition_1()
        {
            var left = new CharSet("abc");
            var right = new CharSet("bcd");
            var result = left + right;
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Addition_2()
        {
            var left = new CharSet("abc");
            var result = left + "bcd";
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Addition_3()
        {
            var left = new CharSet("abc");
            var result = left + 'd';
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Multiply_1()
        {
            var left = new CharSet("abc");
            var right = new CharSet("bcd");
            var result = left * right;
            Assert.AreEqual("bc", result.ToString());
        }

        [TestMethod]
        public void CharSet_Multiply_2()
        {
            var left = new CharSet("abc");
            var result = left * "bcd";
            Assert.AreEqual("bc", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_1()
        {
            var left = new CharSet("abc");
            var right = new CharSet("bcd");
            var result = left - right;
            Assert.AreEqual("a", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_2()
        {
            var left = new CharSet("abc");
            var result = left - "bcd";
            Assert.AreEqual("a", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_3()
        {
            var left = new CharSet("abc");
            var result = left - 'c';
            Assert.AreEqual("ab", result.ToString());
        }

        [TestMethod]
        public void CharSet_RemoveRange_1()
        {
            var charSet = new CharSet("abcdefg");
            charSet.RemoveRange('c', 'e');
            Assert.AreEqual("abfg", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Xor_1()
        {
            var left = new CharSet("abc");
            var right = new CharSet("cde");
            var result = left.Xor(right);
            Assert.AreEqual("abde", result.ToString());
        }
    }
}

