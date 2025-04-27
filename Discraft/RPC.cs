using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordRPC;
using DiscordRPC.Logging;

namespace Discraft
{

    public class RPC
    {

        public static DiscordRpcClient client = new DiscordRpcClient("1364935362971176990");

        public static void Connect()
        {

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };


            client.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client.SetPresence(new RichPresence()
            {
                
                Details = "Optimisation Graphique ?!",
                State = "Des Mondes TOUJOURS plus grand !",
                /*Party = new Party()
                {
                    Privacy = Party.PrivacySetting.Public,
                    ID = "64x64",
                },*/
                Timestamps = Timestamps.Now,
                Buttons = new Button[]
                {
                    new Button()
                    {
                        Label = "Github",
                        Url = "https://github.com/Discretos2022/Discraft"
                    },
                },
                Assets = new Assets()
                {
                    LargeImageKey = "discraft_icon",
                    LargeImageText = "𝕯𝖎𝖘𝖈𝖗𝖆𝖋𝖙 𝟛.𝟘",
                    //SmallImageKey = "discraft_icon"
                }

            });


        }

    }
}
