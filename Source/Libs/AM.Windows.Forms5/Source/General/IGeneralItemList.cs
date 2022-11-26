// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IGeneralItemList.cs -- список элементов пользовательского интерфейса
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms.General;

/// <summary>
/// Список элементов пользовательского интерфейса.
/// Например, дочерние элементы некого родительского элемента.
/// </summary>
public interface IGeneralItemList
{
    /// <summary>
    /// Количество элементов в списке.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Поиск элемента по индексу.
    /// </summary>
    IGeneralItem this [int index] { get; }

    /// <summary>
    /// Поиск элемента по идентификатору.
    /// </summary>
    IGeneralItem? this [string id] { get; }

    /// <summary>
    /// Добавление элемента в список.
    /// </summary>
    void Add (IGeneralItem item);

    /// <summary>
    /// Удаление всех элементов из списка.
    /// </summary>
    void Clear();

    /// <summary>
    /// Создание элемента и добавление его в контейнер.
    /// </summary>
    IGeneralItem CreateItem (string id, string caption);

    /// <summary>
    /// Проверка присутствия элемента в списке.
    /// </summary>
    bool Contains (IGeneralItem item);

    /// <summary>
    /// Удаление элемента из списка.
    /// </summary>
    void Remove (IGeneralItem item);
}
