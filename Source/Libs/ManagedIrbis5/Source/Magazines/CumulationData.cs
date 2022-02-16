// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global

/* CumulationData.cs -- данные, используемые при кумуляции
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Данные используемые при кумуляции.
/// Относятся к одному экземпляру журнала/газеты.
/// </summary>
sealed class CumulationData
{
    /// <summary>
    /// Год.
    /// </summary>
    public string? Year { get; set; }

    /// <summary>
    /// Том.
    /// </summary>
    public string? Volume { get; set; }

    /// <summary>
    /// Номер выпуска.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Место хранения экземпляра.
    /// </summary>
    public string? Place { get; set; }
}