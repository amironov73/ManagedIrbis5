// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IReportSerializable.cs -- интерфейс сериализации/десериализации объектов отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Интерфейс сериализации/десериализации объектов отчета.
/// </summary>
public interface IReportSerializable
{
    /// <summary>
    ///Сериализация объекта.
    /// </summary>
    /// <param name="writer">Поток вывода.</param>
    void Serialize (ReportWriter writer);

    /// <summary>
    /// Десериализация объекта.
    /// </summary>
    /// <param name="reader">Поток ввода.</param>
    void Deserialize (ReportReader reader);
}
