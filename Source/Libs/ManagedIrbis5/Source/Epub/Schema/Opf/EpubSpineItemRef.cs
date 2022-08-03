// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

public class EpubSpineItemRef
{
    public string Id { get; set; }
    public string IdRef { get; set; }
    public bool IsLinear { get; set; }
    public List<SpineProperty> Properties { get; set; }

    public override string ToString()
    {
        StringBuilder resultBuilder = new StringBuilder();
        if (Id != null)
        {
            resultBuilder.Append("Id: ");
            resultBuilder.Append(Id);
            resultBuilder.Append("; ");
        }
        resultBuilder.Append("IdRef: ");
        resultBuilder.Append(IdRef ?? "(null)");
        return resultBuilder.ToString();
    }
}