// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

#nullable enable

namespace UnitTests.AM.Globalization
{
    [TestClass]
    public class RussianStringComparerTest
    {
        private void _TestCompare1
            (
                int expected,
                string str1,
                string str2
            )
        {
            var comparer = new RussianStringComparer(true);

            var actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Compare1()
        {
            _TestCompare1(0, "ежик", "ёжик");
            _TestCompare1(-1, "ежик", "ножик");
        }

        private void _TestCompare2
            (
                int expected,
                string str1,
                string str2
            )
        {
            var comparer = new RussianStringComparer(true, true);

            var actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Compare2()
        {
            _TestCompare2(0, "ежик", "ЁЖИК");
            _TestCompare2(-1, "ежик", "НОЖИК");
        }

        private void _TestEquals1
            (
                bool expected,
                string str1,
                string str2
            )
        {
            var comparer = new RussianStringComparer(true);

            var actual = comparer.Equals(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Equals1()
        {
            _TestEquals1(true, "ежик", "ёжик");
            _TestEquals1(false, "ежик", "ножик");
        }

        private void _TestEquals2
            (
                bool expected,
                string str1,
                string str2
            )
        {
            var comparer = new RussianStringComparer(true, true);

            var actual = comparer.Equals(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Equals2()
        {
            _TestEquals2(true, "ежик", "ЁЖИК");
            _TestEquals2(false, "ежик", "НОЖИК");
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode1()
        {
            var comparer = new RussianStringComparer();

            Assert.AreEqual(0, comparer.GetHashCode(null!));

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode2()
        {
            var comparer = new RussianStringComparer(false, true);

            Assert.AreEqual(0, comparer.GetHashCode(null!));

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode3()
        {
            var comparer = new RussianStringComparer(true);

            Assert.AreEqual(0, comparer.GetHashCode(null!));

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode4()
        {
            var comparer = new RussianStringComparer(true, true);

            Assert.AreEqual(0, comparer.GetHashCode(null!));

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }
    }
}
