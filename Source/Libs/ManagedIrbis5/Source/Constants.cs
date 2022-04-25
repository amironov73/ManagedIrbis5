// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Constants.cs -- общие для ИРБИС64 константы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Общие для ИРБИС64 константы.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Constants
{
    #region Constants

    /// <summary>
    /// Имя файла, содержащего список баз данных для администратора.
    /// </summary>
    public const string AdministratorDatabaseList = "dbnam1.mnu";

    /// <summary>
    /// Имя файла, содержащего список баз данных для каталогизатора.
    /// </summary>
    public const string CatalogerDatabaseList = "dbnam2.mnu";

    /// <summary>
    /// Максимальная длина (размер полки) - ограничение формата.
    /// </summary>
    public const int MaxRecord = 32000;

    /// <summary>
    /// Максимальное количество постингов в пакете.
    /// </summary>
    public const int MaxPostings = 32758;

    /// <summary>
    /// Имя файла, содержащего список баз данных для читателя.
    /// </summary>
    public const string ReaderDatabaseList = "dbnam3.mnu";

    /// <summary>
    /// Рабочий лист "ASP" для статей из сборников и журналов/газет.
    /// </summary>
    public const string Asp = "ASP";

    /// <summary>
    /// Рабочий лист "AUNTD" для аналилическго описания юридического документа или НТД.
    /// </summary>
    public const string Auntd = "AUNTD";

    /// <summary>
    /// Рабочий лист "IBIS" для упрощенного библиографического описания книги.
    /// </summary>
    public const string Ibis = "IBIS";

    /// <summary>
    /// Рабочий лист "MUSP" для описания музейного предмета.
    /// </summary>
    public const string Musp = "MUSP";

    /// <summary>
    /// Рабочий лист "NJ" для описания отдельного выпуска журнала/газеты.
    /// </summary>
    public const string Nj = "NJ";

    /// <summary>
    /// Рабочий лист "OJ" для сводного описания журнала/газеты.
    /// </summary>
    public const string Oj = "OJ";

    /// <summary>
    /// Рабочий лист "PAZK" для описания книги под автором, заглавием или коллективом.
    /// </summary>
    public const string Pazk = "PAZK";

    /// <summary>
    /// Рабочий лист "PRF" для проверки фонда.
    /// </summary>
    public const string Prf = "PRF";

    /// <summary>
    /// Рабочий лист "PVK" для описания под временным коллективом
    /// (труды конференций и т. п.).
    /// </summary>
    public const string Pvk = "PVK";

    /// <summary>
    /// Рабочий лист "SPEC" для описания спецификации многотомника.
    /// </summary>
    public const string Spec = "SPEC";

    #endregion
}
