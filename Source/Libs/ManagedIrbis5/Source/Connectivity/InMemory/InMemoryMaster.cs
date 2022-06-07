// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryMaster.cs -- мастер-файл, расположенный в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Мастер-файл, расположенный в оперативной памяти.
/// </summary>
public class InMemoryMaster
    : NonNullCollection<Record>
{
    #region Public methods

    /// <summary>
    /// Дамп мастер-файла.
    /// </summary>
    public void Dump
        (
            TextWriter output
        )
    {
        Sure.NotNull (output);

        // TODO: implement
    }

    /// <summary>
    /// Загрузка из потока.
    /// </summary>
    public void Read
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        // TODO: implement
    }

    /// <summary>
    /// Чтение записи с указанным MFN.
    /// </summary>
    public Record? ReadRecord
        (
            int mfn
        )
    {
        // TODO: клонировать запись при выдаче

        return mfn <= 0 || mfn >= Count ? default : this[mfn - 1];
    }

    /// <summary>
    /// Сохранение в поток.
    /// </summary>
    public void Save
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        // TODO: implement
    }

    /// <summary>
    /// Сохранение/обновление записи.
    /// </summary>
    public bool WriteRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        // TODO: клонировать запись при сохранении

        if (record.Mfn == 0)
        {
            // это новая запись, помещаем ее в конец базы
            Add (record);
            record.Mfn = Count;
        }
        else
        {
            var mfn = record.Mfn;
            if (mfn < 0 || mfn > Count)
            {
                return false;
            }

            this[mfn - 1] = record;
        }

        return true;
    }

    #endregion
}
