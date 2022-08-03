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

namespace ManagedIrbis.Epub;

public enum EpubSchemaFileType
{
    UNKNOWN = 0,
    META_INF_CONTAINER,
    OPF_PACKAGE,
    EPUB2_NCX,
    EPUB3_NAV_DOCUMENT
}