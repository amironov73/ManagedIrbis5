// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* BiblioProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text.Output;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public class BiblioProcessor
    {
        #region Properties

        /// <summary>
        /// Лог.
        /// </summary>
        public ReportOutput Output { get; private set; }

        /// <summary>
        /// Отчет.
        /// </summary>
        public IrbisReport Report { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public BiblioProcessor()
        {
            Output = new ReportOutput();
            Report = new IrbisReport();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BiblioProcessor
            (
                ReportOutput output
            )
        {
            Output = output;
            Report = new IrbisReport();
        }

        #endregion

        #region Private members

        /// <summary>
        ///
        /// </summary>
        protected virtual void BildDictionaries
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            var document = context.Document;
            document.BuildDictionaries (context);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void BildItems
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            var document = context.Document;
            document.BuildItems (context);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FinalRender
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            WriteDelimiter (context);
            log.WriteLine ("Begin final render");

            var provider = context.Provider
                .ThrowIfNull ("context.Provider");
            var report = Report.ThrowIfNull ("Report");
            var reportContext = new ReportContext (provider);
            Output = reportContext.Output;
            reportContext.SetDriver (new RtfDriver());

            var prologue = GetText (context, "*prologue.txt");
            if (!string.IsNullOrEmpty (prologue))
            {
                reportContext.Driver.SetPrologue (prologue);
            }

            reportContext.Verify (true);
            report.Verify (true);

            report.Render (reportContext);

            log.WriteLine ("End final render");
        }

        /// <summary>
        /// Gather records.
        /// </summary>
        protected virtual void GatherRecords
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            var document = context.Document;
            document.GatherRecords (context);
        }

        /// <summary>
        /// Gather terms.
        /// </summary>
        protected virtual void GatherTerms
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            var document = context.Document;
            document.GatherTerms (context);
        }

        /// <summary>
        /// Number items.
        /// </summary>
        protected virtual void NumberItems
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            var document = context.Document;
            document.NumberItems (context);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RenderReport
            (
                BiblioContext context
            )
        {
            WriteDelimiter (context);
            Report = new IrbisReport();
            var document = context.Document;
            document.RenderItems (context);
        }

        private void WriteDelimiter
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine (new string ('=', 70));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get formatter.
        /// </summary>
        public IPftFormatter AcquireFormatter
            (
                BiblioContext context
            )
        {
            /*

            var provider = context.Provider
                .ThrowIfNull("context.Provider");
            var result = provider.AcquireFormatter()
                .ThrowIfNull("provider.AcquireFormatter");

            //PftContext pftContext = new PftContext(null);
            //PftFormatter result = new PftFormatter(pftContext);
            //result.SetProvider(context.Provider);

            return result;

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Build document.
        /// </summary>
        public virtual string BuildDocument
            (
                BiblioContext context
            )
        {
            var document = context.Document;
            document.Initialize (context);

            GatherRecords (context);
            BildItems (context);
            NumberItems (context);
            GatherTerms (context);
            BildDictionaries (context);
            RenderReport (context);
            FinalRender (context);
            WriteDelimiter (context);

            return string.Empty;
        }

        /// <summary>
        /// Get text.
        /// </summary>
        public virtual string? GetText
            (
                BiblioContext context,
                string path
            )
        {
            var log = context.Log;
            var provider = context.Provider;

            string? result = null;
            try
            {
                string fileName;
                if (path.StartsWith ("*"))
                {
                    fileName = path.Substring (1);
                    result = File.ReadAllText (fileName, IrbisEncoding.Ansi);
                }

                //else if (path.StartsWith("@"))
                //{
                //    fileName = path.Substring(1);
                //    FileSpecification specification
                //        = new FileSpecification
                //            (
                //                IrbisPath.MasterFile,
                //                provider.Database,
                //                fileName
                //            );
                //    result = provider.ReadFile(specification);
                //}
                else
                {
                    result = path;
                }
            }
            catch (Exception exception)
            {
                log.WriteLine ("Exception: {0}", exception);
                throw;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Initialize
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine ("Begin initialize the processor");
            context.Processor = this;
            log.WriteLine ("End initialize the processor");
        }

        /// <summary>
        /// Release the formatter.
        /// </summary>
        public virtual void ReleaseFormatter
            (
                BiblioContext context,
                IPftFormatter formatter
            )
        {
            /*
            var provider = context.Provider;
            provider.ReleaseFormatter(formatter);
            */

            throw new NotImplementedException();
        }

        #endregion
    }
}
