using BootstrapModalEditDialogDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootstrapModalEditDialogDemo.Data
{
    public static class ToDoDataService
    {
        /// <summary>
        /// Gets the list of ToDo Items
        /// </summary>
        /// <returns>A List of ToDoModel objects</returns>
        public static List<ToDoModel> GetToDoList()
        {
            // Return the cached value if it exists
            if (HttpContext.Current.Session["ToDoList"] != null)
            {
                return HttpContext.Current.Session["ToDoList"] as List<ToDoModel>;
            }

            List<ToDoModel> toDoList = new List<ToDoModel>();

            // Mock up some ToDo items
            toDoList.Add(new ToDoModel {ToDoId = 1, Description = "Start Bootstrap Modal Dialog Demo", Completed = true});
            toDoList.Add(new ToDoModel { ToDoId = 2, Description = "Implement Demo" });
            toDoList.Add(new ToDoModel { ToDoId = 3, Description = "Add to GitHub" });


            // Add to the session to persist for the demo
            HttpContext.Current.Session["ToDoList"] = toDoList;

            return toDoList;
        }

        /// <summary>
        /// Gets a ToDoModel object based on it's ToDoId value
        /// </summary>
        /// <param name="toDoId">The ToDoId value</param>
        /// <returns>A ToDoModel if found, null if not found</returns>
        public static ToDoModel GetToDoById(int toDoId)
        {
            List<ToDoModel> toDoList = GetToDoList();
            return toDoList.Where(o => o.ToDoId == toDoId).FirstOrDefault();
        }

        /// <summary>
        /// Inserts a new ToDoModel into the list of ToDoModels
        /// </summary>
        /// <param name="toDoModel">The ToDoModel to insert</param>
        /// <returns>True if ToDoModel is added to the list</returns>
        public static bool InsertToDoItem(ToDoModel toDoModel)
        {
            int id = 0;

            List<ToDoModel> toDoList = GetToDoList();
            
            // Get the max id from the cached list
            if (toDoList.Count > 0)
            {
                id = toDoList.Max(o => o.ToDoId);
            }

            id++;

            toDoModel.ToDoId = id;

            toDoList.Add(toDoModel);

            return true;
        }

        /// <summary>
        /// Updates a ToDoModel in the ToDoModel list
        /// </summary>
        /// <param name="toDoModel">The ToDoModel to be updated</param>
        /// <returns>True if the ToDoModel is updated, False if it fails to update</returns>
        public static bool UpdateToDoItem(ToDoModel toDoModel)
        {
            List<ToDoModel> toDoList = GetToDoList();

            // Try to find the model to update
            ToDoModel model = toDoList.Where(o => o.ToDoId == toDoModel.ToDoId).FirstOrDefault();
            if (model == null)
            {
                // Item not found in the list
                return false;
            }

            model.Description = toDoModel.Description;
            model.Completed = toDoModel.Completed;

            return true;
        }

        /// <summary>
        /// Deletes a ToDoModel from the ToDoModel list
        /// </summary>
        /// <param name="toDoId">The ToDoId of the ToDoModel to be deleted</param>
        /// <returns>True if the ToDoModel is deleted, False if it is not removed</returns>
        public static bool DeleteToDoItem(int toDoId)
        {
            List<ToDoModel> toDoList = GetToDoList();

            // Try to find the model to update
            ToDoModel model = toDoList.Where(o => o.ToDoId == toDoId).FirstOrDefault();
            if (model == null)
            {
                // Item not found in the list
                return false;
            }

            toDoList.Remove(model);

            return true;
        }
    }
}