// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StateHolder.cs -- контейнер для хранения значения и отслеживания его изменений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

using AM.Runtime;

#endregion

#nullable enable

namespace AM.Threading
{
    /// <summary>
    /// Контейнер для хранения значения и отслеживания его изменений.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public sealed class StateHolder<T>
        : IHandmadeSerializable
        where T : IEquatable<T>
    {
        #region Events

        /// <summary>
        /// Вызывается при изменении значения.
        /// </summary>
        public event EventHandler? ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Allow <c>null</c> values?
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// Хэндл для ожидания изменения значения.
        /// </summary>
        public WaitHandle WaitHandle => _waitHandle;

        /// <summary>
        /// Value itself.
        /// </summary>
        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StateHolder
            (
                T value
            )
        {
            _lock = new object();
            AllowNull = true;
            _waitHandle = new AutoResetEvent(false);
            _value = value;
        }

        #endregion

        #region Private members

        private readonly object _lock;

        private T _value;

        private readonly AutoResetEvent _waitHandle;

        #endregion

        #region Public methods

        /// <summary>
        /// Установка нового значения.
        /// </summary>
        public void SetValue
            (
                T newValue
            )
        {
            lock (_lock)
            {

                if (!AllowNull &&
                    ReferenceEquals(newValue, null))
                {
                    Magna.Error
                        (
                            nameof(StateHolder<T>)
                            +
                            "::"
                            + nameof(SetValue)
                            + ": newValue is null"
                        );

                    throw new ArgumentNullException();
                }

                var changed = false;

                if (ReferenceEquals(_value, null))
                {
                    if (!ReferenceEquals(newValue, null))
                    {
                        changed = true;
                    }
                }
                else
                {
                    if (!_value.Equals(newValue))
                    {
                        changed = true;
                    }
                }

                _value = newValue!;

                if (changed)
                {
                    _waitHandle.Set();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator T
            (
                StateHolder<T> holder
            )
        {
            return holder.Value;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator StateHolder<T>
            (
                T value
            )
        {
            return new StateHolder<T>(value);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        [ExcludeFromCodeCoverage]
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            var flag = reader.ReadBoolean();

            if (flag)
            {
                Magna.Error
                    (
                        nameof(StateHolder<T>)
                        + "::"
                        + nameof(RestoreFromStream)
                        + ": not implemented"
                    );

                throw new NotImplementedException();
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        [ExcludeFromCodeCoverage]
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            if (ReferenceEquals(_value, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);

                var serializable = _value as IHandmadeSerializable;

                if (ReferenceEquals(serializable, null))
                {
                    Magna.Error
                        (
                            nameof(StateHolder<T>)
                            + "::"
                            + nameof(SaveToStream)
                            + ": nonserializable value"
                        );

                    throw new NotImplementedException();
                }

                serializable.SaveToStream(writer);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Value?.ToString() ?? "(none)";
        }

        #endregion
    }
}
