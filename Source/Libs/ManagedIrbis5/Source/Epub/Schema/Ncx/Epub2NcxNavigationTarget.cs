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

public class Epub2NcxNavigationTarget
{
    public string Id { get; set; }
    public string Class { get; set; }
    public string Value { get; set; }
    public string PlayOrder { get; set; }
    public List<Epub2NcxNavigationLabel> NavigationLabels { get; set; }
    public Epub2NcxContent Content { get; set; }
}