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
#if CROSSPLATFORM || COREWIN
using AM.Reporting.Code.CodeDom.Compiler;
using AM.Reporting.Code.CSharp;
#else
using System.CodeDom.Compiler;

using Microsoft.CSharp;
#endif
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Class helper for compile source code with path of assemblies
    /// </summary>
    public static class CompileHelper
    {
        /// <summary>
        /// Generate a assembly in memory with some source code and several path for additional assemblies
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="assemblyPaths"></param>
        /// <returns></returns>
        public static Assembly GenerateAssemblyInMemory (string sourceCode, params string[] assemblyPaths)
        {
            using (var compiler = new CSharpCodeProvider())
            {
                var parameters = new CompilerParameters
                {
                    GenerateInMemory = true
                };

                foreach (var asm in assemblyPaths)
                {
                    parameters.ReferencedAssemblies.Add (asm);
                }

#if CROSSPLATFORM || COREWIN
                var mscorPath = compiler.GetReference("System.Private.CoreLib.dll").Display;
                parameters.ReferencedAssemblies.Add(mscorPath);
#endif

                var results = compiler.CompileAssemblyFromSource (parameters, sourceCode);
                return results.CompiledAssembly;
            }
        }
    }
}
