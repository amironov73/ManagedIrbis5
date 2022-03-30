// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ReportDescription.cs -- описание отчета
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
/// Описание отчета.
/// </summary>
public sealed class ReportDescription
{
    /// <summary>
    ///
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ReportClass { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<GroupDescription> Groups { get; }

    /// <summary>
    ///
    /// </summary>
    public List<string> MailTo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? MailTemplate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Brief { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool DontSort { get; set; }

    public ReportDescription()
    {
        Groups = new List<GroupDescription>();
        MailTo = new List<string>();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Title.ToVisibleString();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<string> ResolveMailAddress ()
    {
        throw new NotImplementedException();

        // foreach (string alias in MailTo)
        // {
        //     if (alias.Contains("@"))
        //     {
        //         yield return alias;
        //     }
        //     else
        //     {
        //         bool found = false;
        //         foreach (var list in GlobalConfiguration.Instance.MailingLists)
        //         {
        //             if (list.Id == alias)
        //             {
        //                 found = true;
        //                 foreach (string address in list.Addresses)
        //                 {
        //                     yield return address;
        //                 }
        //             }
        //         }
        //         if (!found)
        //         {
        //             throw new ApplicationException("Bad list.id");
        //         }
        //     }
        // }
    }

    public string ProcessTemplate ()
    {
        throw new NotImplementedException();

        //return MailTemplate;
    }

}
