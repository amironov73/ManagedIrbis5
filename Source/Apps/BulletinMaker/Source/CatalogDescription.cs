// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CatalogDescription.cs -- описание электронного каталога
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using AM;
using AM.AppServices;
using AM.Windows.Forms;

using ManagedIrbis.AppServices;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace BulletinMaker;

/// <summary>
/// Описание электронного каталога.
/// </summary>
public sealed class CatalogDescription
{
    [XmlAttribute("default")]
    public bool Default { get; set; }

    [XmlElement("title")]
    public string Title { get; set; }

    [XmlElement("path")]
    public string Path { get; set; }

    [XmlElement("filter")]
    public string Filter { get; set; }

    [XmlElement("report")]
    public List<ReportDescription> Reports { get; set; }

    [XmlAttribute ("is64")]
    public bool Is64 { get; set; }

    public CatalogDescription()
    {
        Reports = new List<ReportDescription>();
    }

    public ReportDescription GetDefaultReport ()
    {
        return (from report in Reports
                   where report.Default
                   select report).FirstOrDefault()
               ?? (from report in Reports
                   select report).First();
    }

    public override string ToString()
    {
        return Title;
    }
}
