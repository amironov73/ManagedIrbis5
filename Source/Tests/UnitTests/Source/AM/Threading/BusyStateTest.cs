// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Threading;

#nullable enable

namespace UnitTests.AM.Threading;

[TestClass]
public sealed class BusyStateTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BusyState_Construction_1()
    {
        using var state = new BusyState();
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Отработка события")]
    public void BusyState_Event_1()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.StateChanged += StateChangedHandler;
        state.SetState (false);
        Assert.IsTrue (flag);
        Assert.IsFalse (state);
        state.StateChanged -= StateChangedHandler;
    }

    [TestMethod]
    [Description ("Отработка события")]
    public void BusyState_Event_2()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = false;
        state.StateChanged += StateChangedHandler;
        state.SetState (true);
        Assert.IsTrue (flag);
        Assert.IsTrue (state);
        state.StateChanged -= StateChangedHandler;
    }

    [TestMethod]
    [Description ("Установка состояния занятости")]
    public void BusyState_SetState_1()
    {
        using BusyState state = false;
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Установка состояния занятости")]
    public void BusyState_SetState_2()
    {
        using BusyState state = true;
        Assert.IsTrue (state);
    }

    [TestMethod]
    [Description ("Установка состояния занятости")]
    public void BusyState_SetState_3()
    {
        using BusyState state = true;
        var flag = false;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    state.Wait();
                    flag = true;
                }
            );
        state.SetState (false);
        task.Wait();

        Assert.IsTrue (flag);
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Установка состояния занятости")]
    public void BusyState_SetState_4()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.UseAsync = true;
        state.StateChanged += StateChangedHandler;
        state.SetState (false);
        state.StateChanged -= StateChangedHandler;
        Thread.Sleep (10);

        Assert.IsTrue (flag);
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Получение WaitHandle")]
    public void BusyState_WaitHandle_1()
    {
        using BusyState state = true;
        var flag = false;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    state.SetState (false);
                    flag = true;
                });
        state.WaitHandle.WaitOne();
        task.Wait();

        Assert.IsTrue (flag);
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Синхронный запуск кода с состоянием занятости")]
    public void BusyState_Run_1()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using var state = new BusyState();
        state.StateChanged += StateChangedHandler;
        state.Run (() => Thread.Sleep (10));
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Асинхронный запуск кода с состоянием занятости")]
    public void BusyState_RunAsync_1()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using var state = new BusyState();
        state.StateChanged += StateChangedHandler;
        var task = state.RunAsync (() => Thread.Sleep (10));
        task.Wait();
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsFalse (state);
    }

    private static void _TestSerialization
        (
            BusyState first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<BusyState>()!;

        Assert.IsNotNull (second);
        Assert.AreEqual (first.State, second.State);
        Assert.AreEqual (first.UseAsync, second.UseAsync);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void BusyState_Serialization_1()
    {
        using var state = new BusyState();
        _TestSerialization (state);

        state.SetState (!state.State);
        _TestSerialization (state);

        state.SetState (!state.State);
        _TestSerialization (state);
    }

    [TestMethod]
    [Description ("Ожидание с последующим захватом")]
    public void BusyState_WaitAndGrab_1()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.StateChanged += StateChangedHandler;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    Thread.Sleep (10);
                    state.SetState (false);
                });
        task.Wait();
        state.WaitAndGrab();
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsTrue (state);
    }

    [TestMethod]
    [Description ("Ожидание с последующим захватом")]
    public void BusyState_WaitAndGrab_2()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.StateChanged += StateChangedHandler;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    Thread.Sleep (100);
                    state.SetState (false);
                }
            );
        state.WaitAndGrab();
        task.Wait();
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsTrue (state);
    }

    [TestMethod]
    [Description ("Ожидание с последующим захватом")]
    public void BusyState_WaitAndGrab_3()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.StateChanged += StateChangedHandler;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    Thread.Sleep (10);
                    state.SetState (false);
                });
        task.Wait();
        Assert.IsTrue (state.WaitAndGrab (TimeSpan.FromMilliseconds (100)));
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsTrue (state);
    }

    [TestMethod]
    [Description ("Ожидание с последующим захватом")]
    public void BusyState_WaitAndGrab_4()
    {
        var flag = false;

        void StateChangedHandler (object? _, EventArgs __)
        {
            flag = true;
        }

        using BusyState state = true;
        state.StateChanged += StateChangedHandler;
        var task = Task.Factory.StartNew
            (
                () =>
                {
                    Thread.Sleep (100);
                    state.SetState (false);
                }
            );
        Assert.IsTrue (state.WaitAndGrab (TimeSpan.FromMilliseconds (200)));
        task.Wait();
        state.StateChanged -= StateChangedHandler;

        Assert.IsTrue (flag);
        Assert.IsTrue (state);
    }

    [TestMethod]
    [Description ("Ожидание освобождения")]
    public void BusyState_Wait_1()
    {
        using BusyState state = false;
        state.Wait();
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Ожидание освобождения")]
    public void BusyState_Wait_2()
    {
        using BusyState state = false;
        Assert.IsTrue (state.Wait (TimeSpan.FromMilliseconds (100)));
        Assert.IsFalse (state);
    }

    [TestMethod]
    [Description ("Преобразование в текст")]
    public void BusyState_ToString_1()
    {
        using BusyState state = false;
        Assert.AreEqual (false.ToString(), state.ToString());
    }

    [TestMethod]
    [Description ("Преобразование в текст")]
    public void BusyState_ToString_2()
    {
        using BusyState state = true;
        Assert.AreEqual (true.ToString(), state.ToString());
    }
}
