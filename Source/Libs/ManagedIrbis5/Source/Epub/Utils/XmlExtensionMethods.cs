// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* XmlExtensionMethods.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Utils;

/// <summary>
///
/// </summary>
internal static class XmlExtensionMethods
{
    /// <summary>
    ///
    /// </summary>
    public static string GetLowerCaseLocalName
        (
            this XAttribute xAttribute
        )
    {
        return xAttribute.Name.LocalName.ToLowerInvariant();
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetLowerCaseLocalName
        (
            this XElement xElement
        )
    {
        return xElement.Name.LocalName.ToLowerInvariant();
    }

    /// <summary>
    ///
    /// </summary>
    public static bool CompareNameTo
        (
            this XElement xElement,
            string value
        )
    {
        return xElement.Name.LocalName.SameString (value);
    }

    /// <summary>
    ///
    /// </summary>
    public static bool CompareValueTo
        (
            this XAttribute xAttribute,
            string value
        )
    {
        return xAttribute.Value.SameString (value);
    }
}
