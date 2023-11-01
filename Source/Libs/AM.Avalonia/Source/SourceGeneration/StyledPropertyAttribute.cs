// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StyledPropertyAttribute.cs -- генерирует Styled-поля Авалонии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Avalonia.SourceGeneration;

/// <summary>
/// Атрибут, заставляющий генерировать Styled-свойства Avalonia.
/// </summary>
[AttributeUsage (AttributeTargets.Field)]
public class StyledPropertyAttribute
    : Attribute
{
    // пустое тело класса
}
