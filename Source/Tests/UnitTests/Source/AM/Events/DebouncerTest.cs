// ReSharper disable CheckNamespace
// ReSharper disable EventNeverSubscribedTo.Local
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Events;

#nullable enable

namespace UnitTests.AM.Events;

[TestClass]
public sealed class DebouncerTest
{
    class DebouncingSample
    {
        public static bool DebounceDuring100MsStatic()
        {
            return Debouncer.DebounceHereStatic<DebouncingSample> (100);
        }

        public bool DebounceDuring100Ms()
        {
            return this.DebounceHere (100);
        }
    }

    [TestMethod]
    public void Debouncer_DebounceHere_1()
    {
        WeakReference? sampleRef = null;
        WeakReference? sampleRef2 = null;

        new Action (() =>
        {
            var sample = new DebouncingSample();
            var sample2 = new DebouncingSample();

            sampleRef = new WeakReference (sample);
            sampleRef2 = new WeakReference (sample2);

            sample.DebounceHere();
            sample2.DebounceHere();
        }).Invoke();

        GC.Collect();

        Assert.IsFalse (sampleRef!.IsAlive);
        Assert.IsFalse (sampleRef2!.IsAlive);
    }

    [TestMethod]
    public void Debouncer_DebounceHere_2()
    {
        var sample = new DebouncingSample();

        Assert.IsFalse (LocalDebounce (sample));
        Assert.IsTrue (LocalDebounce (sample));

        static bool LocalDebounce (DebouncingSample sample) => sample.DebounceHere();
    }

    [TestMethod]
    public void Debouncer_DebounceHere_3()
    {
        var sample = new DebouncingSample();

        Assert.IsFalse (DebounceLocation1 (sample));
        Assert.IsFalse (DebounceLocation2 (sample));

        Assert.IsTrue (DebounceLocation1 (sample));
        Assert.IsTrue (DebounceLocation2 (sample));

        static bool DebounceLocation1 (DebouncingSample sample) => sample.DebounceHere();
        static bool DebounceLocation2 (DebouncingSample sample) => sample.DebounceHere();
    }

    [TestMethod]
    public void Debouncer_DebounceHere_4()
    {
        var sample = new DebouncingSample();
        var sample2 = new DebouncingSample();

        Assert.IsFalse (SameLocationDebounce (sample));
        Assert.IsFalse (SameLocationDebounce (sample2));
        Assert.IsTrue (SameLocationDebounce (sample));
        Assert.IsTrue (SameLocationDebounce (sample2));

        static bool SameLocationDebounce (DebouncingSample sample) => sample.DebounceHere();
    }

    [Ignore]
    [TestMethod]
    public void Debouncer_DebounceHere_5()
    {
        var testOk = true;

        var thread1 = new Thread (Test);
        var thread2 = new Thread (Test);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Assert.IsTrue (testOk);

        void Test()
        {
            for (int i = 0; i < 100000; i++)
            {
                var sample = new DebouncingSample();
                var sample2 = new DebouncingSample();

                testOk &= !sample.DebounceHere();
                testOk &= !sample2.DebounceHere();

                testOk &= !SameLocationDebounce (sample);
                testOk &= !SameLocationDebounce (sample2);

                testOk &= SameLocationDebounce (sample);
                testOk &= SameLocationDebounce (sample2);
            }
        }

        static bool SameLocationDebounce (DebouncingSample sample) => sample.DebounceHere();
    }
}
