// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FileSpecification.cs -- спецификация пути к файлу на сервере ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    //
    // Путь на файл Αpath.Adbn.Afilename
    // Αpath – код путей:
    // 0 – общесистемный путь.
    // 1 – путь размещения сведений о базах данных сервера ИРБИС64
    // 2 – путь на мастер-файл базы данных.
    // 3 – путь на словарь базы данных.
    // 10 – путь на параметрию базы данных.
    // Adbn – имя базы данных
    // Afilename – имя требуемого файла с расширением
    // В случае чтения ресурса по пути 0 и 1 имя базы данных не задается.
    //

    /// <summary>
    /// Спецификация пути к файлу на сервере ИРБИС64.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Path) + "} {" + nameof(Database)
                     + "} {" + nameof (FileName) + "}")]
    public sealed class FileSpecification
        :  IHandmadeSerializable,
            IVerifiable,
            IEquatable<FileSpecification>
    {
        #region Properties

        /// <summary>
        /// Is the file binary or text?
        /// </summary>
        public bool BinaryFile { get; set; }

        /// <summary>
        /// Path.
        /// </summary>
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// File content (when we want write the file).
        /// </summary>
        public string? Content { get; set; }

        #endregion

        #region Private members

        private static bool _CompareDatabases
            (
                string? first,
                string? second
            )
        {
            if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            {
                return true;
            }

            return first.SameString(second);

        } // method CompareDatabases

        #endregion

         #region Public methods

         /// <summary>
         /// Построение спецификации файла по ее компонентам.
         /// </summary>
         public static string Build
             (
                IrbisPath path,
                string database,
                string fileName
             )
         {
             return ((int) path).ToInvariantString()
                    + "."
                    + database
                    + "."
                    + fileName;

         } // method Build

         /// <summary>
         /// Клонирование спецификации.
         /// </summary>
         public FileSpecification Clone() => (FileSpecification) MemberwiseClone();

        /// <summary>
        /// Parse the text specification.
        /// </summary>
        public static FileSpecification Parse
            (
                string text
            )
        {
            var navigator = new TextNavigator(text);
            var path = int.Parse
                (
                    navigator.ReadTo(".").ToString(),
                    CultureInfo.InvariantCulture
                );
            var database = navigator.ReadTo(".").ToString().EmptyToNull();
            var fileName = navigator.GetRemainingText().ToString();
            var binaryFile = fileName.StartsWith("@");
            if (binaryFile)
            {
                fileName = fileName.Substring(1);
            }

            string? content = null;
            var position = fileName.IndexOf("&", StringComparison.InvariantCulture);
            if (position >= 0)
            {
                content = fileName.Substring(position + 1);
                fileName = fileName.Substring(0, position);
            }
            var result = new FileSpecification
            {
                BinaryFile = binaryFile,
                Path = (IrbisPath)path,
                Database = database,
                FileName = fileName,
                Content = content
            };

            return result;

        } // method Parse

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            BinaryFile = reader.ReadBoolean();
            Path = (IrbisPath)reader.ReadPackedInt32();
            Database = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
            Content = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.Write(BinaryFile);
            writer
                .WritePackedInt32((int)Path)
                .WriteNullable(Database)
                .WriteNullable(FileName)
                .WriteNullable(Content);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FileSpecification> verifier = new Verifier<FileSpecification>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(FileName, nameof(FileName));

            if (Path != IrbisPath.System
                && Path != IrbisPath.Data)
            {
                verifier.NotNullNorEmpty(Database, nameof(Database));
            }

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <summary>
        /// Compare with other <see cref="FileSpecification"/>
        /// instance.
        /// </summary>
        public bool Equals
            (
                FileSpecification? other
            )
        {
            other = other.ThrowIfNull();

            return Path == other.Path
                   && _CompareDatabases(Database, other.Database)
                   && FileName.SameString(other.FileName);

        } // method Equals

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is FileSpecification other && Equals(other);

        } // method Equals

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Path;
                hashCode = (hashCode * 397) ^ (Database != null ? Database.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FileName != null ? FileName.GetHashCode() : 0);

                return hashCode;
            }

        } // method GetHashCode

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var fileName = FileName;
            if (BinaryFile)
            {
                fileName = "@" + fileName;
            }
            else
            {
                if (!ReferenceEquals(Content, null))
                {
                    fileName = "&" + fileName;
                }
            }

            string result;

            switch (Path)
            {
                case IrbisPath.System:
                case IrbisPath.Data:
                    result = $"{(int) Path}..{fileName}";
                    break;

                default:
                    result = $"{(int) Path}.{Database}.{fileName}";
                    break;
            }

            if (!ReferenceEquals(Content, null))
            {
                result = result + "&" + IrbisText.WindowsToIrbis(Content);
            }

            return result;

        } // method ToString

        #endregion

   } // class FileSpecification

} // namespace ManagedIrbis.Infrastructure
