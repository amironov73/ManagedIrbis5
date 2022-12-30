// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Reporting.Code
{
    internal class ExpressionDescriptor
    {
        private MethodInfo methodInfo;
        private readonly AssemblyDescriptor assembly;

        public string MethodName { get; set; }

#pragma warning disable 618
        public object Invoke (object[] parameters)
        {
            if (assembly == null || assembly.Instance == null)
            {
                return null;
            }

            if (methodInfo == null)
            {
                methodInfo = assembly.Instance.GetType().GetMethod (MethodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (methodInfo == null)
            {
                return null;
            }

            return methodInfo.Invoke (assembly.Instance, parameters);
        }
#pragma warning restore 618

        public ExpressionDescriptor (AssemblyDescriptor assembly)
        {
            this.assembly = assembly;
        }
    }
}
