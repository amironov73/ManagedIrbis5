// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CompilerUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    public static class CompilerUtility
    {
        #region Public methods

        /// <summary>
        /// Convert boolean to string according C# rules.
        /// </summary>
        public static string BooleanToText
            (
                bool value
            )
        {
            return value ? "true" : "false";
        }

        /// <summary>
        /// Escape the text according C# rules.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string? Escape
            (
                string? text
            )
        {
            // TODO implement properly

            return text;
        }

        /// <summary>
        /// Find entry point of the assembly.
        /// </summary>
        public static Func<PftContext, PftPacket> GetEntryPoint
            (
                Assembly assembly
            )
        {
            Type[] types = assembly.GetTypes();
            if (types.Length != 1)
            {
                throw new PftCompilerException();
            }
            Type type = types[0];
            if (!type.IsSubclassOf(typeof(PftPacket)))
            {
                throw new PftCompilerException();
            }

            var method = type.GetMethod
                (
                    "CreateInstance",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new [] { typeof(PftContext) },
                    null
                )
                .ThrowIfNull("type.GetMethod");

            var result = (Func<PftContext, PftPacket>)
                Delegate.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>),
                        method,
                        true
                    )
                    .ThrowIfNull("CreateDelegate");

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

            return result;
        }

        /// <summary>
        /// Shorten the text.
        /// </summary>
        public static string ShortenText
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string result = text.Replace('\r', ' ')
                .Replace('\n', ' ')
                .SafeSubstring(0, 25)
                .IfEmpty(string.Empty);

            return result;
        }

        #endregion
    }
}
