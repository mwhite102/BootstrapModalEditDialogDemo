# Bootstrap Modal Edit Dialog Demo
A demo using Bootstrap 3 modal dialogs in an ASP.NET MVC application

## Platform
Created with Visual Studio 2017 

ASP.NET MVC 5.2 

Bootstrap 3

## Prerequisites
This project uses AjaxOptions with an Ajax.BeginForm.  AjaxOptions do not work in an out of the box MVC project.  
To add support, add jQuery.Unobtrusive-ajax.js by installing the Microsoft.jQuery.Unobtrusive-ajax NuGet package.

Update the BundleConfig.cs file to include the script.

```C#
// Include this or form AjaxOptions will not work
bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
        "~/Scripts/jquery.unobtrusive-ajax.js"));
```

Add a reference to the bundle in the layout page.

```JavaScript
@Scripts.Render("~/bundles/jqueryajax")
```

## Putting it all together

### The data model

To get started, I've created a Plain Old CLR Object (POCO) model that I'll use to populate the list of items in the page.

```C#
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
```

I'm using DataAnnotations for validation on the Description property.

### Rendering the list of items in a table

The list of items in the view will be rendered as a table with a row for each item.  When the item is edited, the row for the item will be replaced in the table with the newly edited row.  To do that, each row &lt;tr&gt; in the table needs a unique id, and a way to return just that row from the controller method that performs the update.  It's easy to do that using PartialViews.

```HTML
table class="table table-condensed">
    <thead>
        <tr>
            <th>Id</th>
            <th>Description</th>
            <th class="text-center">Completed</th>
            <th></th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <!--
            Add a table row for each ToDoItem,
            creating a unique id for the row based on the ToDoId
        -->
        @foreach (var toDoModel in Model)
        {
            <tr id="editor-success-@toDoModel.ToDoId">
                @Html.Partial("_ToDoDataRow", toDoModel)
            </tr>
        }
    </tbody>
</table>
```
When rendering the table in the razor view each <tr> gets an ```id="editor-success-@toDoModel.ToDoId"``` id value that uniquely identifies the row, allowing the code to replace just the contents of the row.

The row itself is in a PartialView that is rendered by the ```@Html.Partial("_ToDoDataRow", toDoModel)``` HTML helper.

### Implementing the ModalDialog DIV
The DIV that will become the dialog is simple.

```HTML
<!--Modal Dialog For Editing ToDo Items-->
<div class="modal fade" id="editor-container" tabindex="-1">
        <div class="modal-dialog">
                <div class="modal-content" id="editor-content-container"></div>
        </div>
</div>
```
The first &lt;div&gt; has an id of ```"editor-container"``` to allow accessing it from JavaScript to show and hide it.  The inner &lt;div&gt; has an id of ```"editor-content-container"``` to allow putting arbitray content in the form.  This allow reusing the modal dialog for more than one modal dialog form if your page requires it.  

### Implementing the JavaScript to populate and show ModalDialog

### Implementing a PartialView for displaying individual items.

### Create a PartialView that has AJAX Form for the Modal content.

### Implement the controller methods for editing.


