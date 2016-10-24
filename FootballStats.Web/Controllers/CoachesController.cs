using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.Coach;

namespace FootballStats.Web.Controllers
{
    public class CoachesController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult List()
        {
            return View("~/Views/Coaches/List.cshtml", new ListModel
            {
                Coaches = _unitOfWork.CoachRepository.GetAll(c => new ListModel.Coach
                {
                    Id = c.Id,
                    Name = c.LastName + " " + c.FirstName
                }).ToArray()
            });
        }

        [HttpGet]
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

        [HttpGet]
        public ActionResult Create()
        {
            return View("~/Views/Coaches/Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoachRepository.Save(new Coach
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    DateOfBirth = model.DateOfBirth
                });
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _unitOfWork.CoachRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(new { redirectToUrl = Url.Action("List") });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _unitOfWork.CoachRepository.GetById(id, c => new EditModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MiddleName = c.MiddleName,
                DateOfBirth = c.DateOfBirth
            });

            return View("~/Views/Coaches/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var coach = _unitOfWork.CoachRepository.GetById(model.Id);
                coach.FirstName = model.FirstName;
                coach.LastName = model.LastName;
                coach.MiddleName = model.MiddleName;
                coach.DateOfBirth = model.DateOfBirth;

                _unitOfWork.CoachRepository.Save(coach);
                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            return View();
        }
    }
}