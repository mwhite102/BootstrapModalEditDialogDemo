using BootstrapModalEditDialogDemo.Data;
using BootstrapModalEditDialogDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootstrapModalEditDialogDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(ToDoDataService.GetToDoList());
        }

        [HttpGet]
        public ActionResult AddToDo()
        {
            ToDoModel model = new ToDoModel();
            return PartialView("_ToDoEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddToDo(ToDoModel model)
        {
            if (TryValidateModel(model))
            {
                // Do Save 
                try
                {
                    if (ToDoDataService.InsertToDoItem(model))
                    {
                        // Reload the ToDoItem
                        return PartialView("_ToDoDataRow", model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Failed Validation
            Response.StatusCode = 400;
            return PartialView("_ToDoEdit", model);
        }


        [HttpGet]
        public ActionResult EditToDo(int id)
        {
            ToDoModel model = ToDoDataService.GetToDoById(id);
            return PartialView("_ToDoEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditToDo(ToDoModel model)
        {
            if (TryValidateModel(model))
            {
                // Do Save 
                try
                {
                    if (ToDoDataService.UpdateToDoItem(model))
                    {
                        // Reload the ToDoItem
                        return PartialView("_ToDoDataRow", model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Failed Validation
            Response.StatusCode = 400;
            return PartialView("_ToDoEdit", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ToDoDataService.DeleteToDoItem(id))
            {
                return new EmptyResult();
            }
            else
            {
                Response.StatusCode = 400;
                return new EmptyResult();
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}