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

/* LanguageContext.cs -- контекст исполнения программы-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Контекст исполнения программы-скрипта
/// </summary>
public sealed class LanguageContext
{
    #region Properties

    /// <summary>
    /// Переменные программы-скрипта.
    /// </summary>
    public Dictionary<string, Variable> Variables { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Найти или создать (если не найдено) переменную с указанным именем.
    /// </summary>
    public Variable FindOrCreateVariable
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        if (!Variables.TryGetValue (name, out var result))
        {
            result = new Variable (name);
            Variables.Add (name, result);
        }

        return result;
    }

    /// <summary>
    /// Требование существования переменной.
    /// </summary>
    public Variable RequireVariable
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        if (!Variables.TryGetValue (name, out var result))
        {
            throw new LanguageException ($"Variable {name} doesn't exist");
        }

        return result;
    }

    #endregion
}
