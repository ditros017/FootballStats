using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
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
    }
}