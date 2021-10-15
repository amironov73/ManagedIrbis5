// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LanguageUtility.cs -- полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Полезные методы.
    /// </summary>
    public static class LanguageUtility
    {
        public static T CheckThat<T>
            (
                this T obj,
                Func<T, bool> predicate
            )
        {
            if (!predicate(obj))
            {
                throw new Exception();
            }

            return obj;

        } // method CheckThat

        public static T CheckThat<T, E>
            (
                this T obj,
                Func<T, bool> predicate
            )
            where E: Exception, new()
        {
            if (!predicate(obj))
            {
                throw new E();
            }

            return obj;

        } // method CheckThat

    } // class LanguageUtility

} // namespace SimplestLanguage
