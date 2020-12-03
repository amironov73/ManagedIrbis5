// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OneOf.cs -- контейнер для значения одного из указанных типов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable disable

namespace AM
{
    /// <summary>
    /// Контейнер для значения одного из двух указанных типов.
    /// </summary>
    /// <typeparam name="T1">Первый тип.</typeparam>
    /// <typeparam name="T2">Второй тип. Не должен совпадать с первым.</typeparam>
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
        public OneOf(T1 value1)
        {
            _index = 0;
            _value1 = value1;
            _value2 = default;
        }

        /// <summary>
        /// Конструктор второго типа.
        /// </summary>
        public OneOf(T2 value2)
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

        private OneOf<T1, T2> CheckIndex(int expected)
        {
            if (_index != expected)
            {
                throw new InvalidCastException();
            }

            return this;
        }

        #endregion

        #region Public methods

        public bool Is1 => _index == 0;

        public bool Is2 => _index == 1;

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

        public T1 As1() => CheckIndex(0)._value1;

        public T2 As2() => CheckIndex(1)._value2;

        public void Switch
            (
                Action<T1> action1,
                Action<T2> action2
            )
        {
            switch (_index)
            {
                case 0:
                    action1?.Invoke(_value1);
                    break;

                default:
                    action2?.Invoke(_value2);
                    break;
            }
        }

        public TResult Match<TResult>
            (
                Func<T1, TResult> func1,
                Func<T2, TResult> func2
            )
            => _index switch
            {
                0 => func1(_value1),
                _ => func2(_value2)
            };

        #endregion

        #region Operators

        public static implicit operator OneOf<T1, T2> (T1 value1)  => new OneOf<T1, T2>(value1);

        public static implicit operator OneOf<T1, T2> (T2 value2)  => new OneOf<T1, T2>(value2);

        public static implicit operator T1(OneOf<T1, T2> value) => value.CheckIndex(0)._value1;

        public static implicit operator T2(OneOf<T1, T2> value) => value.CheckIndex(1)._value2;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => _index switch
        {
            0 => $"{typeof(T1)}: {_value1}",
            _ => $"{typeof(T2)}: {_value2}"
        };

        #endregion
    }

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
        public OneOf(T1 value1)
        {
            _index = 0;
            _value1 = value1;
            _value2 = default;
            _value2 = default;
        }

        /// <summary>
        /// Конструктор второго типа.
        /// </summary>
        public OneOf(T2 value2)
        {
            _index = 1;
            _value2 = value2;
            _value1 = default;
            _value3 = default;
        }

        /// <summary>
        /// Конструктор третьего типа.
        /// </summary>
        public OneOf(T3 value3)
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

        #endregion

        #region Public methods

        public bool Is1 => _index == 0;

        public bool Is2 => _index == 1;

        public bool Is3 => _index == 2;

        public bool Try1(out T1 value)
        {
            if (Is1)
            {
                value = _value1;

                return true;
            }

            value = default;

            return false;
        }
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

        private OneOf<T1, T2, T3> CheckIndex(int expected)
        {
            if (_index != expected)
            {
                throw new InvalidCastException();
            }

            return this;
        }

        public T1 As1() => CheckIndex(0)._value1;

        public T2 As2() => CheckIndex(1)._value2;

        public T3 As3() => CheckIndex(2)._value3;

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
                    action1?.Invoke(_value1);
                    break;

                case 1:
                    action2?.Invoke(_value2);
                    break;

                default:
                    action3?.Invoke(_value3);
                    break;
            }
        }

        public TResult Match<TResult>
            (
                Func<T1, TResult> func1,
                Func<T2, TResult> func2,
                Func<T3, TResult> func3
            )
            => _index switch
            {
                0 => func1(_value1),
                1 => func2(_value2),
                _ => func3(_value3)
            };

        #endregion

        #region Operators

        public static implicit operator OneOf<T1, T2, T3>(T1 value1) => new OneOf<T1, T2, T3>(value1);

        public static implicit operator OneOf<T1, T2, T3>(T2 value2) => new OneOf<T1, T2, T3>(value2);

        public static implicit operator OneOf<T1, T2, T3>(T3 value3) => new OneOf<T1, T2, T3>(value3);

        public static implicit operator T1(OneOf<T1, T2, T3> value) => value.CheckIndex(0)._value1;

        public static implicit operator T2(OneOf<T1, T2, T3> value) => value.CheckIndex(1)._value2;

        public static implicit operator T3(OneOf<T1, T2, T3> value) => value.CheckIndex(2)._value3;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => _index switch
        {
            0 => $"{typeof(T1)}: {_value1}",
            1 => $"{typeof(T2)}: {_value2}",
            _ => $"{typeof(T3)}: {_value3}"
        };

        #endregion
    }
}
