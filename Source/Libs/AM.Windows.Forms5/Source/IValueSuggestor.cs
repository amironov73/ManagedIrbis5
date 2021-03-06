﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IValueSuggestor.cs -- интерфейс подсказывателя значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Интерфейс подсказывателя значений.
    /// </summary>
    public interface IValueSuggestor
    {
        /// <summary>
        /// Получение перечня подсказываемых значений.
        /// </summary>
        ICollection GetSuggestedValues ();

    } // interface IValueSuggestor

} // namespace AM.Windows.Forms
