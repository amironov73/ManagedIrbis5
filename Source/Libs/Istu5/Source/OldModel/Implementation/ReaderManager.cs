// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ReaderManager.cs -- менеджер базы читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

using Istu.OldModel.Interfaces;

using LinqToDB;
using LinqToDB.Data;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation;

/// <summary>
/// Менеджер базы читателей для книговыдачи.
/// </summary>
public sealed class ReaderManager
    : IReaderManager
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReaderManager
        (
            Storehouse storehouse
        )
    {
        Sure.NotNull (storehouse);

        _storehouse = storehouse;
    }

    #endregion

    #region Private members

    private readonly Storehouse _storehouse;
    private DataConnection? _dataConnection;

    private DataConnection _GetDb() => _dataConnection ??= _storehouse.GetKladovka();

    #endregion

    #region IReaderManager members

    /// <inheritdoc cref="IReaderManager.CreateReader"/>
    public int CreateReader
        (
            Reader reader
        )
    {
        // TODO верифицировать читателя

        var db = _GetDb();
        var result = db.Insert (reader);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByTicket"/>
    public Reader? GetReaderByTicket
        (
            string ticket
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.Ticket == ticket);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByTicketAndPassword"/>
    public Reader? GetReaderByTicketAndPassword
        (
            string ticket,
            string password
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.Ticket == ticket);
        if (result is not null)
        {
            // если пароль не совпадает, считаем, что читатель не найден
            if (string.CompareOrdinal (result.Password, password) != 0)
            {
                result = null;
            }
        }

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByBarcode"/>
    public Reader? GetReaderByBarcode
        (
            string barcode
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.Barcode == barcode);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByIstuId"/>
    public Reader? GetReaderByIstuId
        (
            int id
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.IstuID == id);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByRfid"/>
    public Reader? GetReaderByRfid
        (
            string rfid
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.Rfid == rfid);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByEmail"/>
    public Reader? GetReaderByEmail
        (
            string email
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.Mail == email);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.GetReaderByTelegramId"/>
    public Reader? GetReaderByTelegramId
        (
            long telegramId
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.FirstOrDefault (reader => reader.TelegramId == telegramId);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.UpdateReaderInfo"/>
    public void UpdateReaderInfo
        (
            Reader reader
        )
    {
        var db = _GetDb();
        db.Update (reader);
    }

    /// <inheritdoc cref="IReaderManager.Reregister"/>
    public void Reregister
        (
            string ticket
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var year = (short)DateTime.Today.Year;
        readers.Where (reader => reader.Ticket == ticket)
            .Set (reader => reader.Reregistered, year)
            .Update();
    }

    /// <inheritdoc cref="IReaderManager.DeleteReader"/>
    public void DeleteReader
        (
            string ticket
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        readers.Delete (reader => reader.Ticket == ticket);
    }

    /// <inheritdoc cref="IReaderManager.CheckExistence"/>
    public bool CheckExistence
        (
            string ticket
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.Count (reader => reader.Ticket == ticket) != 0;

        return result;
    }

    /// <inheritdoc cref="IReaderManager.ValidateTicketString"/>
    public bool ValidateTicketString
        (
            string ticket
        )
    {
        var result = Regex.IsMatch (@"[0-9A-Za-zа-яА-Я\-]+", ticket);

        return result;
    }

    /// <inheritdoc cref="IReaderManager.ValidateNameString"/>
    public bool ValidateNameString
        (
            string name
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IReaderManager.VerifyPassword"/>
    public bool VerifyPassword
        (
            string ticket,
            string password
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var reader = readers.FirstOrDefault (reader => reader.Ticket == ticket);
        var result = reader is not null && string.CompareOrdinal (reader.Ticket, password) == 0;

        return result;
    }

    /// <inheritdoc cref="IReaderManager.FindReaders"/>
    public Reader[] FindReaders
        (
            ReaderSearchCriteria criteria,
            string mask,
            int max
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var column = criteria switch
        {
            ReaderSearchCriteria.Name => "name",
            ReaderSearchCriteria.Ticket => "ticket",
            ReaderSearchCriteria.Barcode => "barcode",
            ReaderSearchCriteria.Rfid => "rfid",
            ReaderSearchCriteria.Group => "group",
            ReaderSearchCriteria.Category => "category",
            ReaderSearchCriteria.Department => "department",
            _ => throw new ArgumentException (nameof (criteria))
        };
        var result = db.Query<Reader>
            (
                $"select top {max} * from {readers.TableName} where [{column}] like @mask order by [name]",
                new DataParameter ("mask", mask)
            );

        return result.ToArray();
    }

    /// <inheritdoc cref="IReaderManager.Search"/>
    public Reader[] Search
        (
            string expression
        )
    {
        var db = _GetDb();
        var result = db.Query<Reader> (expression);

        return result.ToArray();
    }

    /// <inheritdoc cref="IReaderManager.GetPhoto"/>
    public byte[]? GetPhoto
        (
            string ticket
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.Where (reader => reader.Ticket == ticket)
            .Select (reader => reader.Photo)
            .FirstOrDefault();

        return result;
    }

    /// <inheritdoc cref="IReaderManager.SetPhoto"/>
    public void SetPhoto
        (
            string ticket,
            byte[]? photo
        )
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        readers.Where (reader => reader.Ticket == ticket)
            .Set (reader => reader.Photo, photo)
            .Update();
    }

    /// <inheritdoc cref="IReaderManager.ExportPhoto"/>
    public void ExportPhoto
        (
            string path
        )
    {
        if (!Directory.Exists (path))
        {
            Directory.CreateDirectory (path);
        }

        var db = _GetDb();
        var readers = db.GetReaders();
        var havePhoto = readers.Where (reader => reader.Photo != null);
        foreach (var reader in havePhoto)
        {
            var fileName = Path.Combine (path, reader.Ticket + ".jpg");
            File.WriteAllBytes (fileName, reader.Photo!);
        }
    }

    /// <inheritdoc cref="IReaderManager.GetDopplers"/>
    public string[] GetDopplers()
    {
        var db = _GetDb();
        var readers = db.GetReaders();
        var result = readers.GroupBy (reader => reader.Name)
            .Where (group => group.Count() > 1)
            .Select (group => group.Key)
            .ToArray();

        return result!;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (_dataConnection is not null)
        {
            _dataConnection.Dispose();
            _dataConnection = null;
        }
    } // method Dispose

    #endregion
}
