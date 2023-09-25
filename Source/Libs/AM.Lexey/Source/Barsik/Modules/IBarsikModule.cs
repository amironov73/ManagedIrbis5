// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IBarsikModule.cs -- интерфейс Барсик-модуля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Lexey.Barsik;

/// <summary>
/// Интерфейс Барсик-модуля.
/// </summary>
public interface IBarsikModule
{
    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Версия модуля.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Инициализация модуля при загрузке в интерпретатор.
    /// </summary>
    bool AttachModule (Context context);

    /// <summary>
    /// Освобождение (деинициализация) модуля
    /// при выгрузке из интерпретатора.
    /// </summary>
    void DetachModule (Context context);
}
