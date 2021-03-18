// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BiblioContext.cs -- контекст, в котором выполняется построение документа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Client;
using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Контекст, в которм выполняется построение документа
    /// библиографического указателя.
    /// </summary>
    public class BiblioContext
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Processor.
        /// </summary>
        public BiblioProcessor? Processor { get; set; }

        /// <summary>
        /// Document.
        /// </summary>
        public BiblioDocument Document { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Log.
        /// </summary>
        public IConsole Log { get; private set; }

        /// <summary>
        /// Count of <see cref="BiblioItem"/>s.
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// All the gathered records.
        /// </summary>
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// Bad records.
        /// </summary>
        public RecordCollection BadRecords { get; private set; }

        /// <summary>
        /// Context for report.
        /// </summary>
        public ReportContext ReportContext { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioContext
            (
                BiblioDocument document,
                IrbisProvider provider,
                IConsole log
            )
        {
            Document = document;
            Provider = provider;
            Log = log;
            ItemCount = 0;
            ReportContext = new ReportContext(provider);
            Records = new RecordCollection();
            BadRecords = new RecordCollection();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find the record with specified MFN.
        /// </summary>
        [CanBeNull]
        public Record FindRecord
            (
                int mfn
            )
        {
            Sure.Positive(mfn, "mfn");

            Record result = Records
                .FirstOrDefault(record => record.Mfn == mfn);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioContext> verifier
                = new Verifier<BiblioContext>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
