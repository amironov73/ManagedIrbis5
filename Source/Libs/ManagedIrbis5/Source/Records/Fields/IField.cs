// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IField.cs -- маркерный интерфейс для поля записи
 * Ars Magna project, http://arsmagna.ru
 */

using System;

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Маркерный интерфейс для поля записи.
/// </summary>
public interface IField
{
    /// <summary>
    /// Метка поля.
    /// </summary>
    public int Tag { get; set; }

    /// <summary>
    /// Текстовое представление поля.
    /// </summary>
    public ReadOnlyMemory<char> Text { get; set; }
}
