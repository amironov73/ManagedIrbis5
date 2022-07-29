// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Windows.Forms.Dialogs.Interop;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs;

class VistaFileDialogEvents : IFileDialogEvents, IFileDialogControlEvents
{
    const uint S_OK = 0;
    const uint S_FALSE = 1;
    const uint E_NOTIMPL = 0x80004001;

    private VistaFileDialog _dialog;

    public VistaFileDialogEvents (VistaFileDialog dialog)
    {
        if (dialog == null)
        {
            throw new ArgumentNullException ("dialog");
        }

        _dialog = dialog;
    }

    #region IFileDialogEvents Members

    public HRESULT OnFileOk (IFileDialog pfd)
    {
        if (_dialog.DoFileOk (pfd))
        {
            return HRESULT.S_OK;
        }

        return HRESULT.S_FALSE;
    }

    public HRESULT OnFolderChanging (IFileDialog pfd, IShellItem psiFolder)
    {
        return HRESULT.S_OK;
    }

    public void OnFolderChange (IFileDialog pfd)
    {
    }

    public void OnSelectionChange (IFileDialog pfd)
    {
    }

    public void OnShareViolation (IFileDialog pfd, IShellItem psi,
        out NativeMethods.FDE_SHAREVIOLATION_RESPONSE pResponse)
    {
        pResponse = NativeMethods.FDE_SHAREVIOLATION_RESPONSE.FDESVR_DEFAULT;
    }

    public void OnTypeChange (IFileDialog pfd)
    {
    }

    public void OnOverwrite (IFileDialog pfd, IShellItem psi, out NativeMethods.FDE_OVERWRITE_RESPONSE pResponse)
    {
        pResponse = NativeMethods.FDE_OVERWRITE_RESPONSE.FDEOR_DEFAULT;
    }

    #endregion

    #region IFileDialogControlEvents Members

    public void OnItemSelected (IFileDialogCustomize pfdc, int dwIDCtl, int dwIDItem)
    {
    }

    public void OnButtonClicked (IFileDialogCustomize pfdc, int dwIDCtl)
    {
        if (dwIDCtl == VistaFileDialog.HelpButtonId)
        {
            _dialog.DoHelpRequest();
        }
    }

    public void OnCheckButtonToggled (IFileDialogCustomize pfdc, int dwIDCtl, bool bChecked)
    {
    }

    public void OnControlActivating (IFileDialogCustomize pfdc, int dwIDCtl)
    {
    }

    #endregion
}