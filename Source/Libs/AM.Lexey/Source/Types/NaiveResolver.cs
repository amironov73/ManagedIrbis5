// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NaiveResolver.cs -- простейший наивный резольвер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using AM.Lexey.Barsik;
using AM.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Types;

/// <summary>
/// Простейший наивный резольвер типов.
/// </summary>
[PublicAPI]
public class NaiveResolver
    : IResolver
{
    #region Properties

    /// <inheritdoc cref="IResolver.Assemblies"/>
    public HashSet<Assembly> Assemblies { get; } = new ();

    /// <inheritdoc cref="IResolver.Namespaces"/>
    public HashSet<string> Namespaces { get; } = new ();

    #endregion

    #region Private members

    /// <summary>
    /// Конструирование типа, если необходимо.
    /// </summary>
    private static Type ConstructType
        (
            Type mainType,
            Type[]? parameters
        )
    {
        if (parameters is null)
        {
            return mainType;
        }

        if (!mainType.IsGenericType)
        {
            // TODO более осмысленная реакция
            return mainType;
        }

        return mainType.MakeGenericType (parameters);
    }

    #endregion

    #region IResolver members

    /// <inheritdoc cref="IResolver.Reset"/>
    public virtual void Reset()
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IResolver.ResolveConstructor"/>
    public virtual ConstructorInfo? ResolveConstructor
        (
            ConstructorDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        var arguments = descriptor.Arguments ?? Array.Empty<Type>();
        var type = descriptor.Type.ThrowIfNull();
        return type.GetConstructor (BindingFlags.Public|BindingFlags.Instance, arguments);
    }

    /// <inheritdoc cref="IResolver.ResolveMember"/>
    public virtual PropertyOrField? ResolveMember
        (
            MemberDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        var type = descriptor.Type.ThrowIfNull();
        var name = descriptor.Name.ThrowIfNullOrEmpty();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static
            | BindingFlags.Public | BindingFlags.NonPublic;
        const MemberTypes memberTypes = MemberTypes.Field | MemberTypes.Property;
        var members = type.GetMember (name, memberTypes, flags);
        if (members.Length != 1)
        {
            return null;
        }

        return new PropertyOrField (members[0]);
    }

    /// <inheritdoc cref="IResolver.ResolveMethod"/>
    public virtual MethodInfo? ResolveMethod
        (
            MethodDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        var type = descriptor.Type.ThrowIfNull();
        var name = descriptor.Name.ThrowIfNullOrEmpty();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static
            | BindingFlags.Public | BindingFlags.NonPublic;
        var arguments = descriptor.Arguments ?? Array.Empty<Type>();
        var result = type.GetMethod
            (
                name,
                flags,
                arguments
            );
        result ??= type.GetMethod (name, flags);

        return result;
    }

    /// <inheritdoc cref="IResolver.ResolveType"/>
    public virtual Type? ResolveType
        (
            TypeDescriptor descriptor
        )
    {
        Sure.NotNull (descriptor);

        var typeName = descriptor.TypeName.ThrowIfNullOrEmpty();
        switch (typeName)
        {
            case "bool": return typeof (bool);
            case "byte": return typeof (byte);
            case "sbyte": return typeof (sbyte);
            case "short": return typeof (short);
            case "char": return typeof (char);
            case "ushort": return typeof (ushort);
            case "int": return typeof (int);
            case "uint": return typeof (uint);
            case "long": return typeof (long);
            case "ulong": return typeof (ulong);
            case "decimal": return typeof (decimal);
            case "float": return typeof (float);
            case "double": return typeof (double);
            case "object": return typeof (object);
            case "string": return typeof (string);

            // наши псевдо-типы
            case "list": return typeof (BarsikList);
            case "dict": return typeof (BarsikDictionary);
        }

        var stringParameters = descriptor.GenericParameters;
        Type[]? parameters = null;
        if (stringParameters is not null)
        {
            var list = new List<Type>();
            foreach (var oneArgument in stringParameters)
            {
                // TODO обеспеченность вложенные generic
                var oneDescriptor = new TypeDescriptor
                {
                    TypeName = oneArgument
                };
                var foundType = ResolveType (oneDescriptor);
                if (foundType is null)
                {
                    // TODO более осмысленная реакция на ошибку
                    return null;
                }

                list.Add (foundType);
            }

            parameters = list.ToArray();
        }

        if (parameters is not null && !typeName.Contains ('`'))
        {
            // TODO разбирать на имя типа и сборку
            typeName += $"`{parameters.Length}";
        }

        var result = Type.GetType (typeName, false);
        if (result is not null)
        {
            return ConstructType (result, parameters);
        }

        if (!typeName.Contains ('.'))
        {
            // это не полное имя, так что попробуем приписать к нему
            // различные пространства имен
            foreach (var ns in Namespaces)
            {
                var fullName = ns + "." + typeName;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, parameters);
                }
            }
        }

        if (!typeName.Contains (','))
        {
            // это не assembly-qualified name, так что попробуем
            // приписать к нему загруженные нами сборки
            foreach (var asm in Assemblies)
            {
                var asmName = asm.GetName().Name;
                var fullName = typeName + ", " + asmName;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, parameters);
                }

                if (!typeName.Contains ('.'))
                {
                    // это не полное имя, так что попробуем приписать к нему
                    // различные пространства имен
                    foreach (var ns in Namespaces)
                    {
                        fullName = ns + "." + typeName + ", " + asmName;
                        result = Type.GetType (fullName, false);
                        if (result is not null)
                        {
                            return ConstructType (result, parameters);
                        }
                    }
                }
            }
        }

        return null;
    }

    #endregion
}
