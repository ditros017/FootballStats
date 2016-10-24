using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
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
            var model = _unitOfWork.RefereeRepository.GetById(id, r => new DetailsModel
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                MiddleName = r.MiddleName,
                DateOfBirth = r.DateOfBirth,
                FootballMatchCount = r.FootballMatchReferees.Count
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

        [HttpGet]
        public ActionResult Create()
        {
            return View("~/Views/Referees/Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.RefereeRepository.Save(new Referee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    DateOfBirth = model.DateOfBirth,
                    Type = model.Type
                });
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _unitOfWork.RefereeRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(new { redirectToUrl = Url.Action("List") });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _unitOfWork.RefereeRepository.GetById(id, r => new EditModel
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                MiddleName = r.MiddleName,
                DateOfBirth = r.DateOfBirth
            });

            return View("~/Views/Referees/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var referee = _unitOfWork.RefereeRepository.GetById(model.Id);
                referee.FirstName = model.FirstName;
                referee.LastName = model.LastName;
                referee.MiddleName = model.MiddleName;
                referee.DateOfBirth = model.DateOfBirth;
                referee.Type = model.Type;

                _unitOfWork.RefereeRepository.Save(referee);
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }
    }
}