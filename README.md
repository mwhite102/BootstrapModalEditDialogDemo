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

To get started, a Plain Old CLR Object (POCO) model is created that is used to populate the list of items in the page.

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

It is using DataAnnotations for validation on the Description property.

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
When rendering the table in the razor view each &lt;tr&gt; gets an ```id="editor-success-@toDoModel.ToDoId"``` id value that uniquely identifies the row, allowing the code to replace just the contents of the row.

The row itself is in a PartialView that is rendered by the ```@Html.Partial("_ToDoDataRow", toDoModel)``` HTML helper.

```HTML
<td>@Model.ToDoId</td>
<td>@Model.Description</td>
<td class="text-center"><span @if (Model.Completed) { <text> class="glyphicon glyphicon-ok text-success" </text> }></span></td>
<td><a href="#" class="todo-editor-container" data-id="@Model.ToDoId">Edit</a></td>
<td><a href="#" class="todo-delete-container" data-id="@Model.ToDoId">Delete</a></td>
```

Notice that the 'Edit' and 'Delete' anchors have a class of 'todo-editor-container' and 'todo-delete-container' respectively.  Those classes will be used in JavaScript to tie click events to each one.

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
The JavaScript to show the ModalDialog is straight forward.  The jQuery 'on' is used to wire up each 'Edit' anchors in table rows.  When the anchor is clicked, the event handler makes an ajax call to the controller that returns the PartialView that is the form contents.  The PartialView is then set as the html content of the editor-content-container DIV, and the editor-container &lt;div&gt; is shown.

The toDoEditSuccess function is called when the item is successfully editied and updated and simply hides the &lt;div&gt; and removes the contents of the editor-content-container &lt;div&gt;.

The toDoEditFailure function is called when the edit fails.  The contents of the editor-content-container &lt;div&gt; is replaced with the xhr.responseText and will be the PartialView form contents, again, with validation errors added, and the editor-container &lt;div&gt; is shown again.

```JavaScript
// Wire put the click event for 'Edit' links for each ToDo row
$(document).on('click', '.todo-editor-container', function (event) {
    event.preventDefault();

    var url = "/Home/EditTodo";
    var id = $(this).attr('data-id');

    $.get(url + '/' + id, function (data) {
        // Load the data (the partial view) into the modal dialog
        $('#editor-content-container').html(data);
        // Show the modal dialog
        $('#editor-container').modal('show');
    });   
});

// Called when the ToDo edit is successful
function toDoEditSuccess(data, status, xhr) {
    // Hide the modal dialog
    $('#editor-container').modal('hide');
    // Clear the contents of the modal dialog
    $('#editor-content-container').html("");
}

// Called when the ToDo edit fails
function toDoEditFailure(xhr, status, error) {
    // Reload the modal dialog content with the response
    $('#editor-content-container').html(xhr.responseText);
    // Show the modal dialog
    $('#editor-container').modal('show');
}
```

### Create a PartialView that has Ajax Form for the Modal content.
The Partial View for the form uses an Ajax.BeginForm() helper with an AjaxOptions object being passed to it.

The AjaxOptions is where good things happen.  

The InsertionMode is set to InsertionMode.Replace.  What comes back from the server on a successful form post will replace the contents of the update target automatically.  In this case, it will be a table row's content.

The HttpMethod for the form is POST, as expected.

The UpdateTargetId is the id of the HTML element to replace the contents of with the PartialView (the table row) from a succssful form post.

OnSuccess specifies the JavaScript function to call after a successful post.

OnFailure specifies the JavaScript function to call after a failed post.

```HTML
@model BootstrapModalEditDialogDemo.Models.ToDoModel

<div class="modal-header">
    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
    <h4 class="modal-title" id="editor-title">ToDo Properties</h4>
</div>

<div class="modal-body">

    <!--
        Configure the AjaxOptions
    -->

    @using (Ajax.BeginForm("EditToDo", "Home", new AjaxOptions
    {
        InsertionMode = InsertionMode.Replace,
        HttpMethod = "POST",
        UpdateTargetId = "editor-success-" + Model.ToDoId, 
        OnSuccess = "toDoEditSuccess",
        OnFailure = "toDoEditFailure"
    }))
    {
        @Html.Partial("_ToDoEditFormContent", Model)
    }
</div>
<scripts>
    @*
        This view is displayed dynamically via an Ajax call.
        Because of this, these script references are required here in this view
        or unobtrusive validation will not work.
    *@
    @Scripts.Render("~/bundles/jqueryval")
</scripts>
```
The form fields themselves are in yet another PartialView and are reused in the Ajax form that adds new ToDo items.  That Ajax form post to a different controller method, but the fields for the form are identical. It makes sense to use a PartialView instead of duplicate code.  (Don't repeat yourself)

### Implement the controller methods for editing.

The controller methods for editing and saving a ToDoItem are straight forward if you're familiar with ASP.NET MVC. The GET returns the edit form PartialView and the POST updates the ToDoItem and returns a PartialView for the table row details for the item.

```C#
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
```

