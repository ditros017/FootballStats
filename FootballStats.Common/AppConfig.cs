using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballStats.Common.Extensions;

namespace FootballStats.Common
{
    public class AppConfig
    {
        public static bool IsDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public static DbInitializerMode DbInitializer = ConfigurationManager.AppSettings["DbInitializer"].AsEnum<DbInitializerMode>(safe: true);

        public enum DbInitializerMode
        {
            DontNotifyIfModelChanges,
            DropCreateDbIfModelChanges
        }
    }
}