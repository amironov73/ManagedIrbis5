// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforAtA.cs -- отправка сообщения по e-mail
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using MailKit.Net.Smtp;

using MimeKit;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // 2021.1
    //
    // Создан новый форматный выход (необходимо сразу отметить - весьма специфический!)
    // ОТПРАВКА СООБЩЕНИЯ ПО E-MAIL
    // &uf('@A<e-mial>,<V1>,<V2>,..<Vn>..')
    // где:
    // A - односимвольный суффикс, определяющий параметры INI-файла для форматов ТЕМЫ и ТЕЛА письма
    //     <e-mail> - E-mail получателя
    //     <Vn> - значения модельных полей, используемых при формировании ТЕМЫ и ТЕЛА письма
    // Форматный выход возвращает:
    // - пустоту - если сообщение отправлено
    // - строку текста - если при отправке произошла ошибка
    // Форматный выход использует параметры в секции [MAIN] INI-файла:
    // - mailsubjectA= - формат (явный или имя формата с предшествующим символом @),
    //   используемый для формирования ТЕМЫ письма (в формате могут использоваться
    //   модельные поля V1, v2, ....)
    // - mailbodyA= - формат (явный или имя формата с предшествующим символом @), используемый
    //   для формирования ТЕЛА письма (в формате могут использоваться модельные поля V1, v2, ....)
    // Кроме того, используются прежние параметры для рассылки E-mail:
    // MailHost=
    // MailPort=
    // MailFrom=
    // MailFromAdress=
    // MailUser=
    // MailPassword=
    // MailSSL=
    //

    /// <summary>
    /// Отправка сообщения по e-mail.
    /// </summary>
    static class UniforAt
    {
        #region Public methods

        public static void SendEmail
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            expression ??= string.Empty;

            var navigator = new ValueTextNavigator (expression);
            var suffix = navigator.ReadChar();
            var toEmail = navigator.ReadUntil (',').ToString();
            if (suffix == ValueTextNavigator.EOF || string.IsNullOrEmpty (toEmail))
            {
                return;
            }

            var connection = context.Provider as SyncConnection;
            if (connection is null)
            {
                return;
            }

            var ini = connection.IniFile;
            if (ini is null)
            {
                return;
            }

            var main = ini.GetSection ("Main");
            if (main is null)
            {
                return;
            }

            var mailSubject = main["mailsubject" + suffix];
            var mailBody = main["mailbody" + suffix];
            var mailHost = main["MailHost"];
            var mailPort = main.GetValue ("MailPort", 25);
            var mailFrom = main["MailFrom"];
            var mailFromAdress = main["MailFromAdress"];
            var mailUser = main["MailUser"];
            var mailPassword = main["MailPassword"];
            var mailSsl = main.GetValue ("MailSSL", false);

            if (string.IsNullOrEmpty (mailSubject)
                || string.IsNullOrEmpty (mailBody)
                || string.IsNullOrEmpty (mailHost)
                || string.IsNullOrEmpty (mailFrom)
                || string.IsNullOrEmpty (mailFromAdress)
                || string.IsNullOrEmpty (mailUser)
                || string.IsNullOrEmpty (mailPassword))
            {
                return;
            }

            // TODO собственно форматирование тела сообщения

            var message = new MimeMessage();
            message.From.Add (new MailboxAddress (mailFrom, mailFromAdress));
            message.To.Add (new MailboxAddress (toEmail, toEmail));
            message.Subject = mailSubject;
            message.Body = new TextPart (mailBody);

            try
            {
                using var mailClient = new SmtpClient();
                mailClient.Connect (mailHost, mailPort, mailSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                mailClient.AuthenticationMechanisms.Remove ("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                mailClient.Authenticate (mailUser, mailPassword);

                mailClient.Send (message);
                mailClient.Disconnect (true);
            }
            catch (Exception exception)
            {
                context.WriteAndSetFlag (node, exception.Message);
            }
        }

        #endregion
    }
}
