// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ValueFactoryCreator.cs -- создает фабрики значений для анимации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace AeroSuite.AnimationEngine;

/// <summary>
/// Создает фабрики значений для анимации.
/// </summary>
public static class ValueFactoryCreator
{
    /// <summary>
    /// Создание списочной фабрики.
    /// </summary>
    public static ValueFactory<List<T>> CreateListFactory<T>
        (
            ValueFactory<T> originalFactory
        )
    {
        Sure.NotNull (originalFactory);

        return (startValue, targetValue, progress) =>
        {
            if (startValue == null)
            {
                throw new ArgumentNullException (nameof (startValue), "The start value must not be null.");
            }

            if (targetValue == null)
            {
                throw new ArgumentNullException (nameof (targetValue), "The target value must not be null.");
            }

            if (startValue.Count != targetValue.Count)
            {
                throw new ArgumentOutOfRangeException (nameof (targetValue),
                    "The target value item count must be equal to the start value item count.");
            }

            return startValue.Zip (targetValue, (start, target) => originalFactory (start, target, progress)).ToList();
        };
    }

    /// <summary>
    /// Создание фабрики-массива.
    /// </summary>
    public static ValueFactory<T[]> CreateArrayFactory<T>
        (
            ValueFactory<T> originalFactory
        )
    {
        Sure.NotNull (originalFactory);

        return (startValue, targetValue, progress) =>
        {
            if (startValue == null)
            {
                throw new ArgumentNullException (nameof (startValue), "The start value must not be null.");
            }

            if (targetValue == null)
            {
                throw new ArgumentNullException (nameof (targetValue), "The target value must not be null.");
            }

            if (startValue.Length != targetValue.Length)
            {
                throw new ArgumentOutOfRangeException (nameof (targetValue),
                    "The target value item count must be equal to the start value item count.");
            }

            return startValue.Zip (targetValue, (start, target) => originalFactory (start, target, progress)).ToArray();
        };
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2>> CreateTupleFactory<T1, T2>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
                (
                    t1Factory (startValue.Item1, targetValue.Item1, progress),
                    t2Factory (startValue.Item2, targetValue.Item2, progress)
                );
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2, T3>> CreateTupleFactory<T1, T2, T3>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory,
            ValueFactory<T3> t3Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);
        Sure.NotNull (t3Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
                (
                    t1Factory (startValue.Item1, targetValue.Item1, progress),
                    t2Factory (startValue.Item2, targetValue.Item2, progress),
                    t3Factory (startValue.Item3, targetValue.Item3, progress)
                );
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2, T3, T4>> CreateTupleFactory<T1, T2, T3, T4>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory,
            ValueFactory<T3> t3Factory,
            ValueFactory<T4> t4Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);
        Sure.NotNull (t3Factory);
        Sure.NotNull (t4Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
                (
                    t1Factory (startValue.Item1, targetValue.Item1, progress),
                    t2Factory (startValue.Item2, targetValue.Item2, progress),
                    t3Factory (startValue.Item3, targetValue.Item3, progress),
                    t4Factory (startValue.Item4, targetValue.Item4, progress)
            );
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2, T3, T4, T5>> CreateTupleFactory<T1, T2, T3, T4, T5>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory,
            ValueFactory<T3> t3Factory,
            ValueFactory<T4> t4Factory,
            ValueFactory<T5> t5Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);
        Sure.NotNull (t3Factory);
        Sure.NotNull (t4Factory);
        Sure.NotNull (t5Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
                (
                    t1Factory (startValue.Item1, targetValue.Item1, progress),
                    t2Factory (startValue.Item2, targetValue.Item2, progress),
                    t3Factory (startValue.Item3, targetValue.Item3, progress),
                    t4Factory (startValue.Item4, targetValue.Item4, progress),
                    t5Factory (startValue.Item5, targetValue.Item5, progress)
                );
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2, T3, T4, T5, T6>> CreateTupleFactory<T1, T2, T3, T4, T5, T6>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory,
            ValueFactory<T3> t3Factory,
            ValueFactory<T4> t4Factory,
            ValueFactory<T5> t5Factory,
            ValueFactory<T6> t6Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);
        Sure.NotNull (t3Factory);
        Sure.NotNull (t4Factory);
        Sure.NotNull (t5Factory);
        Sure.NotNull (t6Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
                (
                    t1Factory (startValue.Item1, targetValue.Item1, progress),
                    t2Factory (startValue.Item2, targetValue.Item2, progress),
                    t3Factory (startValue.Item3, targetValue.Item3, progress),
                    t4Factory (startValue.Item4, targetValue.Item4, progress),
                    t5Factory (startValue.Item5, targetValue.Item5, progress),
                    t6Factory (startValue.Item6, targetValue.Item6, progress)
                );
    }

    /// <summary>
    /// Создание фабрики-кортежа.
    /// </summary>
    public static ValueFactory<Tuple<T1, T2, T3, T4, T5, T6, T7>> CreateTupleFactory<T1, T2, T3, T4, T5, T6, T7>
        (
            ValueFactory<T1> t1Factory,
            ValueFactory<T2> t2Factory,
            ValueFactory<T3> t3Factory,
            ValueFactory<T4> t4Factory,
            ValueFactory<T5> t5Factory,
            ValueFactory<T6> t6Factory,
            ValueFactory<T7> t7Factory
        )
    {
        Sure.NotNull (t1Factory);
        Sure.NotNull (t2Factory);
        Sure.NotNull (t3Factory);
        Sure.NotNull (t4Factory);
        Sure.NotNull (t5Factory);
        Sure.NotNull (t6Factory);
        Sure.NotNull (t7Factory);

        return (startValue, targetValue, progress)
            => Tuple.Create
            (
                t1Factory (startValue.Item1, targetValue.Item1, progress),
                t2Factory (startValue.Item2, targetValue.Item2, progress),
                t3Factory (startValue.Item3, targetValue.Item3, progress),
                t4Factory (startValue.Item4, targetValue.Item4, progress),
                t5Factory (startValue.Item5, targetValue.Item5, progress),
                t6Factory (startValue.Item6, targetValue.Item6, progress),
                t7Factory (startValue.Item7, targetValue.Item7, progress)
            );
    }
}
