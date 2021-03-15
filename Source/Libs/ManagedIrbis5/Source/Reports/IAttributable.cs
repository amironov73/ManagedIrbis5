// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IAttributable.cs -- объект, снабженный атрибутами
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Объект, снабженный атрибутами.
    /// </summary>
    public interface IAttributable
    {
        /// <summary>
        /// Словарь атрибутов.
        /// </summary>
        ReportAttributes Attributes { get; }

    } // interface IAttributable

} // namespace ManagedIrbis.Reports
