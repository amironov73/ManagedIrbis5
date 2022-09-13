// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Security;

/// <summary>
/// Encapsulates access to the security settings of a PDF document.
/// </summary>
public sealed class PdfSecuritySettings
{
    #region Construction

    internal PdfSecuritySettings (PdfDocument document)
    {
        _document = document;
    }

    #endregion

    readonly PdfDocument _document;

    /// <summary>
    /// Indicates whether the granted access to the document is 'owner permission'. Returns true if the document
    /// is unprotected or was opened with the owner password. Returns false if the document was opened with the
    /// user password.
    /// </summary>
    public bool HasOwnerPermissions { get; internal set; } = true;

    /// <summary>
    /// Gets or sets the document security level. If you set the security level to anything but PdfDocumentSecurityLevel.None
    /// you must also set a user and/or an owner password. Otherwise saving the document will fail.
    /// </summary>
    public PdfDocumentSecurityLevel DocumentSecurityLevel { get; set; }

    /// <summary>
    /// Sets the user password of the document. Setting a password automatically sets the
    /// PdfDocumentSecurityLevel to PdfDocumentSecurityLevel.Encrypted128Bit if its current
    /// value is PdfDocumentSecurityLevel.None.
    /// </summary>
    public string UserPassword
    {
        set => SecurityHandler.UserPassword = value;
    }

    /// <summary>
    /// Sets the owner password of the document. Setting a password automatically sets the
    /// PdfDocumentSecurityLevel to PdfDocumentSecurityLevel.Encrypted128Bit if its current
    /// value is PdfDocumentSecurityLevel.None.
    /// </summary>
    public string OwnerPassword
    {
        set => SecurityHandler.OwnerPassword = value;
    }

    /// <summary>
    /// Determines whether the document can be saved.
    /// </summary>
    internal bool CanSave (ref string message)
    {
        if (DocumentSecurityLevel != PdfDocumentSecurityLevel.None)
        {
            if (string.IsNullOrEmpty (SecurityHandler._userPassword) &&
                string.IsNullOrEmpty (SecurityHandler._ownerPassword))
            {
                message = PSSR.UserOrOwnerPasswordRequired;
                return false;
            }
        }

        return true;
    }

    #region Permissions

    //TODO: Use documentation from our English Acrobat 6.0 version.

    /// <summary>
    /// Permits printing the document. Should be used in conjunction with PermitFullQualityPrint.
    /// </summary>
    public bool PermitPrint
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitPrint) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitPrint;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitPrint;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits modifying the document.
    /// </summary>
    public bool PermitModifyDocument
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitModifyDocument) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitModifyDocument;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitModifyDocument;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits content copying or extraction.
    /// </summary>
    public bool PermitExtractContent
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitExtractContent) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitExtractContent;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitExtractContent;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits commenting the document.
    /// </summary>
    public bool PermitAnnotations
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitAnnotations) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitAnnotations;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitAnnotations;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits filling of form fields.
    /// </summary>
    public bool PermitFormsFill
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitFormsFill) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitFormsFill;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitFormsFill;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits content extraction for accessibility.
    /// </summary>
    public bool PermitAccessibilityExtractContent
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitAccessibilityExtractContent) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitAccessibilityExtractContent;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitAccessibilityExtractContent;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits to insert, rotate, or delete pages and create bookmarks or thumbnail images even if
    /// PermitModifyDocument is not set.
    /// </summary>
    public bool PermitAssembleDocument
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitAssembleDocument) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitAssembleDocument;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitAssembleDocument;
            }

            SecurityHandler.Permission = permission;
        }
    }

    /// <summary>
    /// Permits to print in high quality. insert, rotate, or delete pages and create bookmarks or thumbnail images
    /// even if PermitModifyDocument is not set.
    /// </summary>
    public bool PermitFullQualityPrint
    {
        get => (SecurityHandler.Permission & PdfUserAccessPermission.PermitFullQualityPrint) != 0;
        set
        {
            var permission = SecurityHandler.Permission;
            if (value)
            {
                permission |= PdfUserAccessPermission.PermitFullQualityPrint;
            }
            else
            {
                permission &= ~PdfUserAccessPermission.PermitFullQualityPrint;
            }

            SecurityHandler.Permission = permission;
        }
    }

    #endregion

    /// <summary>
    /// PdfStandardSecurityHandler is the only implemented handler.
    /// </summary>
    internal PdfStandardSecurityHandler SecurityHandler => _document._trailer.SecurityHandler;
}
