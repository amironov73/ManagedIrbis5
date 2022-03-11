// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ExemplarStatus.cs -- коды для статуса экземпляра книги/журнала/газеты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Коды для статуса экземпляра книги/журнала/газеты.
/// </summary>
public static class ExemplarStatus
{
    #region Constants

    /// <summary>
    /// Отдельный экземпляр (на индивидуальном учете)
    /// свободен и доступен для выдачи.
    /// </summary>
    [Description ("Доступен")] public const string Free = "0";

    /// <summary>
    /// Экземпляр выдан читателю.
    /// </summary>
    [Description ("Выдан читателю")] public const string Loan = "1";

    /// <summary>
    /// Данный экземпляр ещё не поступил в библиотеку, ожидается.
    /// </summary>
    [Description ("Ожидается")] public const string Wait = "2";

    /// <summary>
    /// Находится в переплетной мастерской.
    /// </summary>
    [Description ("Находится в переплетной мастерской")]
    public const string Bindery = "3";

    /// <summary>
    /// Экземпляр утерян.
    /// </summary>
    [Description ("Экземпляр утерян")] public const string Lost = "4";

    /// <summary>
    /// Временно не выдается.
    /// </summary>
    [Description ("Временно не выдается")] public const string NotAvailable = "5";

    /// <summary>
    /// Экземпляр списан.
    /// </summary>
    [Description ("Экземпляр списан")] public const string WrittenOff = "6";

    /// <summary>
    /// Номер журнала/газеты поступил, но еще не дошел до места хранения.
    /// </summary>
    [Description ("Номер журнала/газеты поступил, но еще не дошел до места хранения")]
    public const string OnTheWay = "8";

    /// <summary>
    /// Экземпляр на бронеполке.
    /// </summary>
    [Description ("Экземпляр на бронеполке")]
    public const string Reserved = "9";

    /// <summary>
    /// Группа экземпляров для библиотеки сети.
    /// </summary>
    [Description ("Группа экземпляров для библиотеки сети")]
    public const string BiblioNet = "C";

    /// <summary>
    /// Номер журнала/газеты переплетен (входит в подшивку).
    /// </summary>
    [Description ("Номер журнала/газеты переплетен")]
    public const string Bound = "P";

    /// <summary>
    /// Группа экземпляров на размножение с вводом инвентарных номеров.
    /// </summary>
    [Description ("Группа экземпляров на размножение")]
    public const string Reproduction = "R";

    /// <summary>
    /// Группа экземпляров безинвентарного учета.
    /// </summary>
    [Description ("Группа экземпляров беинвентарного учета")]
    public const string Summary = "U";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение массива значение констант.
    /// </summary>
    public static string[] ListValues()
    {
        return ReflectionUtility.ListConstantValues<string> (typeof (ExemplarStatus));
    }

    /// <summary>
    /// Получение словаря "код" - "значение" для статусов экземпляров.
    /// </summary>
    public static Dictionary<string, string> ListValuesWithDescriptions()
    {
        return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (ExemplarStatus));
    }

    #endregion
}
