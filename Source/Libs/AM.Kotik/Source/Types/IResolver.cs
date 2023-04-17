// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ITypeResolver.cs -- интерфейс резольвера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using AM.Reflection;

#endregion

#nullable enable

namespace AM.Kotik.Types;

/// <summary>
/// Интерфейс резольвера.
/// </summary>
public interface IResolver
{
    /// <summary>
    /// Известные резольверу сборки, содержащие типы.
    /// </summary>
    HashSet<Assembly> Assemblies { get; }

    /// <summary>
    /// Известные резольверу пространства имен.
    /// </summary>
    HashSet<string> Namespaces { get; }

    /// <summary>
    /// Сброс резольвера в начальное состояние.
    /// </summary>
    void Reset();

    /// <summary>
    /// Разрешение конструктора.
    /// </summary>
    ConstructorInfo? ResolveConstructor
        (
            ConstructorDescriptor descriptor
        );

    /// <summary>
    /// Разрешение свойства или поля класса.
    /// </summary>
    PropertyOrField? ResolveMember
        (
            MemberDescriptor descriptor
        );

    /// <summary>
    /// Разрешение метода.
    /// </summary>
    MethodInfo? ResolveMethod
        (
            MethodDescriptor descriptor
        );

    /// <summary>
    /// Разрешение типа.
    /// </summary>
    Type? ResolveType (TypeDescriptor descriptor);
}
