using System;

namespace Boot
{
    public class Boot
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("1222193869:AAH4QQJuC2IUJ-0HRxCnBQ98URRNGreBFLIs");

        public Boot(){
            bot.OnMessage += Csharpcornerbotmessage;  
            bot.StartReceiving();           
        }

        private void Csharpcornerbotmessage(object sender, MessageEventArgs e){

        }
         
         ~Boot() {
            bot.StopReciving();
         }
    }
}
