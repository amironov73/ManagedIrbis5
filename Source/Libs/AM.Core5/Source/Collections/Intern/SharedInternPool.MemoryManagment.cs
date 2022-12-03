// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SharedInternPool.MemoryManagement.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;

#endregion

#nullable enable

namespace AM.Collections.Intern;

public partial class SharedInternPool
    : IInternPool
{
    private class InternPoolCleaner
#if NET5_0 || NETCOREAPP3_1
            : IThreadPoolWorkItem
#endif
    {
        private SharedInternPool _pool;
        private int _isTrimming = 0;
        private long _collections = 0;

        public long Collections => Volatile.Read (ref _collections);

        public InternPoolCleaner (SharedInternPool pool)
        {
            _pool = pool;
        }

        public void Execute()
        {
            if (Interlocked.Exchange (ref _isTrimming, 1) == 1)
                return;

            _pool.Trim ((InternPool.TrimLevel)((int)_collections % (int)InternPool.TrimLevel.Max));

            Interlocked.Increment (ref _collections);

            Volatile.Write (ref _isTrimming, 0);
        }
    }

    internal long Collections => _cleaner?.Collections ?? 0;

    private InternPoolCleaner? _cleaner;

    private bool EnqueueTrim()
    {
        var cleaner = _cleaner;
        if (cleaner == null)
        {
            cleaner = new InternPoolCleaner (this);
            cleaner = Interlocked.CompareExchange (ref _cleaner, cleaner, null) ?? cleaner;
        }

#if NET5_0 || NETCOREAPP3_1
            ThreadPool.UnsafeQueueUserWorkItem(cleaner, preferLocal: false);
#else
        ThreadPool.UnsafeQueueUserWorkItem ((o) => ((InternPoolCleaner)o).Execute(), cleaner);
#endif
        return true;
    }

    private bool Trim (InternPool.TrimLevel level)
    {
        var pools = _pools;
#if NET5_0 || NETCOREAPP3_1
            MemoryPressure pressure = GetMemoryPressure();
            if (pressure == MemoryPressure.High)
            {
                // Under high pressure, release everything
                for (int i = 0; i < pools.Length; i++)
                {
                    var pool = pools[i];
                    if (pool != null)
                    {
                        lock (pool)
                        {
                            _totalAdded += pool.Added;
                            _totalConsidered += pool.Considered;
                            _totalDeduped += pool.Deduped;
                            _totalEvictedCount += pool.Count + pool.Evicted;
                            pools[i] = null;
                        }

                    }
                }
            }
            else
            {
#endif
        for (int i = 0; i < pools.Length; i++)
        {
            var pool = pools[i];
            if (pool != null)
            {
                lock (pool)
                {
                    pool.Trim (level);
                }
            }
        }
#if NET5_0 || NETCOREAPP3_1
            }
#endif

        return true;
    }

#if NET5_0 || NETCOREAPP3_1
        private static MemoryPressure GetMemoryPressure()
        {
            const double HighPressureThreshold =
 .90;       // Percent of GC memory pressure threshold we consider "high"
            const double MediumPressureThreshold =
 .70;     // Percent of GC memory pressure threshold we consider "medium"

            GCMemoryInfo memoryInfo = GC.GetGCMemoryInfo();
            if (memoryInfo.MemoryLoadBytes >= memoryInfo.HighMemoryLoadThresholdBytes * HighPressureThreshold)
            {
                return MemoryPressure.High;
            }
            else if (memoryInfo.MemoryLoadBytes >= memoryInfo.HighMemoryLoadThresholdBytes * MediumPressureThreshold)
            {
                return MemoryPressure.Medium;
            }
            return MemoryPressure.Low;
        }
#endif

    private enum MemoryPressure
    {
        Low,
        Medium,
        High
    }
}
