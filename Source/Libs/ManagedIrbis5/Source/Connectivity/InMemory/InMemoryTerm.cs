// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InMemoryTerm.cs -- термин поискового словаря в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Термин поискового словаря в оперативной памяти.
    /// </summary>
    public class InMemoryTerm
    {
        #region Properties

        /// <summary>
        /// Текст.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Постинги.
        /// </summary>
        public List<InMemoryPosting>? Postings { get; set; }

        #endregion
    }
}
