// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NullProvider.cs -- null provider used for testing
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using AM.PlatformAbstraction;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Null provider used for testing.
    /// </summary>
    public sealed class NullProvider
        : IrbisProvider
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NullProvider()
        {
            Database = "IBIS";
        }

        #endregion

        public override bool Connected { get; }

        public override void Configure(string configurationString)
        {
        }

        public override PlatformAbstractionLayer PlatformAbstraction { get; set; }

        public override string ReadFile(FileSpecification file)
        {
            return string.Empty;
        }

        public override int[] Search(string expression)
        {
            return Array.Empty<int>();
        }

        public override TermLink[] ExactSearchLinks(string term)
        {
            return Array.Empty<TermLink>();
        }

        public override TermLink[] ExactSearchTrimLinks(string term, int i)
        {
            return Array.Empty<TermLink>();
        }

        public override string FormatRecord(Record record, string format)
        {
            return string.Empty;
        }

        public override int GetMaxMfn()
        {
            return 0;
        }

        public override ServerVersion GetServerVersion()
        {
            return null;
        }

        public override string[] FormatRecords(int[] mfns, string format)
        {
            return Array.Empty<string>();
        }

        public override string[] ListFiles(FileSpecification specification)
        {
            return Array.Empty<string>();
        }

        public override bool NoOp()
        {
            return true;
        }

        public override void ReleaseFormatter(IPftFormatter formatter)
        {
        }

        public override Record ReadRecord(int mfn)
        {
            return null;
        }

        public override void Dispose()
        {
        }
    } // class NullProvider

} // namespace ManagedIrbis.Client
