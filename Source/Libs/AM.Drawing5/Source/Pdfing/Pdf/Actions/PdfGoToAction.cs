// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfGoToAction.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Actions;

/// <summary>
/// Represents the base class for all PDF actions.
/// </summary>
public sealed class PdfGoToAction
    : PdfAction
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfGoToAction"/> class.
    /// </summary>
    public PdfGoToAction()
    {
        Inititalize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfGoToAction"/> class.
    /// </summary>
    /// <param name="document">The document that owns this object.</param>
    public PdfGoToAction (PdfDocument document)
        : base (document)
    {
        Inititalize();
    }

    #endregion

    void Inititalize()
    {
        Elements.SetName (PdfAction.Keys.Type, "/Action");
        Elements.SetName (PdfAction.Keys.S, "/Goto");
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal new class Keys
        : PdfAction.Keys
    {
        ///// <summary>
        ///// (Required) The type of action that this dictionary describes;
        ///// must be GoTo for a go-to action.
        ///// </summary>
        //[KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Goto")]
        //public const string S = "/S";

        /// <summary>
        /// (Required) The destination to jump to (see Section 8.2.1, "Destinations").
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.ByteString | KeyType.Array | KeyType.Required)]
        public const string D = "/D";
    }
}
