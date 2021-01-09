using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory;

// ReSharper disable CheckNamespace

#nullable enable

namespace UnitTests.AM.Memory
{
    [TestClass]
    public class MemoryArenaTest
    {
        struct Rect
        {
            public int Left, Top, Width, Height;
        }

        [TestMethod]
        public void MemoryArena_Allocate_1()
        {
            var arena = new MemoryArena(1024);
            var memory = arena.Allocate(10);
            Assert.AreEqual(10, memory.Length);

            memory = arena.Allocate(1);
            Assert.AreEqual(1, memory.Length);
        }

        [TestMethod]
        public void MemoryArena_Allocate_2()
        {
            var arena = new MemoryArena(1024);
            ref Rect rect = ref arena.Allocate<Rect>();
            rect.Left = 10;
            rect.Top = 20;
            rect.Width = 30;
            rect.Height = 40;
            Assert.AreEqual(10, rect.Left);
            Assert.AreEqual(20, rect.Top);
            Assert.AreEqual(30, rect.Width);
            Assert.AreEqual(40, rect.Height);
        }

        [TestMethod]
        public void MemoryArena_Allocate_3()
        {
            using var manager = new UnmanagedMemoryManager<byte>(1024);
            var memory = manager.Memory;
            var arena = new MemoryArena(memory);
            ref Rect rect = ref arena.Allocate<Rect>();
            rect.Left = 10;
            rect.Top = 20;
            rect.Width = 30;
            rect.Height = 40;
            Assert.AreEqual(10, rect.Left);
            Assert.AreEqual(20, rect.Top);
            Assert.AreEqual(30, rect.Width);
            Assert.AreEqual(40, rect.Height);
        }

        [TestMethod]
        public void MemoryArena_Reset_1()
        {
            var arena = new MemoryArena(1024);
            var memory = arena.Allocate(10);
            Assert.AreEqual(10, memory.Length);

            arena.Reset();

            memory = arena.Allocate(1);
            Assert.AreEqual(1, memory.Length);
        }

    }
}