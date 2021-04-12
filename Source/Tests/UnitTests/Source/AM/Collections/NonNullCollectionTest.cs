﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class NonNullCollectionTest
    {
        [TestMethod]
        public void NonNullCollection_Construction_1()
        {
            var collection = new NonNullCollection<object>();
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_Add_1()
        {
            var collection = new NonNullCollection<object>
                {
                    new object()
                };
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Add_2()
        {
            var collection = new NonNullCollection<object>
            {
                null!
            };
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_AddRange_1()
        {
            var collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    new[]
                    {
                        new object(),
                        new object(),
                        new object()
                    }
                );
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_AddRange_2()
        {
            var collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    (IEnumerable<object>) new[]
                    {
                        new object(),
                        new object(),
                        new object()
                    }
                );
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_AddRange_3()
        {
            var collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    (IList<object>) new[]
                    {
                        new object(),
                        new object(),
                        new object()
                    }
                );
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_AddRange_4()
        {
            var collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    new[]
                    {
                        new object(),
                        null!,
                        new object()
                    }
                );
        }

        [TestMethod]
        public void NonNullCollection_Clear_1()
        {
            var collection
                = new NonNullCollection<object>
                {
                    new object()
                };

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_Indexer_1()
        {
            var collection = new NonNullCollection<object>
                {
                    new object()
                };
            collection[0] = 1;
            Assert.AreEqual(1, collection[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Indexer_2()
        {
            var collection = new NonNullCollection<object>
                {
                    new object()
                };
            Assert.IsNotNull(collection);
            collection[0] = null!;
        }

        [TestMethod]
        public void NonNullCollection_EnsureCapacity_1()
        {
            var collection = new NonNullCollection<object>
            {
                new object()
            };
            collection.EnsureCapacity(100);
            Assert.IsTrue(collection.Capacity >= 100);
        }

        [TestMethod]
        public void NonNullCollection_ToArray_1()
        {
            var collection = new NonNullCollection<string>();
            var array = collection.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NonNullCollection_ToArray_2()
        {
            var collection = new NonNullCollection<string>();
            collection.Add("hello");
            var array = collection.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("hello", array[0]);
        }

        [TestMethod]
        public void NonNullCollection_ToArray_3()
        {
            var collection = new NonNullCollection<string>();
            collection.Add("hello");
            collection.Add("world");
            var array = collection.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual("hello", array[0]);
            Assert.AreEqual("world", array[1]);
        }
    }
}
