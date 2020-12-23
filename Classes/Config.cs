using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRelayServer.Classes
{
    class Config
    {
        public static string WebsocketServerPort = "8090";
        public static string PrependLogFile = "yes";
        public static string LogFileName = "log.txt";


        public static void LoadSavedConfigs()
        {
            IniParser ini = new IniParser("config.ini");
            WebsocketServerPort = ini.GetSetting("General", "Port", WebsocketServerPort);   // use default value set above
            PrependLogFile = ini.GetSetting("General", "PrependLogFile", PrependLogFile);   // use default value set above
            LogFileName = ini.GetSetting("General", "LogFile", LogFileName);   // use default value set above
            ini = null;

        }

        public static  void SaveConfig()
        {
            IniParser ini = new IniParser("config.ini");
            ini.AddSetting("General", "Port", WebsocketServerPort);
            ini.AddSetting("General", "PrependLogFile", PrependLogFile);
            ini.AddSetting("General", "LogFile", LogFileName);
            ini.SaveSettings();
            ini = null;
        }
    }
}
