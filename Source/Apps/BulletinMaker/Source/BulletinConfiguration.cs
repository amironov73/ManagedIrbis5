// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* BulletinConfiguration.cs -- конфигурация Бюллетеня новых поступлений
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
/// Конфигурация Бюллетеня новых поступлений.
/// </summary>
public sealed class BulletinConfiguration
{
    public List<CatalogDescription> Catalogs { get; set; }

    public List<MailingList> MailingLists { get; set; }

    public MailerConfiguration Mailer { get; set; }

    public BulletinConfiguration()
    {
        Catalogs = new List<CatalogDescription>();
        MailingLists = new List<MailingList>();
    }

    public CatalogDescription GetDefaultCatalog()
    {
        return (from catalog in Catalogs
                   where catalog.Default
                   select catalog).FirstOrDefault()
               ?? (from catalog in Catalogs
                   select catalog).First();
    }

    public static BulletinConfiguration GenerateSampleConfiguration()
    {
        var configuration = new BulletinConfiguration();
        var catalog = new CatalogDescription
        {
            Title = "Книги"
        };
        configuration.Catalogs.Add (catalog);
        var report = new ReportDescription
        {
            Title = "Новые поступления",
            ReportClass = "NewBooks"
        };
        catalog.Reports.Add (report);
        catalog = new CatalogDescription
        {
            Title = "Периодика"
        };
        configuration.Catalogs.Add (catalog);
        report = new ReportDescription
        {
            Title = "Новые журналы",
            ReportClass = "NewMagazines"
        };
        catalog.Reports.Add (report);
        report = new ReportDescription
        {
            Title = "Новые статьи",
            ReportClass = "NewArticles"
        };
        catalog.Reports.Add (report);
        return configuration;
    }

    public void Save
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var serializer = new XmlSerializer (typeof (BulletinConfiguration));

        using Stream stream = File.OpenWrite (fileName);
        serializer.Serialize (stream, this);
    }

    public static BulletinConfiguration Load (string fileName)
    {
        var serializer = new XmlSerializer (typeof (BulletinConfiguration));

        using (Stream stream = File.OpenRead (fileName))
        {
            var result = (BulletinConfiguration) serializer
                    .Deserialize (stream);
            return result;
        }
    }
}
