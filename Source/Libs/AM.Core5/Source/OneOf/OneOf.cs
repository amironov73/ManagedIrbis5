// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* OneOf.cs -- контейнер для значения одного из указанных типов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

#endregion

#nullable disable

namespace AM;

/// <summary>
/// Контейнер для значения одного из двух указанных типов.
/// </summary>
/// <typeparam name="T1">Первый тип.</typeparam>
/// <typeparam name="T2">Второй тип. Не должен совпадать с первым.</typeparam>
[PublicAPI]
public class OneOf<T1, T2>
{
    #region Properties

    /// <summary>
    /// Хранимое значение, возможно, boxed.
    /// </summary>
    public object Value => _index switch
    {
        0 => _value1,
        _ => _value2
    };

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор первого типа.
    /// </summary>
    public OneOf (T1 value1)
    {
        _index = 0;
        _value1 = value1;
        _value2 = default;
    }

    /// <summary>
    /// Конструктор второго типа.
    /// </summary>
    public OneOf (T2 value2)
    {
        _index = 1;
        _value2 = value2;
        _value1 = default;
    }

    #endregion

    #region Private members

    private readonly int _index;

    private readonly T1 _value1;

    private readonly T2 _value2;

    private OneOf<T1, T2> CheckIndex (int expected)
    {
        if (_index != expected)
        {
            throw new InvalidCastException();
        }

        return this;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка: хранится значение первого типа?
    /// </summary>
    public bool Is1 => _index == 0;

    /// <summary>
    /// Проверка: хранится значение второго типа?
    /// </summary>
    public bool Is2 => _index == 1;

    /// <summary>
    /// Попытка извлечения значения первого типа.
    /// </summary>
    public bool Try1 (out T1 value)
    {
        if (Is1)
        {
            value = _value1;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения второго типа.
    /// </summary>
    public bool Try2 (out T2 value)
    {
        if (Is2)
        {
            value = _value2;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Получение значения превого типа.
    /// </summary>
    public T1 As1() => CheckIndex (0)._value1;

    /// <summary>
    /// Получение значения второго типа.
    /// </summary>
    public T2 As2() => CheckIndex (1)._value2;

    /// <summary>
    /// Выполнение действий в зависимости от того,
    /// какое значение какого типа хранится.
    /// </summary>
    public void Switch
        (
            Action<T1> action1,
            Action<T2> action2
        )
    {
        switch (_index)
        {
            case 0:
                action1?.Invoke (_value1);
                break;

            default:
                action2?.Invoke (_value2);
                break;
        }
    }

    /// <summary>
    /// Вычисление результата в зависимости от того,
    /// значение какого типа хранится.
    /// </summary>
    public TResult Match<TResult>
        (
            Func<T1, TResult> func1,
            Func<T2, TResult> func2
        )
        => _index switch
        {
            0 => func1 (_value1),
            _ => func2 (_value2)
        };

    #endregion

    #region Operators

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator OneOf<T1, T2> (T1 value1) => new OneOf<T1, T2> (value1);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator OneOf<T1, T2> (T2 value2) => new OneOf<T1, T2> (value2);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T1 (OneOf<T1, T2> value) => value.CheckIndex (0)._value1;

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T2 (OneOf<T1, T2> value) => value.CheckIndex (1)._value2;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => _index switch
    {
        0 => $"{typeof (T1)}: {_value1}",
        _ => $"{typeof (T2)}: {_value2}"
    };

    #endregion
}

//===================================================================

/// <summary>
/// Контейнер для значения, принадлежащего
/// одному из трех указанных типов.
/// </summary>
[PublicAPI]
public class OneOf<T1, T2, T3>
{
    #region Properties

    /// <summary>
    /// Хранимое значение, возможно, boxed.
    /// </summary>
    public object Value => _index switch
    {
        0 => _value1,
        1 => _value2,
        _ => _value3
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Конструктор первого типа.
    /// </summary>
    public OneOf (T1 value1)
    {
        _index = 0;
        _value1 = value1;
        _value2 = default;
        _value2 = default;
    }

    /// <summary>
    /// Конструктор второго типа.
    /// </summary>
    public OneOf (T2 value2)
    {
        _index = 1;
        _value2 = value2;
        _value1 = default;
        _value3 = default;
    }

    /// <summary>
    /// Конструктор третьего типа.
    /// </summary>
    public OneOf (T3 value3)
    {
        _index = 2;
        _value3 = value3;
        _value1 = default;
        _value2 = default;
    }

    #endregion

    #region Private members

    private readonly int _index;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;

    private OneOf<T1, T2, T3> CheckIndex
        (
            int expected
        )
    {
        if (_index != expected)
        {
            throw new InvalidCastException();
        }

        return this;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Прверка: хранится значение первого типа?
    /// </summary>
    [Pure]
    public bool Is1 => _index == 0;

    /// <summary>
    /// Прверка: хранится значение второго типа?
    /// </summary>
    [Pure]
    public bool Is2 => _index == 1;

    /// <summary>
    /// Прверка: хранится значение третьего типа?
    /// </summary>
    [Pure]
    public bool Is3 => _index == 2;

    /// <summary>
    /// Попытка извлечения значения первого типа.
    /// </summary>
    public bool Try1 (out T1 value)
    {
        if (Is1)
        {
            value = _value1;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения второго типа.
    /// </summary>
    public bool Try2 (out T2 value)
    {
        if (Is2)
        {
            value = _value2;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения третьего типа.
    /// </summary>
    public bool Try3 (out T3 value)
    {
        if (Is3)
        {
            value = _value3;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Извлечение значения первого типа.
    /// </summary>
    [Pure]
    public T1 As1()
    {
        return CheckIndex (0)._value1;
    }

    /// <summary>
    /// Извлечение значения второго типа.
    /// </summary>
    [Pure]
    public T2 As2()
    {
        return CheckIndex (1)._value2;
    }

    /// <summary>
    /// Извлечение значения третьего типа.
    /// </summary>
    [Pure]
    public T3 As3()
    {
        return CheckIndex (2)._value3;
    }

    /// <summary>
    /// Выполнение действий в зависимости от того,
    /// какое значение какого типа хранится.
    /// </summary>
    public void Switch
        (
            Action<T1> action1,
            Action<T2> action2,
            Action<T3> action3
        )
    {
        switch (_index)
        {
            case 0:
                action1?.Invoke (_value1);
                break;

            case 1:
                action2?.Invoke (_value2);
                break;

            default:
                action3?.Invoke (_value3);
                break;
        }
    }

    /// <summary>
    /// Вычисление результата в зависимости от того,
    /// значение какого типа хранится.
    /// </summary>
    public TResult Match<TResult>
        (
            Func<T1, TResult> func1,
            Func<T2, TResult> func2,
            Func<T3, TResult> func3
        )
    {
        return _index switch
        {
            0 => func1 (_value1),
            1 => func2 (_value2),
            _ => func3 (_value3)
        };
    }

    #endregion

    #region Operators

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3>
        (
            T1 value1
        )
    {
        return new OneOf<T1, T2, T3> (value1);
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3>
        (
            T2 value2
        )
    {
        return new OneOf<T1, T2, T3> (value2);
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3>
        (
            T3 value3
        )
    {
        return new OneOf<T1, T2, T3> (value3);
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T1
        (
            OneOf<T1, T2, T3> value
        )
    {
        return value.CheckIndex (0)._value1;
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T2
        (
            OneOf<T1, T2, T3> value
        )
    {
        return value.CheckIndex (1)._value2;
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T3
        (
            OneOf<T1, T2, T3> value
        )
    {
        return value.CheckIndex (2)._value3;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => _index switch
    {
        0 => $"{typeof (T1)}: {_value1}",
        1 => $"{typeof (T2)}: {_value2}",
        _ => $"{typeof (T3)}: {_value3}"
    };

    #endregion
}

//===================================================================

/// <summary>
/// Контейнер для значения, принадлежащего
/// одному из четырех указанных типов.
/// </summary>
[PublicAPI]
public class OneOf<T1, T2, T3, T4>
{
    #region Properties

    /// <summary>
    /// Хранимое значение, возможно, boxed.
    /// </summary>
    public object Value => _index switch
    {
        0 => _value1,
        1 => _value2,
        2 => _value3,
        _ => _value4
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Конструктор первого типа.
    /// </summary>
    public OneOf (T1 value1)
    {
        _index = 0;
        _value1 = value1;
        _value2 = default;
        _value3 = default;
        _value4 = default;
    }

    /// <summary>
    /// Конструктор второго типа.
    /// </summary>
    public OneOf (T2 value2)
    {
        _index = 1;
        _value2 = value2;
        _value1 = default;
        _value3 = default;
        _value4 = default;
    }

    /// <summary>
    /// Конструктор третьего типа.
    /// </summary>
    public OneOf (T3 value3)
    {
        _index = 2;
        _value3 = value3;
        _value1 = default;
        _value2 = default;
        _value4 = default;
    }

    /// <summary>
    /// Конструктор четвертого типа.
    /// </summary>
    public OneOf (T4 value4)
    {
        _index = 3;
        _value4 = value4;
        _value1 = default;
        _value2 = default;
        _value3 = default;
    }

    #endregion

    #region Private members

    private readonly int _index;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;
    private readonly T4 _value4;

    private OneOf<T1, T2, T3, T4> CheckIndex
        (
            int expected
        )
    {
        if (_index != expected)
        {
            throw new InvalidCastException();
        }

        return this;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Прверка: хранится значение первого типа?
    /// </summary>
    [Pure]
    public bool Is1 => _index == 0;

    /// <summary>
    /// Прверка: хранится значение второго типа?
    /// </summary>
    [Pure]
    public bool Is2 => _index == 1;

    /// <summary>
    /// Прверка: хранится значение третьего типа?
    /// </summary>
    [Pure]
    public bool Is3 => _index == 2;

    /// <summary>
    /// Прверка: хранится значение четвертого типа?
    /// </summary>
    [Pure]
    public bool Is4 => _index == 3;

    /// <summary>
    /// Попытка извлечения значения первого типа.
    /// </summary>
    public bool Try1 (out T1 value)
    {
        if (Is1)
        {
            value = _value1;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения второго типа.
    /// </summary>
    public bool Try2 (out T2 value)
    {
        if (Is2)
        {
            value = _value2;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения третьего типа.
    /// </summary>
    public bool Try3 (out T3 value)
    {
        if (Is3)
        {
            value = _value3;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Попытка извлечения значения четвертого типа.
    /// </summary>
    public bool Try4 (out T4 value)
    {
        if (Is4)
        {
            value = _value4;

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Извлечение значения первого типа.
    /// </summary>
    [Pure]
    public T1 As1()
    {
        return CheckIndex (0)._value1;
    }

    /// <summary>
    /// Извлечение значения второго типа.
    /// </summary>
    [Pure]
    public T2 As2()
    {
        return CheckIndex (1)._value2;
    }

    /// <summary>
    /// Извлечение значения третьего типа.
    /// </summary>
    [Pure]
    public T3 As3()
    {
        return CheckIndex (2)._value3;
    }

    /// <summary>
    /// Извлечение значения четвертого типа.
    /// </summary>
    [Pure]
    public T4 As4()
    {
        return CheckIndex (3)._value4;
    }

    /// <summary>
    /// Выполнение действий в зависимости от того,
    /// какое значение какого типа хранится.
    /// </summary>
    public void Switch
        (
            Action<T1> action1,
            Action<T2> action2,
            Action<T3> action3,
            Action<T4> action4
        )
    {
        switch (_index)
        {
            case 0:
                action1?.Invoke (_value1);
                break;

            case 1:
                action2?.Invoke (_value2);
                break;

            case 2:
                action3?.Invoke (_value3);
                break;

            default:
                action4?.Invoke (_value4);
                break;
        }
    }

    /// <summary>
    /// Вычисление результата в зависимости от того,
    /// значение какого типа хранится.
    /// </summary>
    public TResult Match<TResult>
        (
            Func<T1, TResult> func1,
            Func<T2, TResult> func2,
            Func<T3, TResult> func3,
            Func<T4, TResult> func4
        )
    {
        return _index switch
        {
            0 => func1 (_value1),
            1 => func2 (_value2),
            2 => func3 (_value3),
            _ => func4 (_value4)
        };
    }

    #endregion

    #region Operators

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3, T4> (T1 value1) => new (value1);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3, T4> (T2 value2) => new (value2);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3, T4> (T3 value3)
        => new (value3);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    [Pure]
    public static implicit operator OneOf<T1, T2, T3, T4> (T4 value4)
        => new (value4);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T1 (OneOf<T1, T2, T3, T4> value)
        => value.CheckIndex (0)._value1;

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T2 (OneOf<T1, T2, T3, T4> value)
        => value.CheckIndex (1)._value2;

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T3 (OneOf<T1, T2, T3, T4> value)
        => value.CheckIndex (2)._value3;

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T4 (OneOf<T1, T2, T3, T4> value)
        => value.CheckIndex (3)._value4;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => _index switch
    {
        0 => $"{typeof (T1)}: {_value1}",
        1 => $"{typeof (T2)}: {_value2}",
        2 => $"{typeof (T3)}: {_value3}",
        _ => $"{typeof (T3)}: {_value4}"
    };

    #endregion
}
