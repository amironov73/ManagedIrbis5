// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EpubSpineItemRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubSpineItemRef
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? IdRef { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsLinear { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<SpineProperty>? Properties { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        if (Id != null)
        {
            builder.Append ("Id: ");
            builder.Append (Id);
            builder.Append ("; ");
        }

        builder.Append ("IdRef: ");
        builder.Append (IdRef ?? "(null)");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
