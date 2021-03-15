// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnassignedGetOnlyAutoProperty

/* PftFormatter.cs -- PFT-форматтер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Client;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Временная заглушка для форматтера
    /// </summary>
    public class PftFormatter
        : IPftFormatter
    {
        #region IPftFormatter members

        public bool SupportsExtendedSyntax { get; }
        public object Context { get; }
        public string FormatRecord(Record? record) =>
            throw new System.NotImplementedException();

        public string FormatRecord(int mfn) =>
            throw new System.NotImplementedException();

        public string[] FormatRecords(int[] mfns) =>
            throw new System.NotImplementedException();

        public void ParseProgram(string source) =>
            throw new System.NotImplementedException();

        public void SetProvider(IrbisProvider contextProvider) =>
            throw new System.NotImplementedException();

        #endregion

        #region IDisposable members

        public void Dispose() => throw new System.NotImplementedException();

        #endregion

    } // class IPftFormatter

} // namespace ManagedIrbis.Pft
