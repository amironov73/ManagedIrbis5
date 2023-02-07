// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* CachingResolver.cs -- кеширующий резольвер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

using AM.Reflection;

#endregion

#nullable enable

namespace AM.Kotik.Types;

/// <summary>
/// Кеширующий резольвер типов.
/// </summary>
public sealed class CachingResolver
    : NaiveResolver
{
    #region Private members

    private readonly MemberCache _memberCache = new();
    private readonly MethodCache _methodCache = new ();
    private readonly TypeCache _typeCache = new ();

    #endregion

    #region IResolver members

    /// <inheritdoc cref="NaiveResolver.ResolveConstructor"/>
    public override ConstructorInfo? ResolveConstructor
        (
            Type type,
            ConstructorDescriptor descriptor
        )
    {
        Sure.NotNull (type);
        Sure.NotNull (descriptor);

        return null;
    }

    /// <inheritdoc cref="NaiveResolver.ResolveMember"/>
    public override PropertyOrField? ResolveMember
        (
            Type type,
            MemberDescriptor descriptor
        )
    {
        Sure.NotNull (type);
        Sure.NotNull (descriptor);

        if (!_memberCache.TryGetProperty (descriptor, out var result))
        {
            result = base.ResolveMember (type, descriptor);
            if (result is not null)
            {
                _memberCache.Add (descriptor, result);
            }
        }

        return result;
    }

    /// <inheritdoc cref="NaiveResolver.ResolveMethod"/>
    public override MethodInfo? ResolveMethod
        (
            Type type,
            MethodDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        if (!_methodCache.TryGetMethod (descriptor, out var result))
        {
            result = base.ResolveMethod (type, descriptor);
            if (result is not null)
            {
                _methodCache.Add (descriptor, result);
            }
        }

        return result;
    }

    /// <inheritdoc cref="NaiveResolver.ResolveType"/>
    public override Type? ResolveType
        (
            TypeDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        if (!_typeCache.TryGetType (descriptor, out var result))
        {
            result = base.ResolveType (descriptor);
            if (result is not null)
            {
                _typeCache.Add (descriptor, result);
            }
        }

        return result;
    }

    #endregion
}
