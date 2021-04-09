// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MappedAccess64.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Direct database access with <see cref="MemoryMappedFile"/>
    /// Прямой доступ к БД с помощью MemoryMappedFile.
    /// </summary>
    public sealed class MappedAccess64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        public MappedMstFile64 Mst { get; private set; }

        /// <summary>
        /// Cross-reference file.
        /// </summary>
        public MappedXrfFile64 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        public MappedInvertedFile64 InvertedFile { get; private set; }

        /// <summary>
        /// Database path.
        /// </summary>
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MappedAccess64
            (
                string masterFile
            )
        {
            Database = Path.GetFileNameWithoutExtension(masterFile);
            Mst = new MappedMstFile64(Path.ChangeExtension(masterFile, ".mst"));
            Xrf = new MappedXrfFile64(Path.ChangeExtension(masterFile, ".xrf"));
            InvertedFile = new MappedInvertedFile64(Path.ChangeExtension(masterFile, ".ifp"));
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get max MFN for database. Not next MFN!
        /// </summary>
        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

        /// <summary>
        /// Read raw record.
        /// </summary>
        public MstRecord64? ReadRawRecord
            (
                int mfn
            )
        {
            var xrfRecord = Xrf.ReadRecord(mfn);
            if (xrfRecord.Offset == 0)
            {
                return null;
            }

            var result = Mst.ReadRecord(xrfRecord.Offset);

            return result;
        }

        /// <summary>
        /// Read record with given MFN.
        /// </summary>
        public Record? ReadRecord
            (
                int mfn
            )
        {
            var xrfRecord = Xrf.ReadRecord(mfn);
            if (xrfRecord.Offset == 0
                || (xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
            {
                return null;
            }

            var mstRecord = Mst.ReadRecord(xrfRecord.Offset);
            var result = mstRecord.DecodeRecord();
            result.Database = Database;

            return result;
        }

        /// <summary>
        /// Read all versions of the record.
        /// </summary>
        public Record[] ReadAllRecordVersions
            (
                int mfn
            )
        {
            var result = new List<Record>();
            var lastVersion = ReadRecord(mfn);
            if (lastVersion != null)
            {
                result.Add(lastVersion);

                /*

                while (true)
                {
                    long offset = lastVersion.PreviousOffset;
                    if (offset == 0)
                    {
                        break;
                    }
                    MstRecord64 mstRecord = Mst.ReadRecord(offset);
                    MarcRecord previousVersion = mstRecord.DecodeRecord();
                    previousVersion.Database = lastVersion.Database;
                    previousVersion.Mfn = lastVersion.Mfn;
                    result.Add(previousVersion);
                    lastVersion = previousVersion;
                }

                */
            }

            return result.ToArray();
        }

        /// <summary>
        /// Read links for the term.
        /// </summary>
        public TermLink[] ReadLinks
            (
                string key
            )
        {
            return InvertedFile.SearchExact(key);
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        public Term[] ReadTerms
            (
                TermParameters parameters
            )
        {
            var result = InvertedFile.ReadTerms(parameters);

            return result;
        }

        /// <summary>
        /// Simple search.
        /// </summary>
        public int[] SearchSimple
            (
                string key
            )
        {
            var found = InvertedFile.SearchSimple(key);
            var result = new List<int>();
            foreach (var mfn in found)
            {
                if (!Xrf.ReadRecord(mfn).Deleted)
                {
                    result.Add(mfn);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Simple search and read records.
        /// </summary>
        public Record[] SearchReadSimple
            (
                string key
            )
        {
            var mfns = InvertedFile.SearchSimple(key);
            var result = new List<Record>(mfns.Length);
            foreach (var mfn in mfns)
            {
                try
                {
                    var xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        var mstRecord = Mst.ReadRecord(xrfRecord.Offset);
                        if (!mstRecord.Deleted)
                        {
                            var irbisRecord = mstRecord.DecodeRecord();
                            irbisRecord.Database = Database;
                            result.Add(irbisRecord);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "MappedAccess64::SearchReadSimple",
                            exception
                        );
                }
            }
            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            InvertedFile.Dispose();
            Xrf.Dispose();
            Mst.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
