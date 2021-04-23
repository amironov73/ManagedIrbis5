// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryTerm.cs -- термин поискового словаря в оперативной памяти
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
        /// Запись.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Позиция в поле.
        /// </summary>
        public int Position { get; set; }

        #endregion

    } // class InMemoryTerm

} // namespace ManagedIrbis.InMemory
