// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable ConvertToLocalFunction

#region Using directives

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading.Tasks;

#endregion

#nullable enable

namespace UnitTests.AM.Threading.Tasks;

[TestClass]
public sealed class SimplestTaskProcessorTest
{
    [Ignore]
    [TestMethod]
    public void SimplestTaskProcessor_1()
    {
        var lines = new List<string>();

        var processor = new SimplestTaskProcessor (1);
        for (var i = 0; i < 10; i++)
        {
            var number = i;

            var action = () =>
            {
                var item = "Hello " + number;
                lock (lines)
                {
                    lines.Add (item);
                }
            };

            processor.Enqueue (action);
        }

        processor.Complete();
        processor.WaitForCompletion();

        Assert.AreEqual (10, lines.Count);
        Assert.IsFalse (processor.HaveErrors);
        Assert.AreEqual (0, processor.Exceptions.Count);
    }

    [Ignore]
    [TestMethod]
    public void SimplestTaskProcessor_2()
    {
        var lines = new List<string>();

        var processor = new SimplestTaskProcessor (1);
        for (var i = 0; i < 10; i++)
        {
            var number = i;

            var action = () =>
            {
                var item = "Hello " + number;
                lock (lines)
                {
                    lines.Add (item);
                }

                throw new Exception (item);
            };

            processor.Enqueue (action);
        }

        processor.Complete();
        processor.WaitForCompletion();

        Assert.AreEqual (10, lines.Count);
        Assert.IsTrue (processor.HaveErrors);
        Assert.AreEqual (10, processor.Exceptions.Count);
    }

    [TestMethod]
    [Description ("Задание невозможной степени параллелизма")]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void SimplestTaskProcessor_Construction_1()
    {
        _ = new SimplestTaskProcessor (-1);
    }
}
