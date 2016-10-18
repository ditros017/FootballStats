using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Common;
using FootballStats.Data.Infrastructure;
using FootballStats.Web;

[assembly: PreApplicationStartMethod(typeof(AppStart), nameof(AppStart.Start))]

namespace FootballStats.Web
{
    public class AppStart
    {
        public static void Start()
        {
            new AppStart().Run();
        }

        public void Run()
        {
            SetDbInitializer();
        }

        private void SetDbInitializer()
        {
            var mode = AppConfig.DbInitializer;

            switch (mode)
            {
                case AppConfig.DbInitializerMode.DontNotifyIfModelChanges:
                    DbInitializerManager.DontTrackChanges();
                    break;
                case AppConfig.DbInitializerMode.DropCreateDbIfModelChanges:
                    DbInitializerManager.DropCreateDbIfModelChanges();
                    break;
                default:
                    throw new NotSupportedException($"Db initializer mode '{mode}' is not supported.");
            }
        }
    }
}