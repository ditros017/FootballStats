using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.Tournament;

namespace FootballStats.Web.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult List()
        {
            return View("~/Views/Tournaments/List.cshtml", new ListModel
            {
                Tournaments = _unitOfWork.TournamentRepository.GetAll(t => new ListModel.Tournament
                {
                    Id = t.Id,
                    Name = t.Name,
                    FootballMatchCount = t.FootballMatches.Count
                }).ToArray()
            });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _unitOfWork.TournamentRepository.GetById(id, t => new DetailsModel
            {
                Id = t.Id,
                Name = t.Name,
                FootballMatches = t.FootballMatches.Select(m => new DetailsModel.FootballMatch
                {
                    Id = m.Id,
                    StageType = m.StageType,
                    Teams = m.FootballMatchTeams.Select(mt => new DetailsModel.FootballMatch.Team
                    {
                        Id = mt.TeamId,
                        Name = mt.Team.Name,
                        IsGuest = mt.IsGuest
                    })
                })
            });

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

            return View("~/Views/Tournaments/Details.cshtml", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("~/Views/Tournaments/Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TournamentRepository.Save(new Tournament
                {
                    Name = model.Name
                });
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _unitOfWork.TournamentRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(new { redirectToUrl = Url.Action("List") });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _unitOfWork.TournamentRepository.GetById(id, t => new EditModel
            {
                Id = t.Id,
                Name = t.Name
            });

            return View("~/Views/Tournaments/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var tournament = _unitOfWork.TournamentRepository.GetById(model.Id);
                tournament.Name = model.Name;

                _unitOfWork.TournamentRepository.Save(tournament);
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }
    }
}