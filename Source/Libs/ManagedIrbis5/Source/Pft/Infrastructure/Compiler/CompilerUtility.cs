// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CompilerUtility.cs -- вспомогательные методы для PFT-компилятора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// Вспомогательные методы для PFT-компилятора.
    /// </summary>
    public static class CompilerUtility
    {
        #region Public methods

        /// <summary>
        /// Convert boolean to string according C# rules.
        /// </summary>
        public static string BooleanToText (bool value) => value ? "true" : "false";

        /// <summary>
        /// Escape the text according C# rules.
        /// </summary>
        public static string? Escape
            (
                string? text
            )
        {
            // TODO implement properly

            return text;
        } // method Escape

        /// <summary>
        /// Find entry point of the assembly.
        /// </summary>
        public static Func<PftContext, PftPacket> GetEntryPoint
            (
                Assembly assembly
            )
        {
            var types = assembly.GetTypes();
            if (types.Length != 1)
            {
                throw new PftCompilerException();
            }

            var type = types[0];
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

            var result = (Func<PftContext, PftPacket>) Delegate.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>),
                        method,
                        true
                    )
                    .ThrowIfNull("CreateDelegate");

            return result;
        } // method GetEntryPoint

        /// <summary>
        /// Укорачиваем текст, чтобы комментарии были не слишком длинными.
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

            var result = text.Replace('\r', ' ')
                .Replace('\n', ' ')
                .SafeSubstring(0, 25)
                ?? string.Empty;

            return result;
        } // method ShortenText

        #endregion

    } // class CompilerUtility

} // namespace ManagedIrbis.Pft.Infrastructure.Compiler
