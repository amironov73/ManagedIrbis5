// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* RegisterDirectPropertyAttribute.cs -- атрибут для регистрации Direct-свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.CodeGenerators.Attributes;

/// <summary>
/// Атрибут для регистрации Direct-свойств.
/// </summary>
[PublicAPI]
[AttributeUsage (AttributeTargets.Property)]
public sealed class RegisterDirectPropertyAttribute
    : Attribute
{
    // пустое тело класса
}
