using System.ComponentModel.DataAnnotations;

namespace BootstrapModalEditDialogDemo.Models
{
    public class ToDoModel
    {
        public int ToDoId { get; set; }

        [Required(ErrorMessage ="Please enter a description")]
        public string Description { get; set; }

        public bool Completed { get; set; }
    }
}