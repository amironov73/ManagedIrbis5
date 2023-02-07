// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NaiveResolver.cs -- простейший наивный резольвер
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
/// Простейший наивный резольвер типов.
/// </summary>
public class NaiveResolver
    : IResolver
{
    #region Properties

    /// <inheritdoc cref="IResolver.Assemblies"/>
    public HashSet<Assembly> Assemblies { get; } = new ();

    /// <inheritdoc cref="IResolver.Namespaces"/>
    public HashSet<string> Namespaces { get; } = new ();

    #endregion

    #region IResolver members

    /// <inheritdoc cref="IResolver.ResolveConstructor"/>
    public virtual ConstructorInfo? ResolveConstructor
        (
            Type type,
            ConstructorDescriptor descriptor
        )
    {
        Sure.NotNull (type);
        Sure.NotNull (descriptor);

        var arguments = descriptor.Arguments ?? Array.Empty<Type>();
        return type.GetConstructor (BindingFlags.Public|BindingFlags.Instance, arguments);
    }

    /// <inheritdoc cref="IResolver.ResolveMember"/>
    public virtual PropertyOrField? ResolveMember
        (
            Type type,
            MemberDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        return null;
    }

    /// <inheritdoc cref="IResolver.ResolveMethod"/>
    public virtual MethodInfo? ResolveMethod
        (
            Type type,
            MethodDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        return null;
    }

    /// <inheritdoc cref="IResolver.ResolveType"/>
    public virtual Type? ResolveType
        (
            TypeDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        return null;
    }

    #endregion
}
