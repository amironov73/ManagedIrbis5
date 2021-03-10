// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* SubFieldCollectionPool.cs -- пул для коллекции подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пул для коллекции подполей.
    /// </summary>
    public sealed class SubFieldCollectionPool
        : DefaultObjectPool<SubFieldCollection>
    {
        #region Nested classes

        /// <summary>
        /// Полиси создания и очистки подполей.
        /// </summary>
        class SubFieldCollectionPolicy
            : IPooledObjectPolicy<SubFieldCollection>
        {
            #region IPooledObjectPolicy<T> members

            public SubFieldCollection Create() => new ();

            public bool Return
                (
                    SubFieldCollection obj
                )
            {
                obj.Dispose();

                return true;
            } // method Return

            #endregion
        } // class SubFieldCollectionPolicy

        #endregion

        #region Properties

        /// <summary>
        /// Пул по умолчанию.
        /// </summary>
        public static SubFieldCollectionPool Default { get; } = new ();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubFieldCollectionPool()
            : this(new SubFieldCollectionPolicy())
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="policy">Полиси.</param>
        public SubFieldCollectionPool
            (
                IPooledObjectPolicy<SubFieldCollection> policy
            )
            : base(policy)
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="policy">Полиси.</param>
        /// <param name="maximumRetained">Максимальное количество
        /// удерживаемых в пуле экземпляров.</param>
        public SubFieldCollectionPool
            (
                IPooledObjectPolicy<SubFieldCollection> policy,
                int maximumRetained
            )
            : base(policy, maximumRetained)
        {
        } // constructor

        #endregion

    } // class SubFieldCollectionPool

} // namespace ManagedIrbis
