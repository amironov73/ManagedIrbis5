﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils
{
    internal class ObjectPool<T> where T : class, new()
    {
        public ObjectPool()
        {
        }

        public T Get()
        {
#if NO_POOLING
            return new T();
#else
            int count = _pool.Count;
            if (count == 0)
                return new T();

            var obj = _pool[count - 1];
            _pool.RemoveAt(count - 1);
            return obj;
#endif
        }

        public void Return(T obj)
        {
#if NO_POOLING
#else
            Cleaner?.Invoke(obj);
            _pool.Add(obj);
#endif
        }

        public void Return(IEnumerable<T> objs)
        {
#if NO_POOLING
#else
            if (Cleaner != null)
            {
                foreach (var x in objs)
                {
                    Cleaner(x);
                }
                _pool.AddRange(objs);
            }
#endif
        }

        public void ReturnAndClear(List<T> objs)
        {
#if NO_POOLING
#else
            if (Cleaner != null)
            {
                foreach (var x in objs)
                {
                    Cleaner(x);
                }
                _pool.AddRange(objs);
            }
#endif
            objs.Clear();
        }

        public Action<T> Cleaner;

        List<T> _pool = new List<T>();
    }
}
