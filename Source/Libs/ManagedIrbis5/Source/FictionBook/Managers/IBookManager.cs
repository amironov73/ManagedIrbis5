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

#region Using directives

using System.Collections.Generic;
using System.Text;

using ManagedIrbis.FictionBook.Entities;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook.Managers;

public interface IBookManager
{
    IBook LoadBook(string fileName);
    FileOperationResult CopyTo(IBook book, string targetFolder, RenameProfileElement profile, bool useTranslit, bool deleteOriginal = false);
    void EncodeTo(IBook book, Encoding enc);
    List<string> ValidateSchema(IBook book);
}
