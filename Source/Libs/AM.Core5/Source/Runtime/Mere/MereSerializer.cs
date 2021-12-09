// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* MereSerializer.cs -- закат солнца вручную снова
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using AM.IO;

#endregion

#nullable enable

namespace AM.Runtime.Mere
{
    /// <summary>
    /// Закат солнца вручную снова.
    /// Умеет сериализовать объекты с динамическими полями.
    /// </summary>
    public static class MereSerializer
    {
        #region Private members

        private static readonly Dictionary<Type, MereTypeCode> _typeMap = new ()
        {
            { typeof (bool), MereTypeCode.Boolean },
            { typeof (byte), MereTypeCode.Byte },
            { typeof (sbyte), MereTypeCode.SByte },
            { typeof (char), MereTypeCode.Char },
            { typeof (short), MereTypeCode.Int16 },
            { typeof (ushort), MereTypeCode.UInt16 },
            { typeof (int), MereTypeCode.Int32 },
            { typeof (uint), MereTypeCode.UInt32 },
            { typeof (long), MereTypeCode.Int64 },
            { typeof (ulong), MereTypeCode.UInt64 },
            { typeof (float), MereTypeCode.Single },
            { typeof (double), MereTypeCode.Double },
            { typeof (decimal), MereTypeCode.Decimal },
            { typeof (DateTime), MereTypeCode.DateTime },
            { typeof (string), MereTypeCode.String }
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Восстановление объекта из потока.
        /// </summary>
        public static object? Deserialize
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            var typeCode = (MereTypeCode) reader.ReadByte();
            switch (typeCode)
            {
                case MereTypeCode.Null:
                    return null;

                case MereTypeCode.Boolean:
                    return reader.ReadBoolean();

                case MereTypeCode.Byte:
                    return reader.ReadByte();

                case MereTypeCode.SByte:
                    return reader.ReadSByte();

                case MereTypeCode.Char:
                    return reader.ReadChar();

                case MereTypeCode.Int16:
                    return reader.ReadInt16();

                case MereTypeCode.UInt16:
                    return reader.ReadUInt16();

                case MereTypeCode.Int32:
                    return reader.ReadInt32();

                case MereTypeCode.UInt32:
                    return reader.ReadUInt32();

                case MereTypeCode.Int64:
                    return reader.ReadInt64();

                case MereTypeCode.UInt64:
                    return reader.ReadUInt64();

                case MereTypeCode.Single:
                    return reader.ReadSingle();

                case MereTypeCode.Double:
                    return reader.ReadDouble();

                case MereTypeCode.Decimal:
                    return reader.ReadDecimal();

                case MereTypeCode.DateTime:
                    return reader.ReadDateTime();

                case MereTypeCode.String:
                    return reader.ReadString();

                case MereTypeCode.Array:
                {
                    var count = reader.ReadInt32();
                    var array = new object?[count];
                    for (var i = 0; i < count; i++)
                    {
                        var item = Deserialize (reader);
                        array[i] = item;
                    }

                    return array;
                }

                case MereTypeCode.List:
                {
                    var typename = reader.ReadString();
                    var type = Type.GetType (typename, true).ThrowIfNull ();
                    var list = (IList) Activator.CreateInstance (type).ThrowIfNull ();
                    var count = reader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var item = Deserialize (reader);
                        list.Add (item);
                    }

                    return list;
                }

                case MereTypeCode.Dictionary:
                {
                    var typename = reader.ReadString();
                    var type = Type.GetType (typename, true).ThrowIfNull();
                    var dict = (IDictionary)Activator.CreateInstance (type).ThrowIfNull();
                    var count = reader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var key = Deserialize (reader).ThrowIfNull ();
                        var value = Deserialize (reader);
                        dict.Add (key, value);
                    }

                    return dict;
                }

                case MereTypeCode.Object:
                {
                    var typename = reader.ReadString();
                    var type = Type.GetType (typename, true).ThrowIfNull();
                    var obj = (IMereSerializable) Activator.CreateInstance (type).ThrowIfNull();
                    obj.MereDeserialize (reader);

                    return obj;
                }
            }

            throw new ArsMagnaException ($"Unexpected type code: {typeCode}");
        }

        /// <summary>
        /// Сохранение объекта в поток.
        /// </summary>
        public static void Serialize
            (
                BinaryWriter writer,
                object? obj
            )
        {
            Sure.NotNull (writer);

            if (obj is null)
            {
                writer.Write ((byte)MereTypeCode.Null);

                return;
            }

            var type = obj.GetType();
            if (_typeMap.TryGetValue (type, out var typeCode))
            {
                writer.Write ((byte) typeCode);
                switch (typeCode)
                {
                    case MereTypeCode.Boolean:
                        writer.Write ((bool) obj);
                        return;

                    case MereTypeCode.Byte:
                        writer.Write ((byte) obj);
                        return;

                    case MereTypeCode.SByte:
                        writer.Write ((sbyte) obj);
                        return;

                    case MereTypeCode.Char:
                        writer.Write ((char) obj);
                        return;

                    case MereTypeCode.Int16:
                        writer.Write ((short) obj);
                        return;

                    case MereTypeCode.UInt16:
                        writer.Write ((ushort) obj);
                        return;

                    case MereTypeCode.Int32:
                        writer.Write ((int) obj);
                        return;

                    case MereTypeCode.UInt32:
                        writer.Write ((uint) obj);
                        return;

                    case MereTypeCode.Int64:
                        writer.Write ((long) obj);
                        return;

                    case MereTypeCode.UInt64:
                        writer.Write ((ulong) obj);
                        return;

                    case MereTypeCode.Single:
                        writer.Write ((float) obj);
                        return;

                    case MereTypeCode.Double:
                        writer.Write ((double) obj);
                        return;

                    case MereTypeCode.Decimal:
                        writer.Write ((decimal) obj);
                        return;

                    case MereTypeCode.DateTime:
                        writer.Write ((DateTime) obj);
                        return;

                    case MereTypeCode.String:
                        writer.Write ((string) obj);
                        return;

                    default:
                        throw new ArsMagnaException ($"Unexpected type code {typeCode}");
                }
            }

            if (obj is Array array)
            {
                writer.Write ((byte) MereTypeCode.Array);
                writer.Write (array.Length);
                foreach (var item in array)
                {
                    Serialize (writer, item);
                }

                return;
            }

            if (obj is IDictionary dictionary)
            {
                writer.Write ((byte) MereTypeCode.Dictionary);
                writer.Write (type.AssemblyQualifiedName.ThrowIfNullOrEmpty ());
                writer.Write (dictionary.Count);
                foreach (DictionaryEntry entry in dictionary)
                {
                    Serialize (writer, entry.Key);
                    Serialize (writer, entry.Value);
                }

                return;
            }

            if (obj is ICollection collection)
            {
                writer.Write ((byte) MereTypeCode.List);
                writer.Write (type.AssemblyQualifiedName.ThrowIfNullOrEmpty ());
                writer.Write (collection.Count);
                foreach (var item in collection)
                {
                    Serialize (writer, item);
                }

                return;
            }

            if (obj is IMereSerializable serializable)
            {
                writer.Write ((byte) MereTypeCode.Object);
                writer.Write (type.AssemblyQualifiedName.ThrowIfNullOrEmpty());
                serializable.MereSerialize (writer);

                return;
            }

            throw new ArsMagnaException();
        }

        #endregion
    }
}
