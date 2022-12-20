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

using System.Collections.Generic;

namespace AM.Avalonia.General;

/// <summary>
/// Список элементов пользовательского интерфейса.
/// Например, дочерние элементы некого родительского элемента.
/// </summary>
public interface IGeneralItemList
    : IList<IGeneralItem>
{
    // пустое тело интерфейса
}
