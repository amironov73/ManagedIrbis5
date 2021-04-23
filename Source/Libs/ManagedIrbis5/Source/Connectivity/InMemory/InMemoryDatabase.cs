// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryDatabase.cs -- база данных, расположенная в оперативной памяти
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
    /// База данных, расположенная в оперативной памяти.
    /// </summary>
    public class InMemoryDatabase
    {
        #region Properties

        /// <summary>
        /// Имя базы данных. Нечувствительно к регистру.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Мастер-файл.
        /// </summary>
        public InMemoryMaster Master { get; }

        /// <summary>
        /// Инвертированный файл.
        /// </summary>
        public InMemoryInverted Inverted { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryDatabase
            (
                string name
            )
        {
            Name = name;
            Master = new InMemoryMaster();
            Inverted = new InMemoryInverted();
        }

        #endregion

    } // class InMemoryDatabase

} // namespace ManagedIrbis.InMemory
