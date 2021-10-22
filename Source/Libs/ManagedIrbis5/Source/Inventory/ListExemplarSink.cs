// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ListExemplarSink.cs -- приемник экземпляров, хранящий их в оперативной памяти
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
    /// Приемник экземпляров, хранящий их в оперативной памяти.
    /// </summary>
    public sealed class ListExemplarSink
        : IExemplarSink
    {
        #region Properties

        /// <summary>
        /// Собранные экземпляры.
        /// </summary>
        public List<ExemplarInfo> Exemplars { get; } = new ();

        #endregion

        #region IExemplarSink members

        /// <inheritdoc cref="IExemplarSink.AddExemplars"/>
        public void AddExemplars
            (
                IEnumerable<ExemplarInfo> exemplars
            )
        {
            Exemplars.AddRange (exemplars);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => Exemplars.Clear();

        #endregion

    } // class ListExemplarSink

} // namespace ManagedIrbis.Inventory
