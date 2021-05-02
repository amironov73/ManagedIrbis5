// ReSharper disable CheckNamespace

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DisposableCollectionTest
    {
        private static int _count;

        class Dummy
            : IDisposable
        {
            public void Dispose()
            {
                _count++;
            }
        }

        [TestMethod]
        public void DisposableCollection_Dispose()
        {
            _count = 0;

            var collection = new DisposableCollection<Dummy>
            {
                new (),
                new (),
                new ()
            };

            collection.Dispose();

            Assert.AreEqual(3, _count);
        }
    }
}
