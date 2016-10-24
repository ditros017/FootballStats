using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.Player;

namespace FootballStats.Web.Controllers
{
    public class PlayersController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult List()
        {
            return View("~/Views/Players/List.cshtml", new ListModel
            {
                Players = _unitOfWork.PlayerRepository.GetAll(p => new ListModel.Player
                {
                    Id = p.Id,
                    Name = p.LastName + " " + p.FirstName,
                    TeamName = p.Team.Name
                }).ToArray()
            });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _unitOfWork.PlayerRepository.GetById(id, p => new DetailsModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                TeamId = p.TeamId,
                TeamName = p.Team.Name,
                DateOfBirth = p.DateOfBirth,
                FootballMatchCount = p.FootballMatchPlayers.Count(mp => mp.IsStarted || mp.EnterTime.HasValue)
            });

            model.FootballMatchGoalCount = _unitOfWork.FootballMatchPlayerGoalRepository.Count(g => g.FootballMatchPlayer.PlayerId == model.Id);

            return View("~/Views/Players/Details.cshtml", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("~/Views/Players/Create.cshtml", new CreateModel
            {
                Teams = _unitOfWork.TeamRepository.GetAll(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString()
                }).ToArray()
            });
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PlayerRepository.Save(new Player
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    DateOfBirth = model.DateOfBirth,
                    TeamId = model.TeamId
                });
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View(new CreateModel
            {
                Teams = _unitOfWork.TeamRepository.GetAll(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString()
                }).ToArray()
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _unitOfWork.PlayerRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(new { redirectToUrl = Url.Action("List") });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _unitOfWork.PlayerRepository.GetById(id, p => new EditModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                DateOfBirth = p.DateOfBirth,
                TeamId = p.TeamId
            });

            model.Teams = _unitOfWork.TeamRepository.GetAll(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString(),
                Selected = t.Id == model.TeamId
            }).ToArray();

            return View("~/Views/Players/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var player = _unitOfWork.PlayerRepository.GetById(model.Id);
                player.FirstName = model.FirstName;
                player.LastName = model.LastName;
                player.MiddleName = model.MiddleName;
                player.DateOfBirth = model.DateOfBirth;
                player.TeamId = model.TeamId;

                _unitOfWork.PlayerRepository.Save(player);
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            model.Teams = _unitOfWork.TeamRepository.GetAll(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToArray();

            return View(model);
        }
    }
}