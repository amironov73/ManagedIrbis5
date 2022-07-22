// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* TreeFile.cs -- работа с TRE-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

using Sure = AM.Sure;

#endregion

#nullable enable

namespace ManagedIrbis.Trees;

/// <summary>
/// Работа с TRE-файлами.
/// </summary>
public sealed class TreeFile
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Tabulation
    /// </summary>
    public const char Indent = '\x09';

    #endregion

    #region Properties

    /// <summary>
    /// File name.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Root items.
    /// </summary>
    public NonNullCollection<TreeLine> Roots { get; } = new ();

    #endregion

    #region Private members

    /// <summary>
    /// Determines indent level of the string.
    /// </summary>
    private static int CountIndent
        (
            string line
        )
    {
        var result = 0;

        foreach (var c in line)
        {
            if (c == Indent)
            {
                result++;
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private static int _ArrangeLevel
        (
            List<TreeLine> items,
            int level,
            int index,
            int count
        )
    {
        var next = index + 1;
        var level2 = level + 1;

        while (next < count)
        {
            if (items[next].Level <= level)
            {
                break;
            }

            if (items[next].Level == level2)
            {
                items[index].Children.Add (items[next]);
            }

            next++;
        }

        return next;
    }

    private static void _ArrangeLevel
        (
            List<TreeLine> items,
            int level
        )
    {
        var count = items.Count;
        var index = 0;

        while (index < count)
        {
            var next = _ArrangeLevel
                (
                    items,
                    level,
                    index,
                    count
                );

            index = next;
        }
    }

    private static void _WriteLevel
        (
            TextWriter writer,
            NonNullCollection<TreeLine> items,
            int level
        )
    {
        foreach (var item in items)
        {
            for (var i = 0; i < level; i++)
            {
                writer.Write (Indent);
            }

            writer.WriteLine (item.Value);

            _WriteLevel
                (
                    writer,
                    item.Children,
                    level + 1
                );
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Add root item.
    /// </summary>
    public TreeLine AddRoot
        (
            string? value
        )
    {
        var result = new TreeLine (value);
        Roots.Add (result);

        return result;
    }

    /// <summary>
    /// Parse specified stream.
    /// </summary>
    public static TreeFile ParseStream
        (
            TextReader reader
        )
    {
        var result = new TreeFile();

        var list = new List<TreeLine>();
        var line = reader.ReadLine();
        if (ReferenceEquals (line, null))
        {
            goto DONE;
        }

        if (CountIndent (line) != 0)
        {
            Magna.Logger.LogError
                (
                    nameof (TreeFile) + "::" + nameof (ParseStream)
                    + ": indent != 0"
                );

            throw new FormatException();
        }

        list.Add (new TreeLine (line));

        var currentLevel = 0;
        while ((line = reader.ReadLine()) != null)
        {
            var level = CountIndent (line);
            if (level > currentLevel + 1)
            {
                Magna.Logger.LogError
                    (
                        nameof (TreeFile) + "::" + nameof (ParseStream)
                        + ": level > currentLevel + 1"
                    );

                throw new FormatException();
            }

            currentLevel = level;
            line = line.TrimStart (Indent);
            var item = new TreeLine (line)
            {
                Level = currentLevel
            };
            list.Add (item);
        }

        var maxLevel = list.Max (item => item.Level);
        for (var level = 0; level < maxLevel; level++)
        {
            _ArrangeLevel (list, level);
        }

        var roots = list.Where (item => item.Level == 0);
        result.Roots.AddRange (roots);

        DONE:
        return result;
    }

    /// <summary>
    /// Save to text stream.
    /// </summary>
    public void Save
        (
            TextWriter writer
        )
    {
        _WriteLevel
            (
                writer,
                Roots,
                0
            );
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        FileName = reader.ReadNullableString();
        reader.ReadCollection (Roots);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (FileName);
        writer.Write (Roots);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwException
        )
    {
        var result = Roots.Count != 0 && Roots.All
            (
                root => root.Verify (throwException)
            );

        return result;
    }

    #endregion
}
