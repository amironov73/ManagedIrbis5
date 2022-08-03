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

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

public class EpubManifestItem
{
    public string Id { get; set; }
    public string Href { get; set; }
    public string MediaType { get; set; }
    public string RequiredNamespace { get; set; }
    public string RequiredModules { get; set; }
    public string Fallback { get; set; }
    public string FallbackStyle { get; set; }
    public List<ManifestProperty> Properties { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Href = {Href}, MediaType = {MediaType}";
    }
}