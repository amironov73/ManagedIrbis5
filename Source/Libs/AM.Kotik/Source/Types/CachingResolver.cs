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

using JetBrains.Annotations;

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace AM.Kotik.Types;

/// <summary>
/// Кеширующий резольвер типов.
/// </summary>
[PublicAPI]
public sealed class CachingResolver
    : NaiveResolver
{
    #region Private members

    private readonly ConstructorCache _constructorCache = new();
    private readonly MethodCache _methodCache = new ();
    private readonly MemberCache _memberCache = new ();
    private readonly TypeCache _typeCache = new ();

    private readonly DefaultObjectPool<ConstructorDescriptor> _constructorPool
        = new (new DefaultPooledObjectPolicy<ConstructorDescriptor>());

    private readonly DefaultObjectPool<MemberDescriptor> _memberPool
        = new (new DefaultPooledObjectPolicy<MemberDescriptor>());

    private readonly DefaultObjectPool<MethodDescriptor> _methodPool
        = new (new DefaultPooledObjectPolicy<MethodDescriptor>());

    private readonly DefaultObjectPool<TypeDescriptor> _typePool
        = new (new DefaultPooledObjectPolicy<TypeDescriptor>());

    #endregion

    #region Public methods

    /// <summary>
    /// Обнуление кешей.
    /// </summary>
    public void Clear()
    {
        _typeCache.Clear();
        _constructorCache.Clear();
        _methodCache.Clear();
        _memberCache.Clear();
    }
    
    /// <summary>
    /// Разрешение конструктора по его аргументам.
    /// </summary>
    public ConstructorInfo? ResolveConstructor
        (
            Type type,
            Type[]? arguments
        )
    {
        Sure.NotNull (type);

        var descriptor = _constructorPool.Get();
        descriptor.Type = type;
        descriptor.Arguments = arguments;
        var result = ResolveConstructor (descriptor);
        _constructorPool.Return (descriptor);

        return result;
    }

    /// <summary>
    /// Разрешение свойства или поля по его имени.
    /// </summary>
    public PropertyOrField? ResolveMember
        (
            Type type,
            string name
        )
    {
        Sure.NotNull (type);
        Sure.NotNullNorEmpty (name);

        var descriptor = _memberPool.Get();
        descriptor.Type = type;
        descriptor.Name = name;
        var result = ResolveMember (descriptor);
        _memberPool.Return (descriptor);

        return result;
    }

    /// <summary>
    /// Разрешение метода по его имени.
    /// </summary>
    public MethodInfo? ResolveMethod
        (
            Type type,
            string name,
            Type[]? arguments
        )
    {
        Sure.NotNull (type);
        Sure.NotNullNorEmpty (name);

        var descriptor = _methodPool.Get();
        descriptor.Type = type;
        descriptor.Name = name;
        descriptor.Arguments = arguments;
        var result = ResolveMethod (descriptor);
        _methodPool.Return (descriptor);

        return result;
    }

    /// <summary>
    /// Разрешение типа по его имени.
    /// </summary>
    public Type? ResolveType
        (
            string typeName,
            string[]? genericParameters
        )
    {
        Sure.NotNullNorEmpty (typeName);

        var descriptor = _typePool.Get();
        descriptor.TypeName = typeName;
        descriptor.GenericParameters = genericParameters;
        var result = ResolveType (descriptor);
        _typePool.Return (descriptor);

        return result;
    }

    #endregion

    #region IResolver members

    /// <inheritdoc cref="NaiveResolver.ResolveConstructor"/>
    public override ConstructorInfo? ResolveConstructor
        (
            ConstructorDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        if (!_constructorCache.TryGetConstructor (descriptor, out var result))
        {
            result = base.ResolveConstructor (descriptor);
            if (result is not null)
            {
                _constructorCache.Add (descriptor, result);
            }
        }

        return result;
    }

    /// <inheritdoc cref="NaiveResolver.ResolveMember"/>
    public override PropertyOrField? ResolveMember
        (
            MemberDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        if (!_memberCache.TryGetProperty (descriptor, out var result))
        {
            result = base.ResolveMember (descriptor);
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
            MethodDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        if (!_methodCache.TryGetMethod (descriptor, out var result))
        {
            result = base.ResolveMethod (descriptor);
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
