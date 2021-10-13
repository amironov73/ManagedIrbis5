// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* ILanguageEngine.cs -- интерфейс движка исполнения скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Интерфейс движка исполнения скриптов.
    /// </summary>
    public interface ILanguageEngine
    {
        /// <summary>
        /// Исполнение программы-скрипта в указанном контексте.
        /// </summary>
        void ExecuteProgram
            (
                LanguageProgram program,
                LanguageContext context
            );

    } // interface ILanguageEngine

} // namespace SimplestLanguage
