// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ReservationManager.cs -- менеджер резервирования компьютеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

#nullable enable

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Менеджер резервирования компьютеров.
    /// </summary>
    public sealed class ReservationManager
    {
        #region Properties

        /// <summary>
        /// Синхронный ИРБИС-провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReservationManager
            (
                ISyncProvider provider
            )
        {
            Sure.NotNull (provider);

            Provider = provider;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение имени читателя по номеру билета.
        /// </summary>
        public string? GetReaderName
            (
                string ticket
            )
        {
            Sure.NotNullNorEmpty (ticket);

            var previousDatabase = Provider.Database;
            Provider.Database = ReaderUtility.DatabaseName;
            try
            {
                var expression = $"\"{ReaderUtility.IdentifierPrefix}{ticket}\"";
                var found = Provider.Search (expression);
                if (found.Length == 0)
                {
                    return null;
                }

                if (found.Length != 1)
                {
                    return null;
                }

                var mfn = found[0];
                var record = Provider.ReadRecord (mfn);
                if (record is null)
                {
                    return null;
                }

                var reader = ReaderInfo.Parse (record);
                var result = reader.FullName;

                return result;
            }
            finally
            {
                Provider.Database = previousDatabase;
            }

        } // method GetReaderName

        /// <summary>
        /// Получение библиографической записи <see cref="Record"/> для указанного ресурса.
        /// </summary>
        public Record GetResourceRecord
            (
                ReservationInfo resource
            )
        {
            Sure.NotNull (resource);

            var result = GetResourceRecord
                (
                    resource.Room.ThrowIfNull(),
                    resource.Number.ThrowIfNull()
                );

            return result ?? throw new IrbisException();

        } // method GetResourceRecord

        /// <summary>
        /// Получение библиографической записи для указанного ресурса
        /// (задаваемоего сочетанием "зал-номер ресурса").
        /// </summary>
        public Record? GetResourceRecord
            (
                string room,
                string number
            )
        {
            Sure.NotNullNorEmpty (room);
            Sure.NotNullNorEmpty (number);

            var expression = $"\"{ReservationUtility.RoomPrefix}{room}\" * \"{ReservationUtility.NumberPrefix}{number}\"";
            var found = Provider.Search (expression);

            return found.Length != 1
                ? null
                : Provider.ReadRecord (found[0]);

        } // method GetResourceRecord

        /// <summary>
        /// Назначение ресурса читателю.
        /// </summary>
        public ReservationInfo AssignResource
            (
                ReservationInfo resource,
                string ticket
            )
        {
            Sure.NotNull (resource);
            Sure.NotNullNorEmpty (ticket);

            var record = GetResourceRecord (resource);
            resource = ReservationInfo.ParseRecord (record);
            if (!resource.Status.IsOneOf
                (
                    ReservationStatus.Free,
                    ReservationStatus.Reserved
                ))
            {
                // TODO some reaction?

                return resource;
            }

            resource.Status = ReservationStatus.Busy;

            var readerName = GetReaderName (ticket);
            var entry = new HistoryInfo
            {
                BeginDate = DateTime.Now,
                Ticket = ticket,
                Name = readerName
            };
            resource.History.Add (entry);

            resource.ApplyToRecord (record);
            Provider.WriteRecord (record);

            return resource;

        } // method AssignResource

        /// <summary>
        /// Список залов.
        /// </summary>
        public MenuEntry[] ListRooms()
        {
            var specification = new FileSpecification ()
                {
                    Path = IrbisPath.MasterFile,
                    Database = Provider.Database.ThrowIfNullOrEmpty (),
                    FileName = ReservationUtility.RoomMenu
                };
            var menuFile = Provider.ReadMenuFile (specification);

            return menuFile is null
                ? Array.Empty<MenuEntry>()
                : menuFile.Entries.ToArray();

        } // method ListRooms

        /// <summary>
        /// Список ресурсов для уазанного зала.
        /// </summary>
        public ReservationInfo[] ListResources
            (
                string? roomCode
            )
        {
            var expression = $"\"{ReservationUtility.RoomPrefix}{roomCode}\"";
            var found = Provider.Search (expression);
            var result = new List<ReservationInfo> (found.Length);
            foreach (var mfn in found)
            {
                var record = Provider.ReadRecord (mfn);
                if (!ReferenceEquals (record, null))
                {
                    var item = ReservationInfo.ParseRecord (record);
                    result.Add (item);
                }
            }

            return result.ToArray();

        } // method ListResources

        /// <summary>
        /// Освобождение ресурса.
        /// </summary>
        public ReservationInfo ReleaseResource
            (
                ReservationInfo resource
            )
        {
            Sure.NotNull (resource);

            var record = GetResourceRecord (resource);
            resource = ReservationInfo.ParseRecord (record);
            resource.Status = ReservationStatus.Free;
            var entry = resource.History.LastOrDefault();
            if (!ReferenceEquals (entry, null))
            {
                entry.EndDate = DateTime.Now;
            }

            resource.ApplyToRecord (record);
            Provider.WriteRecord (record);

            return resource;

        } // method ReleaseResource

        /// <summary>
        /// Создание запроса на резервирование компьютера.
        /// </summary>
        public ReservationInfo CreateClaim
            (
                ReservationInfo resource,
                DateTime beginDate,
                TimeSpan duration
            )
        {
            Sure.NotNull (resource);

            var record = GetResourceRecord (resource);
            resource = ReservationInfo.ParseRecord (record);
            if (resource.Status.IsOneOf (ReservationStatus.Free, ReservationStatus.Busy))
            {
                throw new IrbisException();
            }

            resource.Status = ReservationStatus.Reserved;
            var claim = new ReservationClaim
            {
                BeginDateTime = beginDate,
                Duration = duration
            };
            resource.Claims.Add (claim);
            resource.ApplyToRecord (record);
            Provider.WriteRecord (record);

            return resource;

        } // method CreateClaim

        #endregion

    } // class ReservationManager

} // namespace ManagedIrbis.Reservations
