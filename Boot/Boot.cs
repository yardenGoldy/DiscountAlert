using System;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;


namespace DiscountAlert.Boot
{
    public class Boot
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("1222193869:AAH4QQJuC2IUJ-0HRxCnBQ98URRNGreBFLI");
        private static readonly ReplyKeyboardMarkup startKeyboard = KeywordWarper.getStartKeyboard();
        public Boot()
        {
            bot.OnMessage += botMessage;
            bot.StartReceiving();
        }

        private void botMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                PrepareQuestionnaires(e);
        }

        public void PrepareQuestionnaires(MessageEventArgs e)
        {
            if (e.Message.Text.ToLower() == "hi")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "hello dude" + Environment.NewLine + "welcome to csharp corner chat bot." + Environment.NewLine + "How may i help you ?", replyMarkup: startKeyboard);
            }
            else if (e.Message.Text.ToLower().Contains("know about"))
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Yes sure..!!" + Environment.NewLine + "Mahesh Chand is the founder of C# Corner.Please go through for more detail." + Environment.NewLine + "https://www.c-sharpcorner.com/about", replyMarkup: startKeyboard);
            }
            else if (e.Message.Text.ToLower().Contains("csharpcorner logo?"))
            {
                bot.SendStickerAsync(e.Message.Chat.Id, "https://csharpcorner-mindcrackerinc.netdna-ssl.com/App_Themes/CSharp/Images/SiteLogo.png");
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Anything else?", replyMarkup: startKeyboard);
            }
            else if (e.Message.Text.ToLower().Contains("list of featured"))
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Give me your profile link ?", replyMarkup: startKeyboard);
            }
            else if (e.Message.Text.ToLower().Contains("here it is"))
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, Environment.NewLine + "https://www.c-sharpcorner.com/article/getting-started-with-ionic-framework-angular-and-net-core-3/" + Environment.NewLine + Environment.NewLine +
                    "https://www.c-sharpcorner.com/article/getting-started-with-ember-js-and-net-core-3/" + Environment.NewLine + Environment.NewLine +
                    "https://www.c-sharpcorner.com/article/getting-started-with-vue-js-and-net-core-32/", replyMarkup: startKeyboard);
            }
            else
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "try from keyboards", replyMarkup: startKeyboard);
            }
        }


        ~Boot()
        {
            bot.StopReceiving();
        }
    }
}
