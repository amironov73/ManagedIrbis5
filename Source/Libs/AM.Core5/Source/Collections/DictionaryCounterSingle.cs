// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DictionaryCounterSingle.cs -- простой словарь-счетчик с дробными числами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Простой словарь-счетчик с дробными числами.
    /// </summary>
    public sealed class DictionaryCounterSingle<TKey>
        : Dictionary<TKey, float>
        where TKey: notnull
    {
        #region Properties

        /// <summary>
        /// Gets the total.
        /// </summary>
        public float Total
        {
            get
            {
                lock (SyncRoot)
                {
                    var result = 0.0f;
                    foreach (var value in Values)
                    {
                        result += value;
                    }

                    return result;
                }
            } // method get
        } // property Total

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterSingle{TKey}"/> class.
        /// </summary>
        public DictionaryCounterSingle()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DictionaryCounterSingle
            (
                IEqualityComparer<TKey> comparer
            )
            : base(comparer)
        {
        } // constructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterSingle{TKey}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public DictionaryCounterSingle
            (
                int capacity
            )
            : base(capacity)
        {
        } // constructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterSingle{TKey}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DictionaryCounterSingle
            (
                DictionaryCounterSingle<TKey> dictionary
            )
            : base(dictionary)
        {
        } // constructor

        #endregion

        #region Private members

        private object SyncRoot => ((ICollection)this).SyncRoot;

        #endregion

        #region Public methods

        /// <summary>
        /// Augments the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="increment">The value.</param>
        /// <returns>New value for given key.</returns>
        public float Augment
            (
                TKey key,
                float increment
            )
        {
            lock (SyncRoot)
            {
                TryGetValue(key, out var result);
                result += increment;
                this[key] = result;

                return result;
            }
        } // method Augment

        /// <summary>
        /// Get accumulated value for the specified key.
        /// </summary>
        public float GetValue
            (
                TKey key
            )
        {
            lock (SyncRoot)
            {
                TryGetValue(key, out var result);

                return result;
            }
        } // method GetValue

        /// <summary>
        /// Increment the specified key.
        /// </summary>
        public double Increment
            (
                TKey key
            )
        {
            return Augment(key, 1.0f);
        } // method Increment

        #endregion

    } // class DictionaryCounterSingle

} // namespace AM.Collections
