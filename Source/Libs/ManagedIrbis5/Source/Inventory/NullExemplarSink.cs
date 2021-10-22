// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NullExemplarSink.cs -- поглотитель экземпляров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Inventory
{
    /// <summary>
    /// Поглотитель экземпляров.
    /// </summary>
    public sealed class NullExemplarSink
        : IExemplarSink
    {
        #region IExemplarSink members

        /// <inheritdoc cref="IExemplarSink.AddExemplars"/>
        public void AddExemplars
            (
                IEnumerable<ExemplarInfo> exemplars
            )
        {
        } // method AddExemplars

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
        } // method Dispose

        #endregion

    } // class NullExemplarSink

} // namespace ManagedIrbis.Inventory
