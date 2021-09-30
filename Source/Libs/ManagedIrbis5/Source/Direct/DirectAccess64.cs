// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* DirectAccess64.cs -- direct reading IRBIS64 databases
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Collections;
using AM.Logging;

using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    //
    // Начиная с 2014.1
    //
    // Добавлена возможность фрагментации словаря БД.
    //
    // При значительных объемах БД (более 1 млн. записей)
    // фрагментация словаря БД существенно ускоряет актуализацию записей.
    //
    // В связи с этим в INI-файл (irbisa.ini) добавлен новый параметр
    //
    // CREATE_OLD_INVERTION_FILES=1
    //
    // который принимает значения: 1 (по умолчанию) - нет фрагментации
    // 0 - есть фрагментация.
    //
    // Использовать фрагментацию словаря (т.е. устанавливать значение 0)
    // имеет смысл при работе с БД, содержащими более 1 млн. записей.
    //
    // При включении фрагментации (CREATE_OLD_INVERTION_FILES=0)
    // для непустой БД (MFN>0) необходимо выполнить режим
    // СОЗДАТЬ СЛОВАРЬ ЗАНОВО.
    //
    // В таблицу описания БД в интерфейсе АРМа Администратор добавлена
    // строка, отражающая состояние БД в части фрагментации словаря:
    // ФРАГМЕНТАЦИЯ СЛОВАРЯ - Да/Нет.
    //

    //
    // Начиная с 2015.1
    //
    // Обеспечена возможность фрагментации файла документов
    // (по умолчанию отсутствует), что позволяет распараллелить
    // (а следовательно и ускорить) процессы одновременного редактирования
    // записей из разных диапазонов MFN. Данный режим рекомендуется
    // применять при активной книговыдаче для баз данных читателей
    // RDR и электронного каталога.
    //
    // Чтобы включить режим фрагментации файла документов, надо установить
    // в ини файле АРМ Администратор параметр
    // MST_NUM_FRAGMENTS=N
    // (где N принимает значения от 2 до 32 и определяет количество
    // фрагментов, на которые разбивается файл документов)
    // после чего сделать РЕОРГАНИЗАЦИЮ ФАЙЛА ДОКУМЕНТОВ.
    // Параметр фрагментации сохраняется непосредственно в базе данных,
    // так что все дальнейшие операции (на сервере или в АРМ Администратор),
    // кроме РЕОРГАНИЗАЦИИ ФАЙЛА ДОКУМЕНТОВ, параметр MST_NUM_FRAGMENTS
    // не используют.
    // В результате фрагментации образуется N пар файлов MST и XRF.
    // Нумерация начинается с индекса 0 и сохраняется в расширении этих файлов,
    // например, IBIS.MST1 IBIS.XRF1, причем индекс 0 не используется,
    // т.е. IBIS.MST0 пишется как IBIS.MST. Новые записи сохраняются
    // в последнем фрагменте файла документов (с индексом N–1),
    // поэтому при существенном изменении общего объема БД необходимо
    // проводить РЕОРГАНИЗАЦИЮ ФАЙЛА ДОКУМЕНТОВ (в результате чего фрагментация,
    // т.е. распределение записей по фрагментам будет выполнена заново.)
    //
    // Фактически не работает
    //

    /// <summary>
    /// Direct reading IRBIS64 databases.
    /// </summary>
    public sealed class DirectAccess64
        : IDisposable,
        ISupportLogging,
        IServiceProvider
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        public MstFile64 Mst { get; private set; }

        /// <summary>
        /// Cross-references file.
        /// </summary>
        public XrfFile64 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        public InvertedFile64 InvertedFile { get; private set; }

        /// <summary>
        /// Database path.
        /// </summary>
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DirectAccess64
            (
                string masterFile,
                DirectAccessMode mode = DirectAccessMode.Exclusive,
                IServiceProvider? serviceProvider = null
            )
        {
            Sure.NotNullNorEmpty (masterFile, nameof(masterFile));

            _serviceProvider = serviceProvider ?? Magna.Host.Services;
            _logger = (ILogger?) GetService (typeof(ILogger<MstFile64>));
            _logger?.LogTrace ($"{nameof(DirectAccess64)}::Constructor ({masterFile}, {mode})");

            Database = Path.GetFileNameWithoutExtension (masterFile)
                .ThrowIfNullOrEmpty();
            Mst = new MstFile64
                (
                    Path.ChangeExtension (masterFile, ".mst"),
                    mode,
                    _serviceProvider
                );
            Xrf = new XrfFile64
                (
                    Path.ChangeExtension(masterFile, ".xrf"),
                    mode,
                    _serviceProvider
                );
            InvertedFile = new InvertedFile64
                (
                    Path.ChangeExtension(masterFile, ".ifp"),
                    mode,
                    _serviceProvider
                );

        } // constructor

        #endregion

        #region Private members

        private ILogger? _logger;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение стандартной информации о базе данных.
        /// </summary>
        public DatabaseInfo GetDatabaseInfo()
        {
            var maxMfn = GetMaxMfn();
            var logicallyDeleted = new LocalList<int>();
            var physicallyDeleted = new LocalList<int>();
            var nonActualized = new LocalList<int>();
            var lockedRecords = new LocalList<int>();

            for (var mfn = 1; mfn <= maxMfn; mfn++)
            {
                var record = Xrf.ReadRecord(mfn);
                if ((record.Status & RecordStatus.LogicallyDeleted) != 0)
                {
                    logicallyDeleted.Add(mfn);
                }
                if ((record.Status & RecordStatus.PhysicallyDeleted) != 0)
                {
                    physicallyDeleted.Add(mfn);
                }
                if ((record.Status & RecordStatus.NonActualized) != 0)
                {
                    nonActualized.Add(mfn);
                }
                if ((record.Status & RecordStatus.Locked) != 0)
                {
                    lockedRecords.Add(mfn);
                }
            }

            var result = new DatabaseInfo
            {
                MaxMfn = maxMfn,
                DatabaseLocked = Mst.ReadDatabaseLockedFlag(),
                LogicallyDeletedRecords = logicallyDeleted.ToArray(),
                PhysicallyDeletedRecords = physicallyDeleted.ToArray(),
                NonActualizedRecords = nonActualized.ToArray(),
                LockedRecords = lockedRecords.ToArray()
            };

            return result;

        } // method GetDatabaseInfo

        /// <summary>
        /// Получает реальный максимальный MFN в базе данных.
        /// НЕ СЛЕДУЮЩИЙ MFN, а именно максимальный!
        /// </summary>
        public int GetMaxMfn() => Mst.ControlRecord.NextMfn - 1;

        /// <summary>
        /// Чтение записи в сыром формате мастер-файла.
        /// </summary>
        public MstRecord64? ReadMstRecord
            (
                int mfn
            )
        {
            XrfRecord64 xrfRecord;
            try
            {
                xrfRecord = Xrf.ReadRecord(mfn);
            }
            catch
            {
                return null;
            }

            if (xrfRecord.Offset == 0)
            {
                return null;
            }

            var result = Mst.ReadRecord (xrfRecord.Offset);

            return result;

        } // method ReadMstRecord

        /// <summary>
        /// Read record with given MFN.
        /// </summary>
        public T? ReadRecord<T>
            (
                int mfn
            )
            where T: class, IRecord, new()
        {
            XrfRecord64 xrfRecord;
            try
            {
                xrfRecord = Xrf.ReadRecord(mfn);
            }
            catch
            {
                return null;
            }

            if (xrfRecord.Offset == 0
                || (xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
            {
                return null;
            }

            var mstRecord = Mst.ReadRecord(xrfRecord.Offset);
            var result = new T();
            result.Decode(mstRecord);
            result.Database = Database;

            return result;

        } // method ReadRecord

        /// <summary>
        /// Read record with given MFN.
        /// </summary>
        public RawRecord? ReadRawRecord
            (
                int mfn
            )
        {
            return null;
        }

        /// <summary>
        /// Read all versions of the record.
        /// </summary>
        public T[] ReadAllRecordVersions<T>
            (
                int mfn
            )
            where T: class, IRecord, new()
        {
            var result = new List<T>();
            var lastVersion = ReadRecord<T>(mfn);
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
                    var mstRecord = Mst.ReadRecord(offset);
                    MarcRecord previousVersion = mstRecord.DecodeRecord();
                    previousVersion.Database = lastVersion.Database;
                    previousVersion.Mfn = lastVersion.Mfn;
                    result.Add(previousVersion);
                    lastVersion = previousVersion;
                }

                */
            }

            return result.ToArray();

        } // method ReadAllRecordVersions

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
        /// Reopen files.
        /// </summary>
        public void ReopenFiles
            (
                DirectAccessMode newMode
            )
        {
            Mst.ReopenFile(newMode);
            Xrf.ReopenFile(newMode);
            InvertedFile.ReopenFiles(newMode);
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
            var result = new List<Record>();
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
                        "DirectReader64::SearchReadSimple",
                        exception
                    );
                }
            }
            return result.ToArray();

        } // method SearchReadSimple

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRawRecord
            (
                MstRecord64 mstRecord
            )
        {
            var leader = mstRecord.Leader;
            var mfn = leader.Mfn;
            XrfRecord64 xrfRecord;
            if (mfn == 0)
            {
                mfn = Mst.ControlRecord.NextMfn;
                leader.Mfn = mfn;
                var control = Mst.ControlRecord;
                control.NextMfn = mfn + 1;
                Mst.ControlRecord = control;
                xrfRecord = new XrfRecord64
                {
                    // Mfn = mfn,
                    Offset = Mst.WriteRecord(mstRecord),
                    Status = (RecordStatus)leader.Status
                };
            }
            else
            {
                xrfRecord = Xrf.ReadRecord(mfn);
                var previousOffset = xrfRecord.Offset;
                leader.Previous = previousOffset;
                var previousLeader = Mst.ReadLeader(previousOffset);
                previousLeader.Status = (int)RecordStatus.NonActualized;
                Mst.UpdateLeader(previousLeader, previousOffset);
                xrfRecord.Offset = Mst.WriteRecord(mstRecord);
            }
            Xrf.WriteRecord(mfn, xrfRecord);

            Mst.UpdateControlRecord(false);

        } // method WriteRawRecord

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                Record record
            )
        {
            if (record.Version < 0)
            {
                record.Version = 0;
            }

            record.Version++;
            record.Status |= RecordStatus.Last | RecordStatus.NonActualized;
            var mstRecord64 = MstRecord64.EncodeRecord(record);
            WriteRawRecord(mstRecord64);
            record.Database = Database;
            record.Mfn = mstRecord64.Leader.Mfn;
            // record.PreviousOffset = mstRecord64.Leader.Previous;

        } // method WriteRecord

        #endregion

        #region ISupportLogging members

        /// <inheritdoc cref="ISupportLogging.Logger"/>
        // TODO implement
        public ILogger? Logger => _logger;

        /// <inheritdoc cref="ISupportLogging.SetLogger"/>
        public void SetLogger(ILogger? logger)
            => _logger = logger;

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType)
            => _serviceProvider.GetService(serviceType);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _logger?.LogTrace($"{nameof(DirectAccess64)}::{nameof(Dispose)}");

            Mst.Dispose();
            Xrf.Dispose();
            InvertedFile.Dispose();

        } // method Dispose

        #endregion

    } // class DirectAccess64

} // namespace ManagedIrbis.Directe

