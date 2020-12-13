// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DisposableCollection.cs -- коллекция, состоящая из disposable-элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Коллекция, состоящая из <see cref="IDisposable"/> элементов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class DisposableCollection<T>
        : Collection<T>,
        IDisposable
        where T : IDisposable
    {
        #region Construction/destruction

        /// <summary>
        /// Finalize.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~DisposableCollection()
        {
            // TODO ???
            Dispose();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            for (int i = 0; i < Count; i++)
            {
                IDisposable item = this[i];
                item?.Dispose();
                //GC.SuppressFinalize(item); ???
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
