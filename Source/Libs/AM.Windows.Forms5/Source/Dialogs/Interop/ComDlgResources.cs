// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Dialogs.Interop;

#nullable enable

static class ComDlgResources
{
    public enum ComDlgResourceId
    {
        OpenButton = 370,
        Open = 384,
        FileNotFound = 391,
        CreatePrompt = 402,
        ReadOnly = 427,
        ConfirmSaveAs = 435
    }

    private static Win32Resources _resources = new Win32Resources ("comdlg32.dll");

    public static string LoadString (ComDlgResourceId id)
    {
        return _resources.LoadString ((uint)id);
    }

    public static string FormatString (ComDlgResourceId id, params string[] args)
    {
        return _resources.FormatString ((uint)id, args);
    }
}