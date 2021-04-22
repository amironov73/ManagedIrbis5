// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

/* PftFormatter.cs -- PFT-форматтер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Временная заглушка для форматтера
    /// </summary>
    public class PftFormatter
        : IPftFormatter
    {
        #region Construction

        public PftFormatter()
        {
        }

        public PftFormatter
            (
                object context
            )
        {
            Context = context;
        }

        #endregion

        #region IPftFormatter members

        /// <inheritdoc cref="IPftFormatter.Program"/>
        public PftProgram? Program { get; set; }

        public virtual bool SupportsExtendedSyntax { get; }

        public object? Context { get; }

        public virtual string FormatRecord(Record? record) =>
            throw new System.NotImplementedException();

        public virtual string FormatRecord(int mfn) =>
            throw new System.NotImplementedException();

        public virtual string[] FormatRecords(int[] mfns) =>
            throw new System.NotImplementedException();

        public virtual void ParseProgram(string source) =>
            throw new System.NotImplementedException();

        public virtual void SetProvider(ISyncIrbisProvider contextProvider) =>
            throw new System.NotImplementedException();

        #endregion

        #region IDisposable members

        public virtual void Dispose() => throw new System.NotImplementedException();

        #endregion

    } // class PftFormatter

} // namespace ManagedIrbis.Pft
