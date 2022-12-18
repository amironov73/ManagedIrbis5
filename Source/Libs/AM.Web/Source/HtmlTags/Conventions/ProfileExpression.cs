// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ProfileExpression.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

#region Using directives

using Elements;

#endregion

#nullable enable

/// <summary>
///
/// </summary>
public class ProfileExpression
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    protected HtmlConventionLibrary Library { get; set; }

    /// <summary>
    ///
    /// </summary>
    public ElementCategoryExpression Labels =>
        new (BuildersFor (ElementConstants.Label));

    /// <summary>
    ///
    /// </summary>
    public ElementCategoryExpression Displays =>
        new (BuildersFor (ElementConstants.Display));

    /// <summary>
    ///
    /// </summary>
    public ElementCategoryExpression Editors =>
        new (BuildersFor (ElementConstants.Editor));

    /// <summary>
    ///
    /// </summary>
    public ElementCategoryExpression ValidationMessages =>
        new (BuildersFor (ElementConstants.ValidationMessage));

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    /// <param name="profileName"></param>
    public ProfileExpression
        (
            HtmlConventionLibrary library,
            string profileName
        )
    {
        Sure.NotNull (library);
        Sure.NotNull (profileName);

        Library = library;
        _profileName = profileName;
    }

    #endregion

    #region Private members

    private readonly string _profileName;

    private BuilderSet BuildersFor (string category)
    {
        return Library.TagLibrary.Category (category).Profile (_profileName);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    public void Apply
        (
            HtmlConventionLibrary library
        )
    {
        Sure.NotNull (library);

        library.Import (Library);
    }

    #endregion
}
