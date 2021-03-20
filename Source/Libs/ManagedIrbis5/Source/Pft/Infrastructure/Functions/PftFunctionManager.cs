// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftFunctionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Function manager.
    /// </summary>
    public sealed class PftFunctionManager
    {
        #region Properties

        /// <summary>
        /// Function registry.
        /// </summary>
        public Dictionary<string, FunctionDescriptor> Registry { get; }
            = new(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Builtin functions.
        /// </summary>
        public static PftFunctionManager BuiltinFunctions { get; } = new();

        /// <summary>
        /// User defined functions.
        /// </summary>
        public static PftFunctionManager UserFunctions { get; } = new();

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

            Registry.Add(name, descriptor);

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
                    Magna.Error
                        (
                            "PftFunctionManager::ExecuteFunction: "
                            + "unknown function="
                            + name.ToVisibleString()
                        );

                    throw new PftSemanticException
                        (
                            "unknown function: "
                            + name
                        );
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
            Registry.TryGetValue(name, out FunctionDescriptor? result);

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
            return Registry.ContainsKey(name);
        }

        /// <summary>
        /// Register the function.
        /// </summary>
        public void RegisterFunction
            (
                string name,
                PftFunction function
            )
        {
            if (name.IsOneOf(PftUtility.GetReservedWords()))
            {
                Magna.Error
                    (
                        "PftFunctionManager::RegisterFunction: "
                        + "reserved word="
                        + name.ToVisibleString()
                    );

                throw new PftException("Reserved word: " + name);
            }

            if (HaveFunction(name))
            {
                Magna.Error
                    (
                        "PftFunctionManager::RegisterFunction: "
                        + "already registered: "
                        + name.ToVisibleString()
                    );

                throw new PftException
                    (
                        "Function already registered: " + name
                    );
            }

            Add(name, function);
        }

        #endregion
    }
}
