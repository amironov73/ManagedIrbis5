// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// Resource identifiers for default animations from shell32.dll.
/// </summary>
[SuppressMessage ("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
public enum ShellAnimation
{
    /// <summary>
    /// An animation representing a file move.
    /// </summary>
    FileMove = 160,

    /// <summary>
    /// An animation representing a file copy.
    /// </summary>
    FileCopy = 161,

    /// <summary>
    /// An animation showing flying papers.
    /// </summary>
    FlyingPapers = 165,

    /// <summary>
    /// An animation showing a magnifying glass over a globe.
    /// </summary>
    SearchGlobe = 166,

    /// <summary>
    /// An animation representing a permament delete.
    /// </summary>
    PermanentDelete = 164,

    /// <summary>
    /// An animation representing deleting an item from the recycle bin.
    /// </summary>
    FromRecycleBinDelete = 163,

    /// <summary>
    /// An animation representing a file move to the recycle bin.
    /// </summary>
    ToRecycleBinDelete = 162,

    /// <summary>
    /// An animation representing a search spanning the local computer.
    /// </summary>
    SearchComputer = 152,

    /// <summary>
    /// An animation representing a search in a document..
    /// </summary>
    SearchDocument = 151,

    /// <summary>
    /// An animation representing a search using a flashlight animation.
    /// </summary>
    SearchFlashlight = 150,
}