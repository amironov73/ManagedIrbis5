// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SubFieldPool.cs -- пул для подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пул для подполей.
    /// </summary>
    public sealed class SubFieldPool
        : DefaultObjectPool<SubField>
    {
        #region Nested classes

        /// <summary>
        /// Полиси создания и очистки подполей.
        /// </summary>
        class SubFieldPolicy
            : IPooledObjectPolicy<SubField>
        {
            #region IPooledObjectPolicy<T> members

            public SubField Create() => new ();

            public bool Return(SubField obj)
            {
                obj.Dispose();

                return true;
            } // method Return

            #endregion
        } // class SubFieldPolicy

        #endregion

        #region Properties

        /// <summary>
        /// Пул по умолчанию.
        /// </summary>
        public static SubFieldPool Default { get; } = new ();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubFieldPool()
            : this(new SubFieldPolicy())
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="policy">Полиси.</param>
        public SubFieldPool
            (
                IPooledObjectPolicy<SubField> policy
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
        public SubFieldPool
            (
                IPooledObjectPolicy<SubField> policy,
                int maximumRetained
            )
            : base(policy, maximumRetained)
        {
        } // constructor

        #endregion

    } // class SubFieldPool

} // namespace ManagedIrbis
