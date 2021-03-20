// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* OuterObject.cs -- внешний (по отношению к PFT-скрипту) объект
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Внешний (по отношению к PFT-скрипту) объект.
    /// </summary>
    abstract class OuterObject
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Usage counter.
        /// </summary>
        public int Counter { get; internal set; }

        /// <summary>
        /// Name of the object.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        protected OuterObject
            (
                string name
            )
        {
            Name = name;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Call method of the object.
        /// </summary>
        public virtual object? CallMethod
            (
                string methodName,
                object[] parameters
            )
        {
            // Nothing to do here

            return null;
        }

        /// <summary>
        /// Decrease counter.
        /// </summary>
        public int DecreaseCounter()
        {
            return --Counter;
        }

        /// <summary>
        /// Increase counter.
        /// </summary>
        public int IncreaseCounter()
        {
            return ++Counter;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
