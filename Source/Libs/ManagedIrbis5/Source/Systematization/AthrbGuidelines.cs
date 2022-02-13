// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthrbGuidelines.cs -- методические рекомендации / описания в базе ATHRB, поле 300
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Методические рекомендации / описания в базе ATHRB.
/// Поле 300.
/// </summary>
public sealed class AthrbGuidelines
{
    #region Properties

    /// <summary>
    /// Методические рекомендации / описания.
    /// Подполе a.
    /// </summary>
    [SubField ('a')]
    public string? Guidelines { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Parse the field.
    /// </summary>
    public static AthrbGuidelines Parse
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new AthrbGuidelines
        {
            Guidelines = field.GetFirstSubFieldValue ('a')
        };

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Guidelines.ToVisibleString();
    }

    #endregion
}
