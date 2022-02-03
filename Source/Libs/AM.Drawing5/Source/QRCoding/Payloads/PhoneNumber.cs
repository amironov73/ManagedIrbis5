// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Номер телефона.
/// </summary>
public class PhoneNumber
    : Payload
{
    #region Construction

    /// <summary>
    /// Generates a phone call payload
    /// </summary>
    /// <param name="number">Phonenumber of the receiver</param>
    public PhoneNumber
        (
            string number
        )
    {
        Sure.NotNullNorEmpty (number);

        _number = number;
    }

    #endregion

    #region Private members

    private readonly string _number;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"tel:{_number}";
    }

    #endregion
}
