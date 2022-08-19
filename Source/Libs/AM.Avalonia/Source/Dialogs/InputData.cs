// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* InputData.cs -- описание вводимых данных для InputDialog
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
/// Описание вводимых данных для <see cref="InputDialog"/>.
/// </summary>
public sealed class InputData
{
    #region Properties

    /// <summary>
    /// Заголовок окна.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Пояснение, что требуется ввести.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Введенное значение.
    /// </summary>
    public string? Value { get; set; }

    #endregion
}
