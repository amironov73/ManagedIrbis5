// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* OperativeCommandCode.cs -- коды операций для оперативных режимов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Коды операций для оперативных режимов.
/// </summary>
public static class OperativeCommandCode
{
    #region Constants

    /// <summary>
    /// Переход от текущего документа к другому ("один к одному").
    /// </summary>
    [Description ("Переход один-к-одному")]
    public const string GotoOne = "0";

    /// <summary>
    /// Переход от текущего документа к группе документов
    /// ("один ко многим").
    /// </summary>
    [Description ("Переход один-ко-многим")]
    public const string GotoMany = "1";

    /// <summary>
    /// Формирование нового документа из текущего.
    /// </summary>
    [Description ("Формирование нового документа")]
    public const string NewDocument = "2";

    /// <summary>
    /// Глобальная корректировка текущего документа с опросом
    /// параметров.
    /// </summary>
    [Description ("Глобальная корректировка")]
    public const string GlobalCorrection = "3";

    /// <summary>
    /// Выполнение пакетного задания.
    /// </summary>
    [Description ("Выполнение пакетного задания")]
    public const string ExecuteBatchJob = "4";

    /// <summary>
    /// Выполнение режима пользователя.
    /// </summary>
    [Description ("Выполнение режима пользователя")]
    public const string ExecuteDll = "5";

    /// <summary>
    /// Переход от текущего документа к другому ("один к одному").
    /// </summary>
    [Description ("Переход один-к-одному (2)")]
    public const string GotoOne2 = "10";

    /// <summary>
    /// Переход от текущего документа к группе документов
    /// ("один ко многим").
    /// </summary>
    [Description ("Переход один-ко-многим (2)")]
    public const string GotoMany2 = "11";

    /// <summary>
    /// Эмуляция режима "Регистрация периодики".
    /// </summary>
    [Description ("Регистрация периодики")]
    public const string MagazineRegistration = "100";

    /// <summary>
    /// Эмуляция режима "Номера".
    /// </summary>
    [Description ("Список номеров")]
    public const string ListIssues = "101";

    /// <summary>
    /// Эмуляция режима "Формирование подшивки".
    /// </summary>
    [Description ("Формирование подшивки")]
    public const string BindMagazines = "102";

    /// <summary>
    /// Эмуляция режима "Сводный".
    /// </summary>
    [Description ("Переход к сводному описанию")]
    public const string GotoSummaryRecord = "103";

    /// <summary>
    /// Эмуляция режима "Статьи".
    /// </summary>
    [Description ("Список статей")]
    public const string ShowArticles = "104";

    /// <summary>
    /// Эмуляция режима "Другие номера".
    /// </summary>
    [Description ("Список других номеров")]
    public const string ListOtherIssues = "105";

    /// <summary>
    /// Эмуляция режима "Новая статья".
    /// </summary>
    [Description ("Новая статья")]
    public const string NewArticle = "106";

    /// <summary>
    /// Эмуляция режима "Номера подшивки".
    /// </summary>
    [Description ("Номера подшивки")]
    public const string ShowBoundIssues = "107";

    /// <summary>
    /// Эмуляция режима "Источник"
    /// </summary>
    [Description ("Переход к источнику")]
    public const string GotoSource = "108";

    /// <summary>
    /// Эмуляция режима "Другие статьи".
    /// </summary>
    [Description ("Другие статьи")]
    public const string ListOtherArticles = "109";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение массива значение констант.
    /// </summary>
    public static string[] ListValues()
    {
        return ReflectionUtility.ListConstantValues<string> (typeof (OperativeCommandCode));
    }

    #endregion
}
