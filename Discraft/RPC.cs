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

                Details = "Dans les étoiles",
                State = "Explore l'univers",

                //Details = "Optimisation Graphique ?!",
                //State = "Des Mondes TOUJOURS plus grand !",
                //Details = "TOUJOURS plus de blocs ?!",
                //State = "Le caillou...",
                //Details = "Chunk Generator V2",
                //State = "Async Engine",
                //Details = "Création d'un monde...",
                //State = "un monde sans vie...",
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
                    LargeImageKey = "spacecraft_icon",
                    LargeImageText = "𝙎𝙥𝙖𝙘𝙚𝙘𝙧𝙖𝙛𝙩™",
                    SmallImageKey = "discraft_icon",
                    SmallImageText = "𝕯𝖎𝖘𝖈𝖗𝖆𝖋𝖙 𝟛.𝟘",
                }

            });


        }

    }
}
