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

public class SkypeCall : Payload
{
    private readonly string skypeUsername;

    /// <summary>
    /// Generates a Skype call payload
    /// </summary>
    /// <param name="skypeUsername">Skype username which will be called</param>
    public SkypeCall (string skypeUsername)
    {
        this.skypeUsername = skypeUsername;
    }

    public override string ToString()
    {
        return $"skype:{skypeUsername}?call";
    }
}
