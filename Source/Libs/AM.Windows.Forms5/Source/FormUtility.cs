// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FormUtility.cs -- полезные методы для работы с формами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Полезные методы для работы с формами.
    /// </summary>
    public static class FormUtility
    {
        #region Public methods

        /// <summary>
        /// Adjusts the placement of the form.
        /// </summary>
        public static Form AdjustPlacement
            (
                this Form form,
                Screen? screen
            )
        {
            const int delta = 1;

            screen ??= Screen.FromControl(form);

            var area = screen.WorkingArea;

            form.Left = Math.Min
                (
                    form.Left,
                    area.Right - form.Width - delta
                );
            form.Left = Math.Max
                (
                    form.Left,
                    delta
                );
            form.Top = Math.Min
                (
                    form.Top,
                    area.Bottom - form.Height - delta
                );
            form.Top = Math.Max
                (
                    form.Top,
                    delta
                );

            return form;

        } // method AdjustPlacement

        /// <summary>
        /// Calculates placement for the window according to
        /// the specified <see cref="WindowPlacement"/>.
        /// </summary>
        public static Point CalculatePlacement
            (
                Size windowSize,
                WindowPlacement placement,
                Size indent
            )
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;
            Point result;

            switch (placement)
            {
                case WindowPlacement.ScreenCenter:
                    result = new Point
                        (
                            workingArea.Left
                                + (workingArea.Width - windowSize.Width) / 2,
                            workingArea.Top
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.TopLeftCorner:
                    result = new Point(indent);
                    break;

                case WindowPlacement.TopRightCorner:
                    result = new Point
                        (
                            workingArea.Right - windowSize.Width - indent.Width,
                            indent.Height
                        );
                    break;

                case WindowPlacement.TopSide:
                    result = new Point
                        (
                            workingArea.Left
                                + (workingArea.Width - windowSize.Width) / 2,
                            indent.Height
                        );
                    break;

                case WindowPlacement.LeftSide:
                    result = new Point
                        (
                            indent.Width,
                            workingArea.Top
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.RightSide:
                    result = new Point
                        (
                            workingArea.Right - windowSize.Width - indent.Width,
                            workingArea.Top
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.BottomSide:
                    result = new Point
                        (
                            workingArea.Left
                                + (workingArea.Width - windowSize.Width) / 2,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                case WindowPlacement.BottomLeftCorner:
                    result = new Point
                        (
                            indent.Width,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                case WindowPlacement.BottomRightCorner:
                    result = new Point
                        (
                            workingArea.Width - windowSize.Width - indent.Width,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement));

            } // switch

            return result;

        } // method CalculatePlacement

        /// <summary>
        /// Show version information in form title.
        /// </summary>
        public static void ShowVersionInfoInTitle
            (
                this Form form
            )
        {
            var assembly = Assembly.GetEntryAssembly();
            var vi = assembly?.GetName().Version;
            if (assembly?.Location is null)
            {
                return;
            }

            var fvi = FileVersionInfo
                .GetVersionInfo(assembly.Location);
            var fi = new FileInfo(assembly.Location);

            // ReSharper disable UseStringInterpolation
            form.Text += string.Format
                (
                    ": version {0} (file {1}) from {2}",
                    vi,
                    fvi.FileVersion,
                    fi.LastWriteTime.ToShortDateString()
                );
            // ReSharper restore UseStringInterpolation
        }

        /// <summary>
        /// Print system information in abstract output.
        /// </summary>
        public static void PrintSystemInformation
            (
                this AbstractOutput? output
            )
        {
            if (output is not null)
            {
                output.WriteLine
                    (
                        "OS version: {0}",
                        Environment.OSVersion
                    );
                output.WriteLine
                    (
                        "Framework version: {0}",
                        Environment.Version
                    );
                var assembly = Assembly.GetEntryAssembly();
                var vi = assembly?.GetName().Version;
                if (assembly?.Location is null)
                {
                    return;
                }

                var fi = new FileInfo(assembly.Location);
                output.WriteLine
                    (
                        "Application version: {0} ({1})",
                        vi.ToVisibleString(),
                        fi.LastWriteTime.ToShortDateString()
                    );
                output.WriteLine
                    (
                        "Memory: {0} Mb",
                        GC.GetTotalMemory(false)/1024
                    );
            }

        } // method PrintSystemInformation

        #endregion

    } // class FormUtility

} // namespace AM.Windows.Forms
