// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ProtoUtility.cs -- служебные методы для облегчения возни с Protobuf
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using Google.Protobuf;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Grpc
{
    /// <summary>
    /// Служебные методы для облегчения возни с Protobuf.
    /// </summary>
    public static class ProtoUtility
    {
        #region Public methods

        /// <summary>
        /// Преобразование из обменного формата.
        /// </summary>
        public static SubField FromProto(this ProtoSubField subField) => new()
        {
            Code = subField.Code.FirstChar(),
            Value = subField.Value
        };

        /// <summary>
        /// Преобразование из обменного формата.
        /// </summary>
        public static Field FromProto
            (
                this ProtoField field
            )
        {
            var result = new Field { Tag = field.Tag };
            foreach (var subfield in field.Subfields)
            {
                result.Add(subfield.FromProto());
            }

            return result;

        } // method FromProto

        /// <summary>
        /// Преобразование из обменного формата.
        /// </summary>
        public static Record FromProto
            (
                this ProtoRecord record
            )
        {
            var result = new Record()
            {
                Mfn = record.Mfn,
                Database = record.Database,
                Status = unchecked ((RecordStatus) record.Status),
                Version = record.Version
            };
            foreach (var field in record.Fields)
            {
                result.Fields.Add(field.FromProto());
            }

            return result;
        }

        /// <summary>
        /// Преобразование в формат для обмена.
        /// </summary>
        public static ProtoSubField ToProto (this SubField subfield) => new ()
        {
            Code = subfield.Code.ToString(),
            Value = subfield.Value
        };

        /// <summary>
        /// Преобразование в формат для обмена.
        /// </summary>
        public static ProtoField ToProto
            (
                this Field field
            )
        {
            var result = new ProtoField() { Tag = field.Tag };
            foreach (var subfield in field.Subfields)
            {
                result.Subfields.Add(subfield.ToProto());
            }

            return result;

        } // method ToProto

        /// <summary>
        /// Преобразование в формат для обмена.
        /// </summary>
        public static ProtoRecord ToProto
            (
                this Record record
            )
        {
            var result = new ProtoRecord()
            {
                Mfn = record.Mfn,
                Database = record.Database,
                Status = unchecked((int) record.Status),
                Version = record.Version
            };
            foreach (var field in record.Fields)
            {
                result.Fields.Add(field.ToProto());
            }

            return result;

        } // method ToProto

        /// <summary>
        /// Вывод в поток в обменном формате.
        /// </summary>
        public static void WriteTo (this Record record, Stream stream) =>
            record.ToProto().WriteTo(stream);

        #endregion

    } // class ProtoUtility

} // namespace ManagedIrbis.Grpc
