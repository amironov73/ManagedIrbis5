// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IPoolingEnumerator_T.cs -- интерфейс типизированного перечислителя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Memory.Collections;

/// <summary>
/// Интерфейс типизированного перечислителя.
/// </summary>
/// <typeparam name="T">Тип перечисляемых элементов.</typeparam>
public interface IPoolingEnumerator<out T>
    : IPoolingEnumerator
{
    /// <inheritdoc cref="IEnumerator{T}.Current"/>
    new T Current { get; }
}
