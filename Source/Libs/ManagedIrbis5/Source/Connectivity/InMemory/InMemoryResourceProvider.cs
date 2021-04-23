// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryResourceProvider.cs -- провайдер ресурсов, расположенных в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Провайдер ресурсов, расположенных в оперативной памяти.
    /// </summary>
    public sealed class InMemoryResourceProvider
        : IResourceProvider
    {
        #region IResourceProvider members

        /// <inheritdoc cref="IResourceProvider.ListResources"/>
        public string[] ListResources
            (
                string path
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IResourceProvider.ReadResource"/>
        public string? ReadResource
            (
                string fileName
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IResourceProvider.ResourceExists"/>
        public bool ResourceExists
            (
                string fileName
            )
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class InMemoryResourceProvider

} // namespace ManagedIrbis.InMemory
