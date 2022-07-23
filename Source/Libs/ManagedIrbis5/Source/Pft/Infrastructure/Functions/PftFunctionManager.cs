// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftFunctionManager.cs -- менеджер функций для PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Менеджер функций для PFT.
/// </summary>
public sealed class PftFunctionManager
{
    #region Properties

    /// <summary>
    /// Реестр функций.
    /// </summary>
    public Dictionary<string, FunctionDescriptor> Registry { get; }
        = new (StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Встроенные функции.
    /// </summary>
    public static PftFunctionManager BuiltinFunctions { get; } = new ();

    /// <summary>
    /// Функции, определенные пользователем.
    /// </summary>
    public static PftFunctionManager UserFunctions { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Static constructor.
    /// </summary>
    static PftFunctionManager()
    {
        StandardFunctions.Register();
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Quick add the function.
    /// </summary>
    public PftFunctionManager Add
        (
            string name,
            PftFunction function
        )
    {
        var descriptor = new FunctionDescriptor
        {
            Name = name,
            Function = function
        };

        Registry.Add (name, descriptor);

        return this;
    }

    /// <summary>
    /// Execute specified function.
    /// </summary>
    public static void ExecuteFunction
        (
            string name,
            PftContext context,
            PftNode node,
            PftNode[] arguments
        )
    {
        Sure.NotNullNorEmpty (name);
        Sure.NotNull (context);
        Sure.NotNull (node);

        if (!UserFunctions.Registry.TryGetValue
                (
                    name,
                    out FunctionDescriptor? descriptor
                ))
        {
            if (!BuiltinFunctions.Registry.TryGetValue
                    (
                        name,
                        out descriptor
                    ))
            {
                Magna.Logger.LogError
                    (
                        nameof (PftFunctionManager) + "::" + nameof (ExecuteFunction)
                        + ": unknown function {Name}",
                        name
                    );

                throw new PftSemanticException ("unknown function: " + name);
            }
        }

        descriptor.Function?.Invoke
            (
                context,
                node,
                arguments
            );
    }

    /// <summary>
    /// Find specified function.
    /// </summary>
    public FunctionDescriptor? FindFunction
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Registry.TryGetValue (name, out FunctionDescriptor? result);

        return result;
    }

    /// <summary>
    /// Have specified function?
    /// </summary>
    public bool HaveFunction
        (
            string name
        )
    {
        return Registry.ContainsKey (name);
    }

    /// <summary>
    /// Регистрация функции.
    /// </summary>
    public void RegisterFunction
        (
            string name,
            PftFunction function
        )
    {
        Sure.NotNullNorEmpty (name);
        Sure.NotNull (function);

        if (name.IsOneOf (PftUtility.GetReservedWords()))
        {
            Magna.Logger.LogError
                (
                    nameof (PftFunctionManager) + "::" + nameof (RegisterFunction)
                    + ": reserved word {Word}",
                    name.ToVisibleString()
                );

            throw new PftException ("Reserved word: " + name);
        }

        if (HaveFunction (name))
        {
            Magna.Logger.LogError
                (
                    nameof (PftFunctionManager) + "::" + nameof (RegisterFunction)
                    + ": already registered {Name}",
                    name.ToVisibleString()
                );

            throw new PftException
                (
                    "Function already registered: " + name
                );
        }

        Add (name, function);
    }

    #endregion
}
