// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* CommandParameterCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents the collection of <see cref="CommandParameter"/> objects.
/// </summary>
/// <remarks>
/// This class is used to store the list of parameters defined in the datasource. See the
/// <see cref="TableDataSource.Parameters"/> property for more details.
/// </remarks>
public class CommandParameterCollection
    : ReportCollectionBase
{
    #region Properties

    /// <summary>
    /// Gets or sets a parameter.
    /// </summary>
    /// <param name="index">The index of a parameter in this collection.</param>
    /// <returns>The parameter with specified index.</returns>
    public CommandParameter this [int index]
    {
        get => (CommandParameter) List[index]!;
        set => List[index] = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameterCollection"/>
    /// class with default settings.
    /// </summary>
    /// <param name="owner">The owner of this collection.</param>
    public CommandParameterCollection
        (
            Base owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Finds a parameter by its name.
    /// </summary>
    /// <param name="name">The name of a parameter.</param>
    /// <returns>The <see cref="CommandParameter"/> object if found; otherwise <b>null</b>.</returns>
    public CommandParameter? FindByName
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        foreach (CommandParameter c in this)
        {
            if (c.Name == name)
            {
                return c;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns an unique parameter name based on given name.
    /// </summary>
    /// <param name="name">The base name.</param>
    /// <returns>The unique name.</returns>
    public string CreateUniqueName
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var baseName = name;
        var i = 1;
        while (FindByName (name) != null)
        {
            name = baseName + i.ToString();
            i++;
        }

        return name;
    }

    #endregion
}
