// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* IPftFormatter.cs -- интерфейс форматтера PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Интерфейс форматтера PFT.
    /// </summary>
    public interface IPftFormatter
        : IDisposable
    {
        /// <summary>
        /// Whether the formatter supports the extended syntax.
        /// </summary>
        bool SupportsExtendedSyntax { get; }

        /// <summary>
        /// Format the record.
        /// </summary>
        string FormatRecord
            (
                Record? record
            );

        /// <summary>
        /// Format the record.
        /// </summary>
        string FormatRecord
            (
                int mfn
            );

        /// <summary>
        /// Format some records.
        /// </summary>
        string[] FormatRecords
            (
                int[] mfns
            );

        /// <summary>
        /// Parse the program.
        /// </summary>
        void ParseProgram
            (
                string source
            );

        /// <summary>
        /// Установка провайдера.
        /// </summary>
        void SetProvider(IrbisProvider contextProvider);

    } // interface IPftFormatter

} // namespace ManagedIrbis.Pft
