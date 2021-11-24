// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryInverted.cs -- инвертированный файл, расположенный в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Инвертированный файл, расположенный в оперативной памяти.
    /// </summary>
    public class InMemoryInverted
        : CaseInsensitiveSortedList<InMemoryTerm>
    {
        #region Public methods

        /// <summary>
        /// Дамп инвертированного файла.
        /// </summary>
        public void Dump
            (
                TextWriter output
            )
        {
            Sure.NotNull (output);

            // TODO: implement
        }

        /// <summary>
        /// Загрузка из потока.
        /// </summary>
        public void Read
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            // TODO: implement
        }

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void Save
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            // TODO: implement
        }

        #endregion
    }
}
