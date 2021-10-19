// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HandmadeSerializer.cs -- закат солнца вручную
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;

using AM.IO;

#endregion

#nullable enable

namespace AM.Runtime
{
    /// <summary>
    /// Закат солнца вручную.
    /// </summary>
    public sealed class HandmadeSerializer
    {
        #region Properties

        /// <summary>
        /// Namespace for short type names.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Assembly for short type names.
        /// </summary>
        public Assembly? Assembly { get; set; }

        /// <summary>
        /// Prefix length.
        /// </summary>
        public PrefixLength PrefixLength { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HandmadeSerializer()
        {
            PrefixLength = PrefixLength.Full;

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HandmadeSerializer
            (
                PrefixLength prefixLength
            )
        {
            PrefixLength = prefixLength;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Десериализация одиночного объекта.
        /// </summary>
        public IHandmadeSerializable Deserialize
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            var typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            var type = ReferenceEquals (Assembly, null)
                ? Type.GetType (typeName, true)
                : Assembly.GetType (typeName, true);
            type = type.ThrowIfNull();

            var result = (IHandmadeSerializable)Activator.CreateInstance (type)
                .ThrowIfNull();

            result.RestoreFromStream (reader);

            return result;

        } // method Deserialize

        /// <summary>
        /// Десериализация массива объектов.
        /// </summary>
        public IHandmadeSerializable[] DeserializeArray
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            var count = reader.ReadPackedInt32();
            var typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            var type = ReferenceEquals (Assembly, null)
                ? Type.GetType (typeName, true)
                : Assembly.GetType (typeName, true);
            type = type.ThrowIfNull ();

            var result = new IHandmadeSerializable[count];

            for (var i = 0; i < count; i++)
            {
                var obj = (IHandmadeSerializable) Activator.CreateInstance (type)
                    .ThrowIfNull ();
                obj.RestoreFromStream (reader);
                result[i] = obj;
            }

            return result;

        } // DeserializeArray

        /// <summary>
        /// Получение имени типа указанного объекта.
        /// </summary>
        public string GetTypeName
            (
                object obj
            )
        {
            Sure.NotNull (obj);

            var type = obj.GetType().ThrowIfNull ();
            string result;

            switch (PrefixLength)
            {
                case PrefixLength.Short:
                    result = type.Name;
                    break;

                case PrefixLength.Moderate:
                    result = type.FullName.ThrowIfNull ();
                    break;

                case PrefixLength.Full:
                    result = type.AssemblyQualifiedName
                        .ThrowIfNull ();
                    break;

                default:
                    Magna.Error
                        (
                            nameof (HandmadeSerializer)
                            + "::"
                            + nameof (GetTypeName)
                            + ": unexpected PrefixLength="
                            + PrefixLength
                        );

                    throw new InvalidOperationException();
            }

            return result.ThrowIfNull ();

        } // method GetTypeName

        /// <summary>
        /// Сериализация одиночного объекта.
        /// </summary>
        /// <remarks>
        /// <c>null</c> не поддерживается.
        /// </remarks>
        public HandmadeSerializer Serialize
            (
                BinaryWriter writer,
                IHandmadeSerializable obj
            )
        {
            Sure.NotNull (writer);
            Sure.NotNull (obj);

            var typeName = GetTypeName (obj);
            writer.Write (typeName);

            obj.SaveToStream (writer);

            return this;

        } // method Serialize

        /// <summary>
        /// Сериализация массива объектов.
        /// </summary>
        /// <remarks>
        /// Пустые массивы не поддерживаются, потому что нам нужно имя типа.
        /// </remarks>
        public HandmadeSerializer Serialize
            (
                BinaryWriter writer,
                IHandmadeSerializable[] array
            )
        {
            Sure.NotNull (writer);
            Sure.NotNull (array);

            var count = array.Length;
            if (count == 0)
            {
                Magna.Error
                    (
                        nameof (HandmadeSerializer) + "::" + nameof (Serialize)
                        + ": count=0"
                    );

                throw new ArgumentException (nameof (array));
            }

            writer.WritePackedInt32 (count);

            var first = array[0];

            var typeName = GetTypeName (first);
            writer.Write (typeName);

            foreach (var obj in array)
            {
                obj.SaveToStream (writer);
            }

            return this;

        } // method Serialize

        #endregion

    } // method HandmadeSerializer

} // namespace AM.Runtime
