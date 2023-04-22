// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AvaloniaUtility.cs -- вспомогательные методы для работы из Barsik с Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;
using AM.Kotik.Avalonia.Controls;
using AM.Kotik.Barsik;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Avalonia;

/// <summary>
/// Вспомогательные методы для работы из Barsik с Avalonia.
/// </summary>
[PublicAPI]
public static class AvaloniaBarsikUtility
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

        var context = interpreter.Context;
        var resolver = context.Commmon.Resolver;
        var assemblies = resolver.Assemblies;
        var namespaces = resolver.Namespaces;

        assemblies.Add (typeof (Window).Assembly); // Avalonia.Controls
        assemblies.Add (typeof (Thickness).Assembly); // Avalonia.Base
        assemblies.Add (typeof (DataGrid).Assembly); // Avalonia.DataGrid
        assemblies.Add (typeof (ManagedFileChooser).Assembly); // Avalonia.Dialogs
        assemblies.Add (typeof (AvaloniaUtility).Assembly); // AM.Avalonia
        assemblies.Add (typeof (BarsikWindow).Assembly); // эта сборка

        namespaces.Add ("Avalonia");
        namespaces.Add ("Avalonia.Animation");
        namespaces.Add ("Avalonia.Controls");
        namespaces.Add ("Avalonia.Data");
        namespaces.Add ("Avalonia.Layout");
        namespaces.Add ("Avalonia.Media");
        namespaces.Add ("Avalonia.Styling");
        namespaces.Add ("AM.Avalonia");
        namespaces.Add ("AM.Avalonia.Controls");
        namespaces.Add ("AM.Kotik.Avalonia");
        namespaces.Add ("AM.Kotik.Avalonia.Controls");
        interpreter.Context.AttachModule (new AvaloniaModule());

        return interpreter;
    }

    #endregion
}
