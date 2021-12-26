// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BotCommand.cs -- абстрактная команда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

#endregion

namespace TeleIrbis.Commands;

/// <summary>
/// Абстрактная команда.
/// </summary>
public abstract class BotCommand
{
    public abstract string Name { get; }

    public virtual string[] Aliases => Array.Empty<string>();

    public abstract void Execute (Message message, TelegramBotClient client);

    /// <summary>
    /// Экранная клавиатура с командами.
    /// </summary>
    public virtual ReplyKeyboardMarkup GetKeyboard()
    {
        var buttons = new List<List<KeyboardButton>>()
        {
            new()
            {
                new KeyboardButton() { Text = "Анонсы" },
                new KeyboardButton() { Text = "Контакты" }
            },
            new()
            {
                new KeyboardButton() { Text = "Режим работы" },
                new KeyboardButton() { Text = "Помощь" }
            }
        };
        ReplyKeyboardMarkup result
            = new ReplyKeyboardMarkup (buttons, true);

        return result;
    }

    public virtual void SendMessage
        (
            TelegramBotClient client,
            ChatId chatId,
            string text
        )
    {
        client.SendTextMessageAsync
                (
                    chatId,
                    text,
                    ParseMode.Html,
                    replyMarkup: GetKeyboard()
                )
            .Wait();
    }

    public virtual bool Contains
        (
            string command
        )
    {
        return command.StartsWith ("/" + Name) || Aliases.Contains (command);
    }
}
