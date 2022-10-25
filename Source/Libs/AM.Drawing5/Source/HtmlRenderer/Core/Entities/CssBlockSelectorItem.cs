// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssBlockSelectorItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Holds single class selector in css block hierarchical selection (p class1 > div.class2)
/// </summary>
public readonly struct CssBlockSelectorItem
{
    /// <summary>
    /// Creates a new block from the block's source
    /// </summary>
    /// <param name="className">the name of the css class of the block</param>
    /// <param name="directParent"> </param>
    public CssBlockSelectorItem
        (
            string className,
            bool directParent
        )
    {
        Sure.NotNullNorEmpty (className);

        ClassName = className;
        DirectParent = directParent;
    }

    /// <summary>
    /// the name of the css class of the block
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// is the selector item has to be direct parent
    /// </summary>
    public bool DirectParent { get; }

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    public override string ToString()
    {
        return ClassName + (DirectParent ? " > " : string.Empty);
    }
}
