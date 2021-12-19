using System;

using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class LocalListTest
    {
        [TestMethod]
        public void LocalList_Construction_1()
        {
            var list = new LocalList<int>();
            try
            {
                Assert.AreEqual (0, list.Count);
                Assert.AreEqual (0, list.Capacity);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Construction_2()
        {
            var expected = 16;
            Span<int> initial = stackalloc int[expected];
            var list = new LocalList<int> (initial);
            try
            {
                Assert.AreEqual (0, list.Count);
                Assert.AreEqual (expected, list.Capacity);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Add_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            Assert.AreEqual (0, list.Count);
            try
            {
                list.Add (1);
                Assert.AreEqual (1, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Add_2()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.AreEqual (0, list.Count);
                list.Add (1);
                list.Add (2);
                Assert.AreEqual (2, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Add_3()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.AreEqual (0, list.Count);
                list.Add (1);
                list.Add (2);
                list.Add (3);
                list.Add (4);
                list.Add (5);
                Assert.AreEqual (5, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_AddRange_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.AreEqual (0, list.Count);
                int[] range = { 1, 2 };
                list.AddRange (range);
                Assert.AreEqual (2, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_GetEnumerator_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                var enumerator = list.GetEnumerator();
                Assert.IsFalse (enumerator.MoveNext());
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_GetEnumerator_2()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                list.Add (3);
                var enumerator = list.GetEnumerator();
                Assert.IsTrue (enumerator.MoveNext());
                Assert.AreEqual (1, enumerator.Current);
                Assert.IsTrue (enumerator.MoveNext());
                Assert.AreEqual (2, enumerator.Current);
                Assert.IsTrue (enumerator.MoveNext());
                Assert.AreEqual (3, enumerator.Current);
                Assert.IsFalse (enumerator.MoveNext());
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Clear_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Clear();
                Assert.AreEqual (0, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Contains_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.IsFalse (list.Contains (1));
                list.Add (1);
                Assert.IsTrue (list.Contains (1));
                Assert.IsFalse (list.Contains (2));
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_CopyTo_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                var array = new int[3];
                list.CopyTo (array, 0);
                Assert.AreEqual (0, array[0]);
                Assert.AreEqual (0, array[1]);
                Assert.AreEqual (0, array[2]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_CopyTo_2()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                list.Add (3);
                var array = new int[3];
                list.CopyTo (array, 0);
                Assert.AreEqual (1, array[0]);
                Assert.AreEqual (2, array[1]);
                Assert.AreEqual (3, array[2]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Remove_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.IsFalse (list.Remove (2));
                list.Add (1);
                list.Add (2);
                list.Add (3);
                Assert.IsTrue (list.Remove (2));
                Assert.IsFalse (list.Remove (2));
                Assert.IsFalse (list.Contains (2));
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Count_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.AreEqual (0, list.Count);
                list.Add (1);
                Assert.AreEqual (1, list.Count);
                list.Add (2);
                Assert.AreEqual (2, list.Count);
                list.Add (3);
                Assert.AreEqual (3, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_IsReadOnly_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.IsFalse (list.IsReadOnly);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_IndexOf_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.IsTrue (list.IndexOf (2) < 0);
                list.Add (1);
                list.Add (2);
                list.Add (3);
                Assert.AreEqual (1, list.IndexOf (2));
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Insert_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            Assert.AreEqual (0, list.Count);
            list.Insert (0, 3);
            Assert.AreEqual (1, list.Count);
            list.Insert (0, 2);
            Assert.AreEqual (2, list.Count);
            list.Insert (0, 1);
            Assert.AreEqual (3, list.Count);
            Assert.AreEqual (2, list[1]);
        }

        [TestMethod]
        public void LocalList_RemoveAt_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                list.Add (3);
                Assert.AreEqual (2, list[1]);
                list.RemoveAt (1);
                Assert.AreEqual (3, list[1]);
                Assert.AreEqual (2, list.Count);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Item_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                list.Add (3);
                Assert.AreEqual (1, list[0]);
                Assert.AreEqual (2, list[1]);
                Assert.AreEqual (3, list[2]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_Item_2()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (0);
                list.Add (0);
                list.Add (0);
                list[0] = 1;
                list[1] = 2;
                list[2] = 3;
                Assert.AreEqual (1, list[0]);
                Assert.AreEqual (2, list[1]);
                Assert.AreEqual (3, list[2]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        [ExpectedException (typeof (IndexOutOfRangeException))]
        public void LocalList_Item_3()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                Assert.AreEqual (0, list[0]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        [ExpectedException (typeof (IndexOutOfRangeException))]
        public void LocalList_Item_4()
        {
            // ReSharper disable UseObjectOrCollectionInitializer
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list[0] = 0;
            }
            finally
            {
                list.Dispose();
            }

            // ReSharper restore UseObjectOrCollectionInitializer
        }

        [TestMethod]
        public void LocalList_ToArray_1()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                var array = list.ToArray();
                Assert.IsNotNull (array);
                Assert.AreEqual (0, array.Length);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_ToArray_2()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                list.Add (3);
                var array = list.ToArray();
                Assert.IsNotNull (array);
                Assert.AreEqual (3, array.Length);
                Assert.AreEqual (1, array[0]);
                Assert.AreEqual (2, array[1]);
                Assert.AreEqual (3, array[2]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_ToArray_3()
        {
            var list = new LocalList<int> (stackalloc int[4]);
            try
            {
                list.Add (1);
                list.Add (2);
                var array = list.ToArray();
                Assert.IsNotNull (array);
                Assert.AreEqual (2, array.Length);
                Assert.AreEqual (1, array[0]);
                Assert.AreEqual (2, array[1]);
            }
            finally
            {
                list.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_ToList_1()
        {
            var local = new LocalList<int> (stackalloc int[4]);
            try
            {
                var list = local.ToList();
                Assert.IsNotNull (list);
                Assert.AreEqual (0, list.Count);
            }
            finally
            {
                local.Dispose();
            }
        }

        [TestMethod]
        public void LocalList_ToList_2()
        {
            var local = new LocalList<int> (stackalloc int[4]);
            try
            {
                local.Add (1);
                local.Add (2);
                local.Add (3);
                var list = local.ToList();
                Assert.IsNotNull (list);
                Assert.AreEqual (3, list.Count);
                Assert.AreEqual (1, list[0]);
                Assert.AreEqual (2, list[1]);
                Assert.AreEqual (3, list[2]);
            }
            finally
            {
                local.Dispose();
            }
        }
    }
}
