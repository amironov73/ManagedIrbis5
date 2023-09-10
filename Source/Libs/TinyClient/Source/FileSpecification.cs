// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* FileSpecification.cs -- спецификация пути к файлу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Спецификация пути к файлу на сервере ИРБИС64.
    /// </summary>
    public sealed class FileSpecification
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
            if (string.IsNullOrEmpty (first) && string.IsNullOrEmpty (second))
            {
                return true;
            }

            return first.SameString (second);
        }

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
            return ((int)path).ToInvariantString()
                   + "."
                   + database
                   + "."
                   + fileName;
        }

        /// <summary>
        /// Parse the text specification.
        /// </summary>
        public static FileSpecification Parse
            (
                string text
            )
        {
            var navigator = new ValueTextNavigator (text.AsSpan());
            var path = int.Parse
                (
                    navigator.ReadTo ('.').ToString(),
                    CultureInfo.InvariantCulture
                );
            var database = navigator.ReadTo ('.').ToString().EmptyToNull();
            var fileName = navigator.GetRemainingText().ToString();
            var binaryFile = fileName.StartsWith ("@");
            if (binaryFile)
            {
                fileName = fileName.Substring (1);
            }

            string? content = null;
            var position = fileName.IndexOf ("&", StringComparison.InvariantCulture);
            if (position >= 0)
            {
                content = fileName.Substring (position + 1);
                fileName = fileName.Substring (0, position);
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
        }

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
            if (ReferenceEquals (other, null))
            {
                throw new ArgumentNullException();
            }

            return Path == other.Path
                   && _CompareDatabases (Database, other.Database)
                   && FileName.SameString (other.FileName);
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals (null, obj))
            {
                return false;
            }

            if (ReferenceEquals (this, obj))
            {
                return true;
            }

            return obj is FileSpecification other && Equals (other);
        }

        /// <inheritdoc cref="object.GetHashCode" />

        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Path;
                hashCode = (hashCode * 397)
                           ^ (Database != null ? Database.GetHashCode() : 0);
                hashCode = (hashCode * 397)
                           ^ (FileName != null ? FileName.GetHashCode() : 0);

                return hashCode;
            }
        }

        // ReSharper restore NonReadonlyMemberInGetHashCode

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
                if (!ReferenceEquals (Content, null))
                {
                    fileName = "&" + fileName;
                }
            }

            string result;

            switch (Path)
            {
                case IrbisPath.System:
                case IrbisPath.Data:
                    result = $"{(int)Path}..{fileName}";
                    break;

                default:
                    result = $"{(int)Path}.{Database}.{fileName}";
                    break;
            }

            if (!ReferenceEquals (Content, null))
            {
                result = result + "&" + Private.WindowsToIrbis (Content);
            }

            return result;
        }

        #endregion
    }
}
