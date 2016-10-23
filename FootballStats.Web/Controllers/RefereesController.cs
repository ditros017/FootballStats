using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Web.Models.Referee;

namespace FootballStats.Web.Controllers
{
    public class RefereesController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult List()
        {
            return View("~/Views/Referees/List.cshtml", new ListModel
            {
                Referees = _unitOfWork.RefereeRepository.GetAll(r => new ListModel.Referee
                {
                    Id = r.Id,
                    Name = r.LastName + " " + r.FirstName,
                    Type = r.Type
                }).ToArray()
            });
        }

        public ActionResult Details(int id)
        {
            var model = _unitOfWork.RefereeRepository.GetById(id, p => new DetailsModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                DateOfBirth = p.DateOfBirth,
                FootballMatchCount = p.FootballMatchReferees.Count
            });

            return View("~/Views/Referees/Details.cshtml", model);
        }

        public PartialViewResult MatchList(int id)
        {
            var model = new MatchListModel
            {
                FootballMatches = _unitOfWork.FootballMatchRepository.GetMany(m => m.FootballMatchReferees.Any(r => r.RefereeId == id), m => new MatchListModel.FootballMatch
                {
                    Id = m.Id,
                    TournamentId = m.TournamentId,
                    TournamentName = m.Tournament.Name,
                    StageType = m.StageType,
                    Teams = m.FootballMatchTeams.Select(mt => new MatchListModel.FootballMatch.Team
                    {
                        Id = mt.TeamId,
                        Name = mt.Team.Name,
                        IsGuest = mt.IsGuest
                    })
                }).ToArray()
            };

            var footballMatchIds = model.FootballMatches.Select(m => m.Id).ToArray();
            var footballMatchesGoals = _unitOfWork.FootballMatchPlayerGoalRepository.GetMany(g => footballMatchIds.Contains(g.FootballMatchPlayer.FootballMatchId), g => new
            {
                g.FootballMatchPlayer.FootballMatchId,
                g.FootballMatchPlayer.Player.TeamId
            }).GroupBy(g => g.FootballMatchId).ToArray();

            foreach (var footballMatchGoals in footballMatchesGoals)
            {
                var footballMatch = model.FootballMatches.Single(m => m.Id == footballMatchGoals.Key);

                foreach (var team in footballMatch.Teams)
                {
                    team.Score = footballMatchGoals.Count(g => g.TeamId == team.Id);
                }
            }

            return PartialView("~/Views/Referees/Partials/MatchListModel.cshtml", model);
        }
    }
}