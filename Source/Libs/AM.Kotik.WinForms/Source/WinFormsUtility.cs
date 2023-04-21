// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WinFormsUtility.cs -- вспомогательные методы для работы из Barsik с WinForms
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Kotik.Barsik;

using JetBrains.Annotations;

#endregion

namespace AM.Kotik.WinForms;

/// <summary>
/// Вспомогательные методы для работы из Barsik с WinForms.
/// </summary>
[PublicAPI]
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

        var context = interpreter.Context;
        var resolver = context.Commmon.Resolver;
        var assemblies = resolver.Assemblies;
        var namespaces = resolver.Namespaces;
        assemblies.Add (typeof (Color).Assembly);
        assemblies.Add (typeof (Form).Assembly);
        namespaces.Add ("System.Drawing");
        namespaces.Add ("System.Windows.Forms");
        context.AttachModule (new WinFormsModule());

        return interpreter;
    }

    #endregion
}
