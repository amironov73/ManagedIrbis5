// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using System;
using System.Collections;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class VirtualChildrenTest
    {
        private VirtualChildren _GetChildren()
        {
            var result = new VirtualChildren();
            PftNode[] children =
            {
                new PftComma(),
                new PftBang()
            };
            result.SetChildren(children);

            return result;
        }

        [TestMethod]
        public void VirtualChildren_Construction_1()
        {
            var children = new VirtualChildren();
            Assert.AreEqual(0, children.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Add_1()
        {
            var children = new VirtualChildren();
            children.Add(new PftComma());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Clear_1()
        {
            var children = new VirtualChildren();
            children.Clear();
        }

        [TestMethod]
        public void VirtualChildren_Contains_1()
        {
            var children = new VirtualChildren();
            Assert.IsFalse(children.Contains(new PftNode()));
        }

        [TestMethod]
        public void VirtualChildren_CopyTo_1()
        {
            var children = _GetChildren();
            var array = new PftNode[2];
            children.CopyTo(array, 0);
            Assert.IsInstanceOfType(array[0], typeof(PftComma));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Remove_1()
        {
            var children = new VirtualChildren();
            children.Remove(new PftNode());
        }

        [TestMethod]
        public void VirtualChildren_Count_1()
        {
            var children = _GetChildren();
            Assert.AreEqual(2, children.Count);
        }

        [TestMethod]
        public void VirtualChildren_IsReadOnly_1()
        {
            var children = _GetChildren();
            Assert.IsTrue(children.IsReadOnly);
        }

        [TestMethod]
        public void VirtualChildren_IndexOf_1()
        {
            var children = _GetChildren();
            Assert.AreEqual(-1, children.IndexOf(new PftNode()));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Insert_1()
        {
            var children = new VirtualChildren();
            children.Insert(0, new PftNode());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_RemoveAt_1()
        {
            var children = _GetChildren();
            children.RemoveAt(0);
        }

        [TestMethod]
        public void VirtualChildren_Indexer_1()
        {
            var children = _GetChildren();
            var node = children[0];
            Assert.IsInstanceOfType(node, typeof(PftComma));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Indexer_2()
        {
            var children = _GetChildren();
            children[0] = new PftNode();
        }

        [TestMethod]
        public void VirtualChildren_ToString_1()
        {
            var children = _GetChildren();
            Assert.AreEqual("2", children.ToString());
        }

        [TestMethod]
        public void VirtualChildren_SetChildren_1()
        {
            var children = new VirtualChildren();
            PftNode[] nodes =
            {
                new PftComma()
            };
            children.SetChildren(nodes);
            Assert.AreEqual(1, children.Count);
            var node = children[0];
            Assert.IsInstanceOfType(node, typeof(PftComma));
        }

        [TestMethod]
        public void VirtualChildren_GetEnumerator_1()
        {
            var flag = false;
            var children = _GetChildren();
            children.Enumeration += (_, _) => { flag = true; };
            var enumerator = children.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void VirtualChildren_GetEnumerator_2()
        {
            var flag = false;
            var children = _GetChildren();
            children.Enumeration += (_, _) => { flag = true; };
            var enumerator = ((IEnumerable)children).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsFalse(enumerator.MoveNext());
            Assert.IsTrue(flag);
        }
    }
}
