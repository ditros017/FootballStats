using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.Account;

namespace FootballStats.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetMany(u => u.Name == model.Name && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    //Roles.AddUserToRole(user.Name, user.Role.ToString());

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
            }

            return View("~/Views/Account/Login.cshtml", model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetMany(u => u.Name == model.Name).FirstOrDefault();
                if (user == null)
                {
                    _unitOfWork.UserRepository.Save(new User {Name = model.Name, Password = model.Password, Role = UserRole.None});
                    _unitOfWork.Commit();

                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    //Roles.AddUserToRole(model.Name, UserRole.None.ToString());

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }

            return View("~/Views/Account/Register.cshtml", model);
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}