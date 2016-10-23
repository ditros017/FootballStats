using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Web.Models.Coach;

namespace FootballStats.Web.Controllers
{
    public class CoachesController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Details(int id)
        {
            var model = _unitOfWork.CoachRepository.GetById(id, c => new DetailsModel
            {
                Id = id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MiddleName = c.MiddleName,
                DateOfBirth = c.DateOfBirth
            });

            model.CoachingTeam = _unitOfWork.TeamRepository.SingleOrDefault(t => t.CoachId == id, t => new DetailsModel.Team
            {
                Id = t.Id,
                Name = t.Name
            });

            return View("~/Views/Coaches/Details.cshtml", model);
        }
    }
}