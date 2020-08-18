using Telegram.Bot.Types.ReplyMarkups;

namespace DiscountAlert.Boot
{
    public static class KeywordWarper
    {
        public static ReplyKeyboardMarkup getStartKeyboard()
        {
            var keyboard = new ReplyKeyboardMarkup();

            keyboard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]{
                    new KeyboardButton("hi"),
                    new KeyboardButton("csharpcorner logo?")
                },
                new KeyboardButton[]{
                    new KeyboardButton("know about"),
                    new KeyboardButton("list of featured")
                },
                new KeyboardButton[]{
                    new KeyboardButton("here it is"),
                }
            };

            return keyboard;
        }

    }
}
