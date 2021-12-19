// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;
#nullable enable
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class VirtualListTest
    {
        private Action<VirtualList<int>.Parameters> _GetRetriever()
        {
            Action<VirtualList<int>.Parameters> result = parameters =>
            {
                var array = new int[10];
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = 100 + i;
                }

                parameters.List.SetCache (array, 0);
            };

            return result;
        }

        private VirtualList<int> _GetList()
        {
            var result = new VirtualList<int> (_GetRetriever(), 10, 10);

            return result;
        }

        private VirtualList<int> _GetList2()
        {
            Action<VirtualList<int>.Parameters> retriever = _ => { };
            var result = new VirtualList<int> (retriever, 10, 10);

            return result;
        }

        [TestMethod]
        public void VirtualList_Construction_1()
        {
            var list = _GetList();
            Assert.AreEqual (10, list.CacheSize);
            Assert.IsTrue (list.IsReadOnly);
        }

        [TestMethod]
        public void VirtualList_Construction_2()
        {
            var list = new VirtualList<int> (_GetRetriever(), 10, 20);
            Assert.IsTrue (list.IsReadOnly);
        }

        [TestMethod]
        public void VirtualList_GetItem_1()
        {
            var list = _GetList();
            Assert.AreEqual (100, list[0]);
            Assert.AreEqual (101, list[1]);
            Assert.AreEqual (102, list[2]);
            Assert.AreEqual (109, list[9]);
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_Item_1()
        {
            var list = _GetList();
            list[2] = 202;
        }

        [TestMethod]
        public void VirtualList_Contains_1()
        {
            var list = _GetList();
            Assert.IsTrue (list.Contains (102));
            Assert.IsFalse (list.Contains (202));
        }

        [TestMethod]
        public void VirtualList_Contains_2()
        {
            var list = _GetList2();
            Assert.IsFalse (list.Contains (102));
            Assert.IsFalse (list.Contains (202));
        }

        [TestMethod]
        public void VirtualList_GetEnumerator_1()
        {
            var list = _GetList();
            var array = list.ToArray();
            Assert.AreEqual (10, array.Length);
        }

        [TestMethod]
        public void VirtualList_GetEnumerator_2()
        {
            IList<int> list = _GetList();
            var array = list.ToArray();
            Assert.AreEqual (10, array.Length);
        }

        [TestMethod]
        public void VirtualList_IndexOf_1()
        {
            var list = _GetList();
            Assert.AreEqual (2, list.IndexOf (102));
            Assert.AreEqual (-1, list.IndexOf (1002));
        }

        [TestMethod]
        public void VirtualList_IndexOf_2()
        {
            var list = _GetList2();
            Assert.AreEqual (-1, list.IndexOf (102));
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_Add_1()
        {
            IList<int> list = _GetList();
            list.Add (111);
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_Clear_1()
        {
            IList<int> list = _GetList();
            list.Clear();
        }

        [TestMethod]
        public void VirtualList_CopyTo_1()
        {
            IList<int> list = _GetList();
            var array = new int[10];
            list.CopyTo (array, 0);
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_Remove_1()
        {
            IList<int> list = _GetList();
            list.Remove (111);
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_RemoveAt_1()
        {
            IList<int> list = _GetList();
            list.RemoveAt (1);
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        public void VirtualList_Insert_1()
        {
            IList<int> list = _GetList();
            list.Insert (1, 111);
        }

        [TestMethod]
        public void VirtualList_SetCache_1()
        {
            Action<VirtualList<int>.Parameters> retriever = parameters =>
            {
                var array = new int[20];
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = 100 + i;
                }

                parameters.List.SetCache (array, 0);
            };
            var list = new VirtualList<int> (retriever, 10, 10);
            Assert.IsTrue (list.Contains (102));
            Assert.IsFalse (list.Contains (202));
        }

        [TestMethod]
        public void VirtualList_Parameters_1()
        {
            var list = _GetList();
            var parameters
                = new VirtualList<int>.Parameters (list, 10, true);
            Assert.AreSame (list, parameters.List);
            Assert.AreEqual (10, parameters.Index);
            Assert.IsTrue (parameters.Up);
        }
    }
}
