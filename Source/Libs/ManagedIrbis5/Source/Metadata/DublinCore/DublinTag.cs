// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* DublinTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Metadata.DublinCore;

/// <summary>
/// DC field tag.
/// </summary>
public static class DublinTag
{
    #region Constants

    /// <summary>
    /// Название/заглавие.
    /// </summary>
    [Description ("Название/заглавие")]
    public const string Title = "Title";

    /// <summary>
    /// Создатель/автор.
    /// </summary>
    [Description ("Создатель/автор")]
    public const string Creator = "Creator";

    /// <summary>
    /// Тема.
    /// </summary>
    [Description ("Тема")]
    public const string Subject = "Subject";

    /// <summary>
    /// Описание.
    /// </summary>
    [Description ("Описание")]
    public const string Description = "Description";

    /// <summary>
    /// Издатель.
    /// </summary>
    [Description ("Издатель")]
    public const string Publisher = "Publisher";

    /// <summary>
    /// Внесший вклад.
    /// </summary>
    [Description ("Внесший вклад")]
    public const string Contributor = "Contributor";

    /// <summary>
    /// Дата.
    /// </summary>
    [Description ("Дата")]
    public const string Date = "Date";

    /// <summary>
    /// Тип.
    /// </summary>
    [Description ("Тип")]
    public const string Type = "Type";

    /// <summary>
    /// Формат документа.
    /// </summary>
    [Description ("Формат документа")]
    public const string Format = "Format";

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Description ("Идентификатор")]
    public const string Identifier = "Identifier";

    /// <summary>
    /// Источник.
    /// </summary>
    [Description ("Источник")]
    public const string Source = "Источник";

    /// <summary>
    /// Язык.
    /// </summary>
    [Description ("Язык")]
    public const string Language = "Language";

    /// <summary>
    /// Отношения.
    /// </summary>
    [Description ("Отношения")]
    public const string Relation = "Relation";

    /// <summary>
    /// Покрытие.
    /// </summary>
    [Description ("Покрытие")]
    public const string Coverage = "Coverage";

    /// <summary>
    /// Авторские права.
    /// </summary>
    [Description ("Авторские права")]
    public const string Rights = "Rights";

    /// <summary>
    /// Аудитория (зрители).
    /// </summary>
    [Description ("Аудитория (зрители)")]
    public const string Audience = "Audience";

    /// <summary>
    /// Происхождение.
    /// </summary>
    [Description ("Происхождение")]
    public const string Provenance = "Provenance";

    /// <summary>
    /// Правообладатель.
    /// </summary>
    [Description ("Правообладатель")]
    public const string RightsHolder = "RightsHolder";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение массива значение констант.
    /// </summary>
    public static string[] ListValues()
    {
        return ReflectionUtility.ListConstantValues<string> (typeof (DublinTag));
    }

    /// <summary>
    /// Получение словаря "код" - "значение" для статусов экземпляров.
    /// </summary>
    public static Dictionary<string, string> ListValuesWithDescriptions()
    {
        return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (DublinTag));
    }

    #endregion
}
