// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StandardFunctions.Objects.cs --
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
    static partial class StandardFunctions
    {
        #region Private members

        private static readonly Dictionary<string, OuterObject> _objects
            = new (StringComparer.CurrentCultureIgnoreCase);

        //================================================================
        // INTERNAL METHODS
        //================================================================

        /// <summary>
        /// Create object of given type.
        /// </summary>
        internal static OuterObject CreateObject
            (
                string className
            )
        {
            var type = Type.GetType(className, true, true)
                .ThrowIfNull("Type.GetType");
            var name = Guid.NewGuid().ToString("N");
            var result = (OuterObject) Activator.CreateInstance(type, name)
                .ThrowIfNull("Activator.CreateInstance");
            result.IncreaseCounter();
            RegisterObject(result);

            return result;
        }

        /// <summary>
        /// Get object by the name.
        /// </summary>
        internal static OuterObject? GetObject
            (
                string name
            )
        {
            _objects.TryGetValue(name, out var result);

            return result;
        }

        internal static void RegisterObject
            (
                OuterObject obj
            )
        {
            _objects.Add(obj.Name, obj);
        }

        internal static void UnregisterObject
            (
                string name
            )
        {
            _objects.Remove(name);
        }

        //================================================================
        // OBJECT ORIENTED FUNCTIONS
        //================================================================

        private static void CallObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            var objName = context.GetStringArgument(arguments, 0);
            var methodName = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(objName)
                && !string.IsNullOrEmpty(methodName))
            {
                var callParameters = new List<string>();
                for (var i = 2; i < arguments.Length; i++)
                {
                    var arg = context.GetStringArgument(arguments, i);
                    if (ReferenceEquals(arg, null))
                    {
                        break;
                    }
                    callParameters.Add(arg);
                }

                var obj = GetObject(objName);
                if (!ReferenceEquals(obj, null))
                {
                    obj.CallMethod
                        (
                            methodName,

                            // ReSharper disable CoVariantArrayConversion
                            callParameters.ToArray()
                            // ReSharper restore CoVariantArrayConversion
                        );
                }
            }
        }

        private static void CloseObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            var name = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(name))
            {
                var obj = GetObject(name);
                if (!ReferenceEquals(obj, null))
                {
                    if (obj.DecreaseCounter() <= 0)
                    {
                        UnregisterObject(name);
                    }
                }
            }
        }

        private static void CreateObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            var className = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(className))
            {
                var obj = CreateObject(className);

                context.Write(node, obj.Name);
            }
        }

        private static void OpenObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            var name = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(name))
            {
                var obj = GetObject(name);
                if (!ReferenceEquals(obj, null))
                {
                    obj.IncreaseCounter();

                    context.Write(node, obj.Name);
                }
            }
        }

        #endregion
    }
}
