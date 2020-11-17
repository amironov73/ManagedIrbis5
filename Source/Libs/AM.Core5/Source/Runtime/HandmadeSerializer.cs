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
        /// Constructor.
        /// </summary>
        public HandmadeSerializer()
        {
            PrefixLength = PrefixLength.Full;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HandmadeSerializer
            (
                PrefixLength prefixLength
            )
        {
            PrefixLength = prefixLength;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize object.
        /// </summary>
        public IHandmadeSerializable Deserialize
            (
                BinaryReader reader
            )
        {
            var typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            var type = ReferenceEquals(Assembly, null)
                ? Type.GetType(typeName, true)
                : Assembly.GetType(typeName, true);
            type = type.ThrowIfNull("type");

            var result = (IHandmadeSerializable)Activator.CreateInstance(type)
                .ThrowIfNull("Activator.CreateInstance");

            result.RestoreFromStream(reader);

            return result;
        }

        /// <summary>
        /// Deserialize array of objects.
        /// </summary>
        public IHandmadeSerializable[] DeserializeArray
            (
                BinaryReader reader
            )
        {
            var count = reader.ReadPackedInt32();
            var typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            var type = ReferenceEquals(Assembly, null)
                ? Type.GetType(typeName, true)
                : Assembly.GetType(typeName, true);
            type = type.ThrowIfNull("type");

            var result = new IHandmadeSerializable[count];

            for (var i = 0; i < count; i++)
            {
                var obj = (IHandmadeSerializable)Activator.CreateInstance(type)
                    .ThrowIfNull("Activator.CreateInstances");
                obj.RestoreFromStream(reader);
                result[i] = obj;
            }

            return result;
        }

        /// <summary>
        /// Get name of specified type.
        /// </summary>
        public string GetTypeName
            (
                object obj
            )
        {
            Sure.NotNull(obj, nameof(obj));

            var type = obj.GetType().ThrowIfNull("obj.GetType()");
            string result;

            switch (PrefixLength)
            {
                case PrefixLength.Short:
                    result = type.Name;
                    break;

                case PrefixLength.Moderate:
                    result = type.FullName.ThrowIfNull("type.FullName");
                    break;

                case PrefixLength.Full:
                    result = type.AssemblyQualifiedName
                        .ThrowIfNull("type.AssemblyQualifiedName");
                    break;

                default:
                    Magna.Error
                        (
                            nameof(HandmadeSerializer)
                            + "::"
                            + nameof(GetTypeName)
                            + ": unexpected PrefixLength="
                            + PrefixLength
                        );

                    throw new InvalidOperationException();
            }

            return result.ThrowIfNull("result");
        }

        /// <summary>
        /// Serialize the object.
        /// </summary>
        public HandmadeSerializer Serialize
            (
                BinaryWriter writer,
                IHandmadeSerializable obj
            )
        {
            var typeName = GetTypeName(obj);
            writer.Write(typeName);

            obj.SaveToStream(writer);

            return this;
        }

        /// <summary>
        /// Serialize the object.
        /// </summary>
        public HandmadeSerializer Serialize
            (
                BinaryWriter writer,
                IHandmadeSerializable[] array
            )
        {
            var count = array.Length;
            if (count == 0)
            {
                Magna.Error
                    (
                        nameof(HandmadeSerializer) + "::" + nameof(Serialize)
                        + ": count=0"
                    );

                throw new ArgumentException();
            }

            writer.WritePackedInt32(count);

            var first = array[0];

            var typeName = GetTypeName(first);
            writer.Write(typeName);

            foreach (var obj in array)
            {
                obj.SaveToStream(writer);
            }

            return this;
        }

        #endregion
    }
}
