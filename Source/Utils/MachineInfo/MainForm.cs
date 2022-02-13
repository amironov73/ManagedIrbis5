// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo

/* MainForm.cs -- main application form
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualBasic.Devices;

#endregion

#nullable enable

namespace MachineInfo;

/// <summary>
/// Main application form.
/// </summary>
public sealed partial class MainForm
    : Form
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();

        DiscoverSystem();
        DiscoverNetwork();
        DiscoverMemory();
        DiscoverDrives();
    }

    private void AddLine
        (
            ListViewGroup group,
            string name,
            string value
        )
    {
        var item = new ListViewItem(new [] { name, value });
        group.Items.Add(item);
        _listView.Items.Add(item);
    }

    private void DiscoverSystem()
    {
        var systemGroup = _listView.Groups["System"];
        AddLine (systemGroup, "Operating system", RuntimeInformation.OSDescription);
        AddLine (systemGroup, "OS architecture", RuntimeInformation.OSArchitecture.ToString());
        AddLine (systemGroup, "Framework", RuntimeInformation.FrameworkDescription);
        AddLine (systemGroup, "Runtime", RuntimeInformation.RuntimeIdentifier);
        AddLine (systemGroup, "Logged in user", Environment.UserName);
    }

    private void DiscoverNetwork()
    {
        var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        var networkGroup = _listView.Groups["Network"];
        AddLine (networkGroup, "Host name", hostEntry.HostName);

        foreach (var address in hostEntry.AddressList)
        {
            AddLine (networkGroup, "Address",  address.ToString()) ;
        }
    }

    private void DiscoverMemory()
    {
        const long megabyte = 1024 * 1024;
        var info = new ComputerInfo();
        var memoryGroup = _listView.Groups["Memory"];
        AddLine (memoryGroup, "Total physical memory",
            $"{(info.TotalPhysicalMemory / megabyte):N0} Mb");
        AddLine (memoryGroup, "Available physical memory",
            $"{(info.AvailablePhysicalMemory / megabyte):N0} Mb");
        AddLine(memoryGroup, "Available virtual memory",
            $"{(info.AvailableVirtualMemory / megabyte):N0} Mb");
    }

    private void DiscoverDrives()
    {
        const double gigabyte = 1024 * 1024 * 1024;
        var info = new ServerComputer();
        var driveGroup = _listView.Groups["Drives"];
        foreach (var drive in info.FileSystem.Drives)
        {
            if (drive.DriveType == DriveType.Fixed)
            {
                var driveName = drive.Name;
                var driveDescription = $"Total: {(drive.TotalSize / gigabyte):N} Gb, " +
                                       $"available: {(drive.AvailableFreeSpace / gigabyte):N} Gb";
                AddLine (driveGroup, driveName, driveDescription);
            }
        }
    }

    private void _listView_DoubleClick
        (
            object sender,
            EventArgs e
        )
    {
        Clipboard.SetText (GatherInformation());
    }

    private void _listView_ClientSizeChanged
        (
            object sender,
            EventArgs e
        )
    {
        _valueColumn.Width = _listView.ClientSize.Width - _nameColumn.Width;
    }

    private static void GatherGroup
        (
            StringBuilder builder,
            ListViewGroup group
        )
    {
        builder.AppendLine (group.Header);
        builder.AppendLine ();

        foreach (ListViewItem item in group.Items)
        {
            builder.AppendFormat
                (
                    "{0,-30} {1}",
                    item.SubItems[0].Text,
                    item.SubItems[1].Text
                );
            builder.AppendLine ();
        }

        builder.AppendLine (new string ('-', 70));
    }

    private string GatherInformation()
    {
        var builder = new StringBuilder();

        foreach (ListViewGroup group in _listView.Groups)
        {
            GatherGroup (builder, group);
            builder.AppendLine ();
        }

        return builder.ToString();
    }
}
