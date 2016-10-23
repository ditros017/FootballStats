using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Web.Models.Tournament;

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
            return new EmptyResult();
        }
    }
}