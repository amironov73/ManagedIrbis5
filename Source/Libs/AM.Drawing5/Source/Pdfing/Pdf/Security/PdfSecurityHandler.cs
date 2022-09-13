// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Security;

/// <summary>
/// Represents the base of all security handlers.
/// </summary>
public abstract class PdfSecurityHandler
    : PdfDictionary
{
    #region Construction

    internal PdfSecurityHandler (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    internal PdfSecurityHandler (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal class Keys
        : KeysBase
    {
        /// <summary>
        /// (Required) The name of the preferred security handler for this document. Typically,
        /// it is the name of the security handler that was used to encrypt the document. If
        /// SubFilter is not present, only this security handler should be used when opening
        /// the document. If it is present, consumer applications can use any security handler
        /// that implements the format specified by SubFilter.
        /// Standard is the name of the built-in password-based security handler. Names for other
        /// security handlers can be registered by using the procedure described in Appendix E.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required)]
        public const string Filter = "/Filter";

        /// <summary>
        /// (Optional; PDF 1.3) A name that completely specifies the format and interpretation of
        /// the contents of the encryption dictionary. It is needed to allow security handlers other
        /// than the one specified by Filter to decrypt the document. If this entry is absent, other
        /// security handlers should not be allowed to decrypt the document.
        /// </summary>
        [KeyInfo ("1.3", KeyType.Name | KeyType.Optional)]
        public const string SubFilter = "/SubFilter";

        /// <summary>
        /// (Optional but strongly recommended) A code specifying the algorithm to be used in encrypting
        /// and decrypting the document:
        /// 0 An algorithm that is undocumented and no longer supported, and whose use is strongly discouraged.
        /// 1 Algorithm 3.1, with an encryption key length of 40 bits.
        /// 2 (PDF 1.4) Algorithm 3.1, but permitting encryption key lengths greater than 40 bits.
        /// 3 (PDF 1.4) An unpublished algorithm that permits encryption key lengths ranging from 40 to 128 bits.
        /// 4 (PDF 1.5) The security handler defines the use of encryption and decryption in the document, using
        ///             the rules specified by the CF, StmF, and StrF entries.
        /// The default value if this entry is omitted is 0, but a value of 1 or greater is strongly recommended.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Optional)]
        public const string V = "/V";

        /// <summary>
        /// (Optional; PDF 1.4; only if V is 2 or 3) The length of the encryption key, in bits.
        /// The value must be a multiple of 8, in the range 40 to 128. Default value: 40.
        /// </summary>
        [KeyInfo ("1.4", KeyType.Integer | KeyType.Optional)]
        public const string Length = "/Length";

        /// <summary>
        /// (Optional; meaningful only when the value of V is 4; PDF 1.5)
        /// A dictionary whose keys are crypt filter names and whose values are the corresponding
        /// crypt filter dictionaries. Every crypt filter used in the document must have an entry
        /// in this dictionary, except for the standard crypt filter names.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional)]
        public const string CF = "/CF";

        /// <summary>
        /// (Optional; meaningful only when the value of V is 4; PDF 1.5)
        /// The name of the crypt filter that is used by default when decrypting streams.
        /// The name must be a key in the CF dictionary or a standard crypt filter name. All streams
        /// in the document, except for cross-reference streams or streams that have a Crypt entry in
        /// their Filter array, are decrypted by the security handler, using this crypt filter.
        /// Default value: Identity.
        /// </summary>
        [KeyInfo ("1.5", KeyType.Name | KeyType.Optional)]
        public const string StmF = "/StmF";

        /// <summary>
        /// (Optional; meaningful only when the value of V is 4; PDF 1.)
        /// The name of the crypt filter that is used when decrypting all strings in the document.
        /// The name must be a key in the CF dictionary or a standard crypt filter name.
        /// Default value: Identity.
        /// </summary>
        [KeyInfo ("1.5", KeyType.Name | KeyType.Optional)]
        public const string StrF = "/StrF";

        /// <summary>
        /// (Optional; meaningful only when the value of V is 4; PDF 1.6)
        /// The name of the crypt filter that should be used by default when encrypting embedded
        /// file streams; it must correspond to a key in the CF dictionary or a standard crypt
        /// filter name. This entry is provided by the security handler. Applications should respect
        /// this value when encrypting embedded files, except for embedded file streams that have
        /// their own crypt filter specifier. If this entry is not present, and the embedded file
        /// stream does not contain a crypt filter specifier, the stream should be encrypted using
        /// the default stream crypt filter specified by StmF.
        /// </summary>
        [KeyInfo ("1.6", KeyType.Name | KeyType.Optional)]
        public const string EFF = "/EFF";
    }
}
