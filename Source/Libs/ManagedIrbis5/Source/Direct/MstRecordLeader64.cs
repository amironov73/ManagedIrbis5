// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MstRecordLeader64.cs -- заголовок библиографической записи в MST-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Заголовок библиографической записи в MST-файле.
    /// </summary>
    [DebuggerDisplay("MFN={Mfn}, Length={Length}, NVF={Nvf}, Status={Status}")]
    public struct MstRecordLeader64
    {
        #region Constants

        /// <summary>
        /// Фиксированный размер лидера записи.
        /// </summary>
        public const int LeaderSize = 32;

        #endregion

        #region Properties

        /// <summary>
        /// Номер записи в  файле документов.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Длина записи (всегда четное число).
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Ссылка на предыдущую версию записи.
        /// </summary>
        public long Previous { get; set; }

        /// <summary>
        /// Смещение (базовый адрес) полей
        /// переменной длины (это общая часть
        /// лидера и справочника записи в байтах).
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Число полей в записи (т.е. число входов
        /// в справочнике).
        /// </summary>
        public int Nvf { get; set; }

        /// <summary>
        /// Индикатор записи (логически удаленная и т.п.).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Номер версии записи.
        /// </summary>
        public int Version { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record leader.
        /// </summary>
        public static MstRecordLeader64 Read
            (
                Stream stream
            )
        {
            var result = new MstRecordLeader64
            {
                Mfn = stream.ReadInt32Network(),
                Length = stream.ReadInt32Network(),
                Previous = stream.ReadInt64Network(),
                Base = stream.ReadInt32Network(),
                Nvf = stream.ReadInt32Network(),
                Version = stream.ReadInt32Network(),
                Status = stream.ReadInt32Network()
            };

            //Debug.Assert(result.Base ==
            //    (LeaderSize + result.Nvf * MstDictionaryEntry64.EntrySize));

            return result;

        } // method Read

        /// <summary>
        /// Read the record leader.
        /// </summary>
        public static MstRecordLeader64 Parse
            (
                ReadOnlySpan<byte> bytes
            )
        {
            // var navigator = new ValueByteNavigator (bytes);
            var result = new MstRecordLeader64
            {
                // Mfn = stream.ReadInt32Network(),
                // Length = stream.ReadInt32Network(),
                // Previous = stream.ReadInt64Network(),
                // Base = stream.ReadInt32Network(),
                // Nvf = stream.ReadInt32Network(),
                // Version = stream.ReadInt32Network(),
                // Status = stream.ReadInt32Network()
            };

            return result;

        } // method Parse

        /// <summary>
        /// Write the record leader.
        /// </summary>
        public void Write
            (
                Stream stream
            )
        {
            stream.WriteInt32Network(Mfn);
            stream.WriteInt32Network(Length);
            stream.WriteInt64Network(Previous);
            stream.WriteInt32Network(Base);
            stream.WriteInt32Network(Nvf);
            stream.WriteInt32Network(Version);
            stream.WriteInt32Network(Status);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString ()
        {
            return $"Mfn: {Mfn}, Length: {Length}, Previous: {Previous}, Base: {Base}, Nvf: {Nvf}, Status: {Status}, Version: {Version}";
        }

        #endregion
    }
}
