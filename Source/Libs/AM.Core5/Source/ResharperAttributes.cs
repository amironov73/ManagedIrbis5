// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ResharperAttributes.cs -- атрибуты Решарпера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace JetBrains.Annotations;

/// <summary>
/// Indicates that IEnumarable, passed as parameter, is not enumerated.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class NoEnumerationAttribute
    : Attribute
{
    // пустое тело класса
}
