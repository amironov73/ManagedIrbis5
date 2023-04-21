// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* AvaloniaUtility.cs -- вспомогательные методы для работы из Barsik с Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Kotik.Barsik;

#endregion

namespace AM.Kotik.Avalonia;

/// <summary>
/// Вспомогательные методы для работы из Barsik с Avalonia.
/// </summary>
public static class AvaloniaUtility
{
    #region Public methods

    /// <summary>
    /// Подключение WinForms-модуля.
    /// </summary>
    public static Interpreter WithAvalonia
        (
            this Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        interpreter.Context.AttachModule (new AvaloniaModule());

        return interpreter;
    }

    #endregion
}
