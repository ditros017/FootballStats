﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FootballStats.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TournamentList",
                url: "Tournaments",
                defaults: new {controller = "Tournaments", action = "List"}
            );

            routes.MapRoute(
                name: "TournamentDetails",
                url: "Tournaments/{id}",
                defaults: new {controller = "Tournaments", action = "Details"}
            );

            routes.MapRoute(
                name: "FootballMatchList",
                url: "Tournaments/{tournamentId}/FootballMatches",
                defaults: new { controller = "FootballMatches", action = "List" }
            );

            routes.MapRoute(
                name: "FootballMatchDetails",
                url: "Tournaments/{tournamentId}/FootballMatches/{id}",
                defaults: new { controller = "FootballMatches", action = "Details" }
            );

            routes.MapRoute(
                name: "CoachList",
                url: "Coaches",
                defaults: new { controller = "Coaches", action = "List" }
            );

            routes.MapRoute(
                name: "CoachDetails",
                url: "Coaches/{id}",
                defaults: new { controller = "Coaches", action = "Details" }
            );

            routes.MapRoute(
                name: "PlayerList",
                url: "Players",
                defaults: new { controller = "Players", action = "List" }
            );

            routes.MapRoute(
                name: "PlayerDetails",
                url: "Players/{id}",
                defaults: new { controller = "Players", action = "Details" }
            );

            routes.MapRoute(
                name: "TeamList",
                url: "Teams",
                defaults: new { controller = "Teams", action = "List" }
            );

            routes.MapRoute(
                name: "TeamDetails",
                url: "Teams/{id}",
                defaults: new { controller = "Teams", action = "Details" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}