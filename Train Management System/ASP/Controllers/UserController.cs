using ASP.Models;
using Data.User;
using Logic.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// attempts to log in user with given email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult Login(string email, string password)
        {
            try
            {
                UserRegistration container = new UserRegistration(new UserDAL());
                List<string> errors = new List<string>();
                UserModel user = container.Login(email, password, out errors);

                if (errors.Count != 0)
                {
                    return Json(new
                    {
                        errors = errors
                    });
                }

                //claims n cookies
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return SignIn(new ClaimsPrincipal(claimsIdentity), CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// signs up user, but not yet registered
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult SignUp(string email)
        {
            List<string> errors = new List<string>();
            try
            {
                UserRegistration container = new UserRegistration(new UserDAL());
                container.SignUp(email, out errors);
                if (errors.Count == 0)
                {
                    return Json(Ok());
                } else
                {
                    throw new Exception();
                }
            }
            catch
            {
                return Json(new { errors = errors });
            }
        }

        /// <summary>
        /// actually register the user with given details
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, ActionName("ConfirmRegister")]
        public IActionResult Register(UserViewModel userViewModel)
        {
            try
            {
                UserRegistration container = new UserRegistration(new UserDAL());
                List<string> errors = new List<string>();


                container.Register(new UserModel(userViewModel.Email, userViewModel.FirstName, userViewModel.LastName, userViewModel.Role, userViewModel.EmployeePosition, userViewModel.Password), out errors);
                if (errors.Count == 0)
                {
                    return Json(Ok());
                } else
                {
                    return Json(new
                    {
                        errors = errors
                    });
                }
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [AllowAnonymous]
        public IActionResult Register(string email, string key)
        {
            UserRegistration container = new UserRegistration(new UserDAL());
            List<string> errors = new List<string>();
            UserModel userModel = container.Login(email, key, out errors);

            if (errors.Count != 0)
            {
                return Conflict("credentials not found");
            }

            UserViewModel userViewModel = new UserViewModel() { Email = email, FirstName = userModel.FirstName, LastName = userModel.LastName };

            return View(userViewModel);
        }

        /// <summary>
        /// logs current user out
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            List<string> errors = new List<string>();
            try
            {
                UserContainer container = new UserContainer(new UserDAL());
                UserModel userModel = new UserModel(userViewModel.Email, userViewModel.FirstName, userViewModel.LastName, userViewModel.Role, userViewModel.EmployeePosition, null);
                container.CreateUser(userModel, out errors);

                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = errors});
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            UserContainer userC = new UserContainer(new UserDAL());
            List<UserModel> userModelList = userC.GetAllUsers();
            if (userModelList == null)
                return View(null);
            List<UserViewModel> userViewModelList = new List<UserViewModel>();
            foreach (UserModel userModel in userModelList)
                userViewModelList.Add(new UserViewModel() { UserId = userModel.UserId, Email = userModel.Email, EmployeePosition = userModel.EmployeePosition, FirstName = userModel.FirstName, LastName = userModel.LastName, Role = userModel.Role });

            return View(userViewModelList);
        }

        public IActionResult Edit(int id)
        {
            UserContainer userC = new UserContainer(new UserDAL());
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (role == "Employee" || role == "Passenger")
                id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            UserModel userModel = userC.FindUserById(id);
            UserViewModel userViewModel = new UserViewModel() { UserId = userModel.UserId, Email = userModel.Email, EmployeePosition = userModel.EmployeePosition, FirstName = userModel.FirstName, LastName = userModel.LastName, Role = userModel.Role, JourneyId = userModel.JourneyId };

            if (role == "Administrator")
                return PartialView("_Edit", userViewModel);
            else
                return View("_Edit", userViewModel);
        }

        [HttpPost, ActionName("ConfirmEdit")]
        public IActionResult ConfirmEdit(UserViewModel viewModel)
        {
            try
            {
                UserContainer userC = new UserContainer(new UserDAL());
                UserModel userModel = userC.FindUserById(viewModel.UserId);

                if (viewModel.FirstName != null)
                    userModel.FirstName = viewModel.FirstName;
                if (viewModel.LastName != null)
                    userModel.LastName = viewModel.LastName;
                if (viewModel.Email != null)
                    userModel.Email = viewModel.Email;
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                if (role == "Administrator")
                    userModel.Role = viewModel.Role;
                if (viewModel.EmployeePosition != null)
                    userModel.EmployeePosition = viewModel.EmployeePosition;
                if (viewModel.JourneyId != null)
                    userModel.JourneyId = viewModel.JourneyId;

                userModel.UpdateUser(new UserDAL());

                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            return PartialView("_Delete", id);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult ConfirmDelete(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) == id)
            {
                return Json(new { errors = new List<string>() { { "Can't delete urself lol" } } });
            }

            try
            {
                UserContainer userC = new UserContainer(new UserDAL());
                userC.DeleteUser(id);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }
    }
}