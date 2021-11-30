// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PingStatistics.cs -- статистика пинга до сервера ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Statistics
{
    /// <summary>
    /// Статистика пинга до сервера ИРБИС64.
    /// </summary>
    public sealed class PingStatistics
    {
        #region Properties

        /// <summary>
        /// Average roundtrip time.
        /// </summary>
        public int AverageTime
        {
            get
            {
                return (int) Data
                    .Where (item => item.Success)
                    .Select (item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Average();
            }
        }

        /// <summary>
        /// Maximum roundtrip time.
        /// </summary>
        public int MaxTime
        {
            get
            {
                return Data
                    .Where (item => item.Success)
                    .Select (item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Max ();
            }
        }

        /// <summary>
        /// Minimum roundtrip time.
        /// </summary>
        public int MinTime
        {
            get
            {
                return Data
                    .Where (item => item.Success)
                    .Select (item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Min();
            }
        }

        /// <summary>
        /// Data.
        /// </summary>
        public Queue<PingData> Data { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Add entry.
        /// </summary>
        public void Add
            (
                PingData item
            )
        {
            Data.Enqueue (item);
        }

        /// <summary>
        /// Clear the statistics.
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        #endregion
    }
}
