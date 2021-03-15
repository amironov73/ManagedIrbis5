// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ReportAttributes.cs -- словарь атрибутов для ячеек, полос и отчета в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Словарь атрибутов для ячеек, полос и отчета в целом.
    /// </summary>
    public sealed class ReportAttributes
        : Dictionary<string, object>,
        IVerifiable
    {
        #region Public methods

        /// <summary>
        /// Get the attribute value by name.
        /// </summary>
        public object? GetAttribute
            (
                string name
            )
        {
            TryGetValue(name, out object? result);

            return result;
        } // method GetAttribute

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<ReportAttributes>(this, throwOnError);

            // TODO Add some verification

            return verifier.Result;
        } // method Verify

        #endregion

    } // class ReportAttibutes

} // namespace ManagedIrbis.Reports
