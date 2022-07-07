// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CumulationMethod.cs -- метод кумуляции выпусков журналов/газет
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Magazines;

/// <summary>
/// Метод кумуляции выпусков журналов/газет.
/// </summary>
public enum CumulationMethod
{
    /// <summary>
    /// Только по годам (и, конечно же, по томам).
    /// </summary>
    Year,

    /// <summary>
    /// По годам и месту хранения.
    /// </summary>
    Place,

    /// <summary>
    /// По годам, месту хранения и номеру комплекта.
    /// </summary>
    Complect
}
