// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* KnownDistributions.cs -- известные дистрибутивы Linux
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Известные дистрибутивы Linux.
/// </summary>
[PublicAPI]
public static class KnownDistributions
{
    #region Constants

    /// <summary>
    /// Debian и дистрибутивы, основанные на нём,
    /// используют формат пакетов .deb и менеджер пакетов dpkg.
    /// </summary>
    public const string Debian = "debian";

    /// <summary>
    /// Самая первая Live CD (затем DVD) версия Debian.
    /// </summary>
    public const string Knoppix = "knoppix";

    /// <summary>
    /// Raspberry Pi OS.
    /// </summary>
    public const string Raspberry = "raspberry";

    /// <summary>
    /// Игровой дистрибутив, разработанный Valve Corporation,
    /// содержащий в себе клиент Steam и проприетарные драйверы.
    /// </summary>
    public const string SteamOS = "steamos";

    /// <summary>
    /// Легковесный дистрибутив, ориентированный изначально
    /// на устаревшее оборудование с ограниченными характеристиками
    /// и архитектурой процессора x86.
    /// </summary>
    public const string Antix = "antix";

    /// <summary>
    /// Операционная система повышенной безопасности,
    /// разработанная для нужд российской армии.
    /// </summary>
    public const string Astra = "astra";

    /// <summary>
    /// Дистрибутив, основанный на Debian, целью которого
    /// является предоставление пользователю максимально
    /// удобного и красивого интерфейса.
    /// </summary>
    public const string Deepin = "deepin";

    /// <summary>
    /// Форк Debian, разработанный в 2014 в целях дать пользователю
    /// выбор в системах инициализации, разделяя пакеты из systemd.
    /// </summary>
    public const string Devuan = "devuan";

    /// <summary>
    /// Распространение дистрибутива спонсируется компанией Canonical Ltd.
    /// и основавшим её южноафриканским предпринимателем Марком Шаттлвортом.
    /// Основной целью является получение полноценного и качественного
    /// рабочего стола.
    /// </summary>
    public const string Ubuntu = "ubuntu";

    /// <summary>
    /// Дистрибутив для сетевого аудита и проверки на уязвимости.
    /// </summary>
    public const string Kali = "kali";

    /// <summary>
    /// Red Hat Linux — дистрибутив Linux компании Red Hat.
    /// Выпускался в период с 1995 по 2003 год включительно.
    /// </summary>
    public const string RedHat = "redhat";

    /// <summary>
    /// Поддерживаемый сообществом дистрибутив Linux, спонсируемый SUSE.
    /// Проводит политику, по которой весь код в стандартных установках
    /// должен быть свободным и открытым, включая модули ядра Linux.
    /// Продукты SUSE Enterprise основаны на кодовой базе openSUSE.
    /// </summary>
    public const string OpenSuse = "opensuse";

    /// <summary>
    /// Поддерживаемый сообществом дистрибутив Linux, являющийся
    /// открытой версией RHEL, хорошо адаптированной под серверы.
    /// </summary>
    public const string CentOS = "centos";

    /// <summary>
    /// Поддерживаемый сообществом дистрибутив Linux, спонсируемый Red Hat.
    /// Репозитории содержат новейшие пакеты.
    /// </summary>
    public const string Fedora = "fedora";

    /// <summary>
    /// Первый релиз был основан на Red Hat Linux 5.1 и KDE 1 в июле 1998.
    /// С того времени он стал независимым дистрибутивом. Название было
    /// изменено на Mandriva, в ней содержались инструменты, обеспечивающие
    /// лёгкую конфигурацию системы.
    /// </summary>
    public const string Mandrake = "mandrake";

    /// <summary>
    /// Поддерживается Oracle. Призван быть полностью совместимым
    /// с Red Hat Enterprise Linux.
    /// </summary>
    public const string Oracle = "oracle";

    /// <summary>
    /// Операционная система РЕД ОС разрабатывается российской компанией
    /// ООО "РЕД СОФТ".
    /// </summary>
    public const string RedOS = "redos";

    /// <summary>
    /// Дистрибутив, удобный в использовании в основном благодаря своему
    /// менеджеру настроек. В настоящее время разработка прекращена,
    /// разработчики ушли в другие проекты.
    /// </summary>
    public const string Mandriva = "mandriva";

    /// <summary>
    /// Российский дистрибутив Linux, доступный в трёх изданиях:
    /// ROSA Desktop Fresh, ROSA Enterprise Desktop и ROSA Enterprise
    /// Linux Server. Два последних созданы для использования
    /// в коммерческих целях. Версия для персональных компьютеров
    /// содержит проприетарное программное обеспечение.
    /// </summary>
    public const string Rosa = "rosa";

    /// <summary>
    /// ALT Linux - набор российских операционных систем, основанных на RPM,
    /// построенных поверх ядра Linux и репозитория Sisyphus. ALT Linux
    /// разрабатывается группой разработчиков сообщества ALT Linux Team
    /// и компанией "Базальт СПО".
    /// </summary>
    public const string Alt = "alt";

    /// <summary>
    /// Дистрибутив, созданный для опытных пользователей. Использует систему
    /// rolling release и использует утилиту pacman для управления пакетами.
    /// Придерживается философии KISS.
    /// </summary>
    public const string Arch = "arch";

    /// <summary>
    /// Дистрибутив, в основе которого оптимизированное и часто обновляемое ПО,
    /// а также возможность полной конфигурации системы вплоть до параметров
    /// компиляции отдельных пакетов. Дистрибутивы, основанные на Gentoo, имеют
    /// систему управления пакетами Portage с emerge или одним из альтернативных
    /// пакетных менеджеров.
    /// </summary>
    public const string Gentoo = "gentoo";

    /// <summary>
    /// Chrome OS используется на множестве устройств, таких как Chromebook,
    /// Chromebox и планшетные ПК. Полностью зависит от Интернета,
    /// запуск приложений осуществляется из браузера Chrome.
    /// Операционная система имеет интерфейс, идентичный Chrome,
    /// вместо традиционных окружений рабочего стола.
    /// </summary>
    public const string ChromiumOS = "chromiumos";

    /// <summary>
    /// Chrome OS используется на множестве устройств, таких как Chromebook,
    /// Chromebox и планшетные ПК. Полностью зависит от Интернета,
    /// запуск приложений осуществляется из браузера Chrome.
    /// Операционная система имеет интерфейс, идентичный Chrome,
    /// вместо традиционных окружений рабочего стола.
    /// </summary>
    public const string ChromeOS = "chromeos";

    /// <summary>
    /// Ориентированный на безопасность и легковесный дистрибутив Linux,
    /// основанный на musl и BusyBox.
    /// </summary>
    public const string Alpine = "alpine";

    /// <summary>
    /// Мобильная операционная система, основанная на ядре Linux разрабатываемая
    /// корпорацией Google. Предназначена для сенсорных устройств,
    /// таких как смартфоны и планшеты.
    /// </summary>
    public const string Android = "android";

    /// <summary>
    /// Дистрибутив Linux, разрабатывающийся для использования в домашних
    /// маршрутизаторах и других встраиваемых системах.
    /// </summary>
    public const string OpenWrt = "openwrt";

    /// <summary>
    /// Минимальный дистрибутив Linux, который можно запустить на системах
    /// с очень низкими параметрами — даже при наличии менее 32 МБ RAM.
    /// </summary>
    public const string Puppy = "puppy";

    /// <summary>
    /// Минимальная (около 10 МБ) операционная система с BusyBox, FLTK
    /// и прочим минималистическим программным обеспечением.
    /// </summary>
    public const string TinyCore = "tinycore";

    #endregion
}
