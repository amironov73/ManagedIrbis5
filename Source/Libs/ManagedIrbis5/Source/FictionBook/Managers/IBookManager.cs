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

/* IBookManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using ManagedIrbis.FictionBook.Entities;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook.Managers;

/// <summary>
///
/// </summary>
public interface IBookManager
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    IBook LoadBook
        (
            string fileName
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="book"></param>
    /// <param name="targetFolder"></param>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <param name="deleteOriginal"></param>
    /// <returns></returns>
    FileOperationResult CopyTo
        (
            IBook book,
            string targetFolder,
            RenameProfileElement profile,
            bool useTranslit,
            bool deleteOriginal = false
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="book"></param>
    /// <param name="enc"></param>
    void EncodeTo (IBook book, Encoding enc);

    /// <summary>
    ///
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    List<string> ValidateSchema (IBook book);
}
