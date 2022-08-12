// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ISiberianRowCollection.cs -- интерфейс коллекции строк грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Интерфейс коллекции строк грида
/// </summary>
public interface ISiberianRowCollection
    : IList<SiberianRow>
{
    // пустое тело интерфейса
}
