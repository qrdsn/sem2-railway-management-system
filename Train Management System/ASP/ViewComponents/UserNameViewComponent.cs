using ASP.Models;
using Data.User;
using Logic.User;
using Microsoft.AspNetCore.Mvc;

namespace ASP.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id)
        {
            UserContainer userC = new UserContainer(new UserDAL());
            UserModel userModel = userC.FindUserById(id);
            UserViewModel userViewModel = new UserViewModel() { UserId = userModel.UserId, Email = userModel.Email, EmployeePosition = userModel.EmployeePosition, FirstName = userModel.FirstName, LastName = userModel.LastName, Role = userModel.Role };

            return View("~/Views/User/_Name.cshtml", userViewModel);
        }
    }
}
