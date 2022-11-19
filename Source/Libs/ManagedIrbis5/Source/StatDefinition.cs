// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StatDefinition.cs -- параметры для команды Stat
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Параметры для команды Stat.
/// </summary>
public sealed class StatDefinition
{
    #region Nested classes

    /// <summary>
    /// Метод сортировки.
    /// </summary>
    public enum SortMethod
    {
        /// <summary>
        /// Не сортировать.
        /// </summary>
        None = 0,

        /// <summary>
        /// В порядке возрастания.
        /// </summary>
        Ascending = 1,

        /// <summary>
        /// В порядке убывания.
        /// </summary>
        Descending = 2
    }

    /// <summary>
    /// Элемент статистики.
    /// </summary>
    public sealed class Item
    {
        #region Properties

        /// <summary>
        /// Спецификация оля записи (возможно, с подполем).
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// Максимальная длина записи (длинные строки усекаются).
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Количество отбираемых элементов.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Метод сортировки.
        /// </summary>
        public SortMethod Sort { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Field},{Length},{Count},{(int)Sort}";

        #endregion
    }

    #endregion

    #region Properties

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string? DatabaseName { get; set; }

    /// <summary>
    /// Элементы, по которым собирается статистика.
    /// </summary>
    public List<Item> Items { get; } = new ();

    /// <summary>
    /// Спецификация поисковго запроса для поиска по словарю.
    /// </summary>
    public string? SearchQuery { get; set; }

    /// <summary>
    /// Минимальный MFN.
    /// </summary>
    public int MinMfn { get; set; }

    /// <summary>
    /// Максимальный MFN.
    /// </summary>
    public int MaxMfn { get; set; }

    /// <summary>
    /// Спецификация для последовательного поиска (опционально).
    /// </summary>
    public string? SequentialQuery { get; set; }

    /// <summary>
    /// Список MFN.
    /// </summary>
    public List<int> MfnList { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование в пользовательский запрос.
    /// </summary>
    public void Encode<T>
        (
            IIrbisProvider connection,
            T query
        )
        where T : IQuery
    {
        // "2"               STAT
        // "IBIS"            database
        // "v200^a,10,100,1" field
        // "T=A$"            search
        // "0"               min
        // "0"               max
        // ""                sequential
        // ""                mfn list

        var items = string.Join (IrbisText.IrbisDelimiter, Items);
        var mfns = string.Join (",", MfnList);
        query.AddAnsi (connection.EnsureDatabase (DatabaseName));
        query.AddAnsi (items);
        query.AddUtf (SearchQuery);
        query.Add (MinMfn);
        query.Add (MaxMfn);
        query.AddUtf (SequentialQuery);

        // TODO: реализовать список MFN
        query.AddAnsi (mfns);
    }

    #endregion
}
