// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* WinFormsUtility.cs -- вспомогательные методы для работы из Barsik с WinForms
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM.Scripting.Barsik;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace AM.Scripting.WinForms;

/// <summary>
/// Вспомогательные методы для работы из Barsik с WinForms.
/// </summary>
public static class WinFormsUtility
{
    #region Public methods

    /// <summary>
    /// Подключение WinForms-модуля.
    /// </summary>
    public static Interpreter WithWinForms
        (
            this Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        interpreter.Context.AttachModule (new WinFormsModule());

        return interpreter;
    }

    #endregion
}
