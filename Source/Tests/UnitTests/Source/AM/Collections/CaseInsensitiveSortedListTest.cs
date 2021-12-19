// ReSharper disable CheckNamespace

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public sealed class CaseInsensitiveSortedListTest
    {
        [TestMethod]
        public void CaseInsensitiveSortedList_Construction_1()
        {
            var list = new CaseInsensitiveSortedList<int>();
            Assert.AreEqual (0, list.Count);
            list.Add ("first", 1);
            list.Add ("second", 2);
            list.Add ("third", 3);
            Assert.AreEqual (3, list.Count);
            Assert.IsTrue (list.ContainsKey ("first"));
            Assert.IsTrue (list.ContainsKey ("FIRST"));
            Assert.IsTrue (list.ContainsKey ("second"));
            Assert.IsTrue (list.ContainsKey ("SECOND"));
            Assert.IsTrue (list.ContainsKey ("third"));
            Assert.IsTrue (list.ContainsKey ("THIRD"));
            Assert.IsFalse (list.ContainsKey ("fourth"));
            Assert.IsFalse (list.ContainsKey ("FOURTH"));
        }

        [TestMethod]
        public void CaseInsensitiveSortedList_Construction_2()
        {
            var list = new CaseInsensitiveSortedList<int> (100);
            Assert.AreEqual (0, list.Count);
            list.Add ("first", 1);
            list.Add ("second", 2);
            list.Add ("third", 3);
            Assert.AreEqual (3, list.Count);
            Assert.IsTrue (list.ContainsKey ("first"));
            Assert.IsTrue (list.ContainsKey ("FIRST"));
            Assert.IsTrue (list.ContainsKey ("second"));
            Assert.IsTrue (list.ContainsKey ("SECOND"));
            Assert.IsTrue (list.ContainsKey ("third"));
            Assert.IsTrue (list.ContainsKey ("THIRD"));
            Assert.IsFalse (list.ContainsKey ("fourth"));
            Assert.IsFalse (list.ContainsKey ("FOURTH"));
        }

        [TestMethod]
        public void CaseInsensitiveSortedList_Construction_3()
        {
            var first = new CaseInsensitiveSortedList<int>
            {
                ["first"] = 1,
                ["second"] = 2,
                ["third"] = 3,
            };

            var second = new CaseInsensitiveSortedList<int> (first);
            Assert.AreEqual (first.Count, second.Count);
            Assert.IsTrue (second.ContainsKey ("first"));
            Assert.IsTrue (second.ContainsKey ("FIRST"));
            Assert.IsTrue (second.ContainsKey ("second"));
            Assert.IsTrue (second.ContainsKey ("SECOND"));
            Assert.IsTrue (second.ContainsKey ("third"));
            Assert.IsTrue (second.ContainsKey ("THIRD"));
            Assert.IsFalse (second.ContainsKey ("fourth"));
            Assert.IsFalse (second.ContainsKey ("FOURTH"));
        }

        [TestMethod]
        public void CaseInsensitiveSortedList_Search_1()
        {
            var list = new CaseInsensitiveSortedList<int>();
            var found = list.Search ("last");
            Assert.AreEqual (-1, found.Index);
            Assert.AreEqual (string.Empty, found.Value);
        }

        [TestMethod]
        public void CaseInsensitiveSortedList_Search_2()
        {
            var list = new CaseInsensitiveSortedList<int>
            {
                ["first"] = 1,
                ["second"] = 2,
                ["third"] = 3,
            };

            var found = list.Search ("last");
            Assert.AreEqual (0, found.Index);
            Assert.AreEqual ("first", found.Value);

            found = list.Search ("advantage");
            Assert.AreEqual (0, found.Index);
            Assert.AreEqual ("first", found.Value);

            found = list.Search ("first");
            Assert.AreEqual (0, found.Index);
            Assert.AreEqual ("first", found.Value);

            found = list.Search ("second");
            Assert.AreEqual (1, found.Index);
            Assert.AreEqual ("second", found.Value);

            found = list.Search ("third");
            Assert.AreEqual (2, found.Index);
            Assert.AreEqual ("third", found.Value);

            found = list.Search ("zeta");
            Assert.AreEqual (2, found.Index);
            Assert.AreEqual ("third", found.Value);
        }
    }
}
