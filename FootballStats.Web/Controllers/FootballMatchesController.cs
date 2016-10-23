using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.FootballMatch;

namespace FootballStats.Web.Controllers
{
    public class FootballMatchesController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult List(int tournamentId)
        {
            return null;
        }

        public ActionResult Details(int id)
        {
            var model = _unitOfWork.FootballMatchRepository.GetById(id, m => new DetailsModel
            {
                Id = m.Id,
                TournamentId = m.TournamentId,
                TournamentName = m.Tournament.Name,
                Teams = m.FootballMatchTeams.Select(mt => new DetailsModel.Team
                {
                    Id = mt.TeamId,
                    Name = mt.Team.Name,
                    IsGuest = mt.IsGuest,
                    Coach = new DetailsModel.Coach
                    {
                        Id = mt.Team.CoachId,
                        Name = mt.Team.Coach.LastName + " " + mt.Team.Coach.FirstName
                    },
                    Players = m.FootballMatchPlayers.Where(mp => mp.Player.TeamId == mt.TeamId).Select(mp => new DetailsModel.Player
                    {
                        Id = mp.PlayerId,
                        Name = mp.Player.LastName + " " + mp.Player.FirstName,
                        IsStarted = mp.IsStarted,
                        GetAwayTime = mp.GetAwayTime,
                        EnterTime = mp.EnterTime,
                        Fouls = mp.FootballMatchPlayerFouls.Select(f => new DetailsModel.Foul
                        {
                            Type = f.Type,
                            Time = f.Time
                        }),
                        Goals = mp.FootballMatchPlayerGoals.Select(g => new DetailsModel.Goal
                        {
                            Type = g.Type,
                            Time = g.Time
                        })
                    })
                }),
                Referees = m.FootballMatchReferees.Select(r => new DetailsModel.Referee
                {
                    Id = r.RefereeId,
                    Name = r.Referee.LastName + " " + r.Referee.FirstName,
                    Type = r.Referee.Type
                })
            });

            return View("~/Views/FootballMatches/Details.cshtml", model);
        }
    }
}