// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

/* SerializerAttribute.cs -- маркирует метод-сериализатор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Runtime;

/// <summary>
/// Маркирует метод-сериализатор
/// </summary>
[AttributeUsage (AttributeTargets.Method)]
public sealed class SerializerAttribute
    : Attribute
{
    // пустое тело класса
}
