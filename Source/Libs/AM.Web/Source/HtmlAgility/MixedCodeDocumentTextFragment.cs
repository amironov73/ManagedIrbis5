// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents a fragment of text in a mixed code document.
/// </summary>
public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
{
    #region Constructors

    internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
        :
        base(doc, MixedCodeDocumentFragmentType.Text)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the fragment text.
    /// </summary>
    public string Text
    {
        get { return FragmentText; }
        set { FragmentText = value; }
    }

    #endregion
}
