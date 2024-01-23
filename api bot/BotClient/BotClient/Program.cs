using BackendApi.Contracts.Role;
using BackendApi.Contracts.User;
using Newtonsoft.Json;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.WebRequestMethods;

namespace BotClient
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello There");

            var botClient = new TelegramBotClient("6681901109:AAGwaUz9i_BdPRhU8gQcqR6aF74el4LPkgg");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );


            var me = await botClient.GetMeAsync();

            /*HttpClient client = new HttpClient();
            var result = await client.GetAsync("https://localhost:7098/api/Users");
            Console.WriteLine(result);

            var test = await result.Content.ReadAsStringAsync();
            Console.WriteLine(test);

            GetUserResponse[] users = JsonConvert.DeserializeObject<GetUserResponse[]>(test);

            foreach (var u in users)
            {
                Console.WriteLine(u.UserId + " " + u.Username);
            }*/


            Console.WriteLine($"{me} is working");

            Console.ReadLine();

            cts.Cancel();
        }
        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;

        }


        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {

                var message = update.Message;
                var messageText = message.Text;

                var chatId = message.Chat.Id;

                Console.WriteLine($"Recieved a '{messageText}' message in chat {chatId}.");





                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "You said: \n" + messageText,
                    cancellationToken: cancellationToken);

                if (message.Text == "/check")
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Проверка: ОК!",
                    cancellationToken: cancellationToken);
                }



                if (message.Text == "/hi")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Здраствуй, " + message.From.Username,
                        cancellationToken: cancellationToken);
                }



                if (message.Text == "/pic")
                {
                    await botClient.SendPhotoAsync(
                        chatId: chatId,
                        photo: InputFile.FromUri(new Uri("https://i.pinimg.com/564x/c1/d6/cb/c1d6cbe6d9b85de125ca643036f9d5a7.jpg")),
                        caption: "Настроение:",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                }

                if (message.Text == "/video")
                {
                    await botClient.SendVideoAsync(
                    chatId: chatId,
                    video: InputFile.FromUri("https://rr2---sn-hpa7kn76.googlevideo.com/videoplayback?expire=1706021527&ei=N36vZaeVB9jei9oP7MKvmA4&ip=156.146.41.199&id=o-AJ2YmHvwqUsRxLWJ2ogP-eQGe6NFHsFv7meod_1wIBDe&itag=18&source=youtube&requiressl=yes&xpc=EgVo2aDSNQ%3D%3D&mh=Zk&mm=31%2C26&mn=sn-hpa7kn76%2Csn-hgn7rne7&ms=au%2Conr&mv=m&mvi=2&pl=24&initcwndbps=1370000&vprv=1&svpuc=1&mime=video%2Fmp4&cnr=14&ratebypass=yes&dur=9.334&lmt=1658240670021200&mt=1705994214&fvip=5&fexp=24007246&c=ANDROID&txp=5318224&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cxpc%2Cvprv%2Csvpuc%2Cmime%2Ccnr%2Cratebypass%2Cdur%2Clmt&sig=AJfQdSswRAIgbv5-htb4PAEDii1AyEzZhzSwrg6w7i_twvk496fzjZoCIATnE5o4wsaDO3euTq19kvI3JTsUPduOeC3eZBrHLjI8&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps&lsig=AAO5W4owRAIgPAlsb62Z64T0TvKgLCCzeLyFDpeUNbhniOV9xuJ0qmgCIAyl1Q2x-7aDnpYK7pqFEiypjioa35XBJ-ndw3XvGJT_&title=Кот%20орет%20мем%20оригинал"),
                    cancellationToken: cancellationToken);
                }



                if (message.Text == "/sticker")
                {
                    await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: InputFile.FromString("CAACAgIAAxkBAAELPKllr5OefbeQLad1nPzTT2W__DPH3AACVBcAAoxM4EomtOHHUF9KiDQE"),
                    cancellationToken: cancellationToken);
                }

                if (message.Text == "/but")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        InlineKeyboardButton.WithUrl(
                            text: "Ссылка на Киноманию",
                            url: "https://github.com/Hukkatir/api-doc")
                    });

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Ссылка на супер проект",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                }



                if (message.Text == "/database")
                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[] { new KeyboardButton("Пользователи") },
                        new[] { new KeyboardButton("Роли") }
                    })
                    {
                        ResizeKeyboard = true
                    };

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Выберите действие:",
                        replyMarkup: keyboard,
                        cancellationToken: cancellationToken);
                }
                else if (message.Text == "Пользователи")
                {
                    await GetUsersAsync(botClient, chatId, cancellationToken);
                }
                else if (message.Text == "Роли")
                {
                    await GetRolesAsync(botClient, chatId, cancellationToken);
                }



                static async Task GetRolesAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
                {
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync("https://localhost:7098/api/Role", cancellationToken);
                    var content = await response.Content.ReadAsStringAsync();
                    var roles = JsonConvert.DeserializeObject<GetRoleResponse[]>(content);

                    var messageText = "";
                    foreach (var role in roles)
                    {
                        messageText += $"\n\nДанные роли:\nRoleId: {role.RoleId}\nRoleName: {role.RoleName}\nDescription: {role.Descrip}";
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: messageText,
                        cancellationToken: cancellationToken);
                }


                static async Task GetUsersAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
                {
                    HttpClient client = new HttpClient();

                    var response = await client.GetAsync("https://localhost:7098/api/Users", cancellationToken);
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<GetUserResponse[]>(content);

                    var messageText = "";
                    foreach (var user in users)
                    {
                        messageText += $"\n\nДанные пользователя:\nUserId: {user.UserId}\nRoleId: {user.RoleId}\nUsername: {user.Username}\nEmail: {user.Email}";
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: messageText,
                        cancellationToken: cancellationToken);
                }

            }
        }
    }
}


