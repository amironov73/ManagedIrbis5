// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Reader.cs -- информация о читателе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.OldModel;

/// <summary>
/// Информация о читателе.
/// </summary>
[PublicAPI]
[Table (Name = "readers")]
public sealed class Reader
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Column, PrimaryKey, Identity]
    public int ID { get; set; }

    /// <summary>
    /// Фамилия, имя, отчество читателя.
    /// </summary>
    [Column]
    public string? Name { get; set; }

    /// <summary>
    /// Категория: студент, преподаватель, сотрудник.
    /// </summary>
    [Column, Nullable]
    public string? Category { get; set; }

    /// <summary>
    /// Подразделение, факультет.
    /// </summary>
    [Column, Nullable]
    public string? Department { get; set; }

    /// <summary>
    /// Лаборатория, отдел.
    /// </summary>
    [Column, Nullable]
    public string? Laboratory { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [Column]
    public string? Ticket { get; set; }

    /// <summary>
    /// Дата регистрации в системе книговыдачи.
    /// </summary>
    [Column, Nullable]
    public string? Registered { get; set; }

    /// <summary>
    /// Год перерегистрации.
    /// </summary>
    [Column]
    public short Reregistered { get; set; }

    /// <summary>
    /// Адрес прописки.
    /// </summary>
    [Column, Nullable]
    public string? Address { get; set; }

    /// <summary>
    /// Студенческая группа.
    /// </summary>
    [Column, Nullable]
    public string? Group { get; set; }

    /// <summary>
    /// Признак задолженности за пределами
    /// системы книговыдачи.
    /// </summary>
    [Column]
    public bool Debtor { get; set; }

    /// <summary>
    /// Верифицирован?
    /// </summary>
    [Column]
    public bool Verified { get; set; }

    /// <summary>
    /// Штрих-код читательского.
    /// </summary>
    [Column, Nullable]
    public string? Barcode { get; set; }

    /// <summary>
    /// Идентификатор оператора, выполнившего
    /// регистрацию читателя.
    /// </summary>
    [Column]
    public int Operator { get; set; }

    /// <summary>
    /// Дата.
    /// </summary>
    [Column (Name = "whn")]
    public DateTime Moment { get; set; }

    /// <summary>
    /// Признак блокировки (например, из-за
    /// нарушения Правил пользования библиотекой.
    /// </summary>
    [Column]
    public bool Blocked { get; set; }

    /// <summary>
    /// Читатель подписал обходной лист.
    /// </summary>
    [Column (Name = "podpisal")]
    public bool Gone { get; set; }

    /// <summary>
    /// Пароль для ресурсов библиотеки.
    /// </summary>
    [Column, Nullable]
    public string? Password { get; set; }

    /// <summary>
    /// E-mail для уведомлений.
    /// </summary>
    [Column, Nullable]
    public string? Mail { get; set; }

    /// <summary>
    /// Занимаемая должность.
    /// </summary>
    [Column (Name = "job"), Nullable]
    public string? JobTitle { get; set; }

    /// <summary>
    /// Кафедра.
    /// </summary>
    [Column, Nullable]
    public string? Cathedra { get; set; }

    /// <summary>
    /// Штрих-код линейки.
    /// </summary>
    [Column, Nullable]
    public string? Proxy { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    [Column, Nullable]
    public string? Phone { get; set; }

    /// <summary>
    /// Идентификатор MIRA.
    /// </summary>
    [Column]
    public int IstuID { get; set; }

    /// <summary>
    /// Фото.
    /// </summary>
    [Column]
    public byte[]? Photo { get; set; }

    /// <summary>
    /// В академическом отпуске?
    /// </summary>
    [Column]
    public bool Academ { get; set; }

    /// <summary>
    /// Не проверять ФИО.
    /// </summary>
    [Column]
    public bool DontVerify { get; set; }

    /// <summary>
    /// Место работы (для посторонних читателей).
    /// </summary>
    [Column, Nullable]
    public string? Workplace { get; set; }

    /// <summary>
    /// Задолжник-вечиник.
    /// </summary>
    [Column]
    public bool Everlasting { get; set; }

    /// <summary>
    /// Комментарий в произвольной форме.
    /// </summary>
    [Column, Nullable]
    public string? Comment { get; set; }

    /// <summary>
    /// Предупреждение, например, о наличии штрафа.
    /// </summary>
    [Column, Nullable]
    public string? Alert { get; set; }

    /// <summary>
    /// День рождения.
    /// </summary>
    [Column, Nullable]
    public string? Birthdate { get; set; }

    /// <summary>
    /// Члены семьи.
    /// </summary>
    [Column, Nullable]
    public string? Family { get; set; }

    /// <summary>
    /// Запрет редактирования сведений о читателе.
    /// </summary>
    [Column]
    public bool Immutable { get; set; }

    /// <summary>
    /// Предупреждать о посещении.
    /// </summary>
    [Column]
    public bool Notify { get; set; }

    /// <summary>
    /// RFID-метка читательского билета.
    /// </summary>
    [Column, Nullable]
    public string? Rfid { get; set; }

    /// <summary>
    /// Журналы на руках.
    /// </summary>
    [Column, Nullable]
    public string? Magazines { get; set; }

    /// <summary>
    /// Внесенные средства.
    /// </summary>
    [Column]
    public decimal Debet { get; set; }

    /// <summary>
    /// Согласие на всё.
    /// </summary>
    [Column, Nullable]
    public string? Agree { get; set; }

    /// <summary>
    /// Сертификат о прививке против COVID-19.
    /// Студентам, имеющим сертификат,
    /// прощаются долги перед библиотекой.
    /// </summary>
    /// <remarks>
    /// Безграмотное название колонки в базе
    /// придумано командой Копайгородского.
    /// </remarks>
    [Column ("sertif"), Nullable]
    public string? Certificate { get; set; }

    /// <summary>
    /// Идентификатор пользователя в Telegram.
    /// Используется ботом.
    /// </summary>
    [Column ("telega"), Nullable]
    public long TelegramId { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Ticket}: {Name}";

    #endregion
}
