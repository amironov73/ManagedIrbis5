// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BindingAttribute.cs -- генерирует компайл-тайм привязку Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Avalonia.SourceGeneration;

/// <summary>
/// Атрибут, заставляющий генерировать компайл-тайм привязку Avalonia.
/// </summary>
[AttributeUsage (AttributeTargets.Property)]
public sealed class BindingAttribute
    : Attribute
{
    // пустое тело класса
}
