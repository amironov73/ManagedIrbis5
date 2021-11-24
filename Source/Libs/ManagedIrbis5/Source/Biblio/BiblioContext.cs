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

using System.Linq;

using AM;
using AM.Text.Output;

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
        /// Процессор.
        /// </summary>
        public BiblioProcessor? Processor { get; set; }

        /// <summary>
        /// Документ.
        /// </summary>
        public BiblioDocument Document { get; private set; }

        /// <summary>
        /// Синхронный провайдер.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Лог.
        /// </summary>
        public AbstractOutput Log { get; private set; }

        /// <summary>
        /// Общее количество <see cref="BiblioItem"/>.
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
        /// Конструктор.
        /// </summary>
        public BiblioContext
            (
                BiblioDocument document,
                ISyncProvider provider,
                AbstractOutput log
            )
        {
            Sure.NotNull (document);
            Sure.NotNull (provider);
            Sure.NotNull (log);

            Document = document;
            Provider = provider;
            Log = log;
            ItemCount = 0;
            ReportContext = new ReportContext (provider);
            Records = new RecordCollection();
            BadRecords = new RecordCollection();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find the record with specified MFN.
        /// </summary>
        public Record? FindRecord
            (
                int mfn
            )
        {
            Sure.Positive (mfn);

            var result = Records.FirstOrDefault (record => record.Mfn == mfn);

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
            var verifier = new Verifier<BiblioContext> (this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion
    }
}
