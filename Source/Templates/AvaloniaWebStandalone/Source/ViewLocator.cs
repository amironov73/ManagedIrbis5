// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ViewLocator.cs -- находит представления для моделей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

#endregion

#pragma warning disable IL2057 // Type.GetType не может гарантировать доступность типа

#nullable enable

namespace AvaloniaWeb;

/// <summary>
/// Находит представления для моделей.
/// </summary>
public class ViewLocator
    : IDataTemplate
{
    public IControl? Build 
        (
            object? data
        )
    {
        if (data is null)
        {
            return null;
        }

        var name = data.GetType().FullName!.Replace ("ViewModel", "View");
        var type = Type.GetType (name, throwOnError: false);

        if (type != null)
        {
            return (Control)Activator.CreateInstance (type)!;
        }

        return new TextBlock { Text = name };
    }

    public bool Match (object? data)
    {
        return data is ViewModelBase;
    }
}
