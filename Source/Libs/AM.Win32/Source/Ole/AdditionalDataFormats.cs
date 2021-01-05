// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AdditionalDataFormats.cs -- additional data formats
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Additional OLE data formats.
    /// </summary>
    public static class AdditionalDataFormats
    {
        #region Constants

        /// <summary>
        /// Descriptor of file group.
        /// </summary>
        /// <seealso cref="FILEGROUPDESCRIPTORA"/>
        /// <seealso cref="FILEGROUPDESCRIPTORW"/>
        /// <seealso cref="FILEDESCRIPTORA"/>
        /// <seealso cref="FILEDESCRIPTORW"/>
        public const string FileGroupDescriptor = "FileGroupDescriptor";

        /// <summary>
        /// Descriptor of file group.
        /// </summary>
        /// <seealso cref="FILEGROUPDESCRIPTORA"/>
        /// <seealso cref="FILEGROUPDESCRIPTORW"/>
        /// <seealso cref="FILEDESCRIPTORA"/>
        /// <seealso cref="FILEDESCRIPTORW"/>
        public const string FileGroupDescriptorW = "FileGroupDescriptorW";

        /// <summary>
        /// Uniform Resource Locator (URL).
        /// </summary>
        public const string UniformResourceLocator = "UniformResourceLocator";

        /// <summary>
        /// Uniform Resource Locator (URL).
        /// </summary>
        public const string UniformResourceLocatorW = "UniformResourceLocatorW";

        #endregion

    } // class AdditionalDataFormats

} // namespace AM.Win32
