// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CompilerProxy.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// Хак для загрузки сборки в другом домене.
    /// </summary>
    /// <remarks>
    /// Borrowed from https://stackoverflow.com/questions/658498/how-to-load-an-assembly-to-appdomain-with-all-references-recursively
    /// </remarks>
    public sealed class CompilerProxy
        : MarshalByRefObject
    {
        #region Public methods

        /// <summary>
        /// Load the assembly.
        /// </summary>
        public Assembly LoadAssembly
            (
                string assemblyPath
            )
        {
            Assembly result = Assembly.LoadFile(assemblyPath);

            return result;
        }

        #endregion
    }
}

