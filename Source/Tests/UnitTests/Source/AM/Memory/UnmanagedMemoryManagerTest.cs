// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory;

#endregion

#nullable enable

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class UnmanagedMemoryManagerTest
{
    private const int ItemCount = 100;
    private const int ItemSize = sizeof (int);
    private const int ByteCount = ItemCount * ItemSize;

    [TestMethod]
    [Description ("Создание менеджера из диапазона")]
    public unsafe void UnmanagedMemoryManager_Construction_1()
    {
        var block = NativeMemory.Alloc ((nuint) ByteCount);
        var span = new Span<int> (block, ItemCount);
        using var manager = new UnmanagedMemoryManager<int> (span);

        Assert.AreEqual (ItemCount, manager.Memory.Length);
    }

    [TestMethod]
    [Description ("Создание менеджера из указателя")]
    public unsafe void UnmanagedMemoryManager_Construction_2()
    {
        var block = NativeMemory.Alloc ((nuint) ByteCount);
        using var manager = new UnmanagedMemoryManager<int> ((int*) block, ItemCount);

        Assert.AreEqual (ItemCount, manager.Memory.Length);
    }

    [TestMethod]
    [Description ("Создание менеджера из дескриптора")]
    public void UnmanagedMemoryManager_Construction_3()
    {
        var handle = Marshal.AllocHGlobal (ByteCount);
        using var manager = new UnmanagedMemoryManager<int> (handle, ItemCount);

        Assert.AreEqual (ItemCount, manager.Memory.Length);

        // Marshal.FreeHGlobal вызывать не нужно!
    }

    [TestMethod]
    [Description ("Создание менеджера из Marshal")]
    public void UnmanagedMemoryManager_Construction_4()
    {
        using var manager = new UnmanagedMemoryManager<int> (ItemCount);

        Assert.AreEqual (ItemCount, manager.Memory.Length);
    }

    [TestMethod]
    [Description ("Получение диапазона")]
    public unsafe void UnmanagedMemoryManager_GetSpan_1()
    {
        var block = NativeMemory.Alloc ((nuint) ByteCount);
        var span = new Span<int> (block, ItemCount);
        using var manager = new UnmanagedMemoryManager<int> (span);

        var actual = manager.GetSpan();
        Assert.AreEqual (span.Length, actual.Length);
    }

    [TestMethod]
    [Description ("Пришпиливание указателя")]
    public unsafe void UnmanagedMemoryManager_Pin_1()
    {
        var block = (int*) NativeMemory.Alloc ((nuint) ByteCount);
        var span = new Span<int> (block, ItemCount);
        using var manager = new UnmanagedMemoryManager<int> (span);

        var actual = manager.Pin();
        Assert.IsTrue (block == actual.Pointer);

        manager.Unpin();
    }
}
