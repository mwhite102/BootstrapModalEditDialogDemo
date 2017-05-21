//-----------------------------------------------------------------
// ToDo Editing Code
//-----------------------------------------------------------------

// Wire put the click event for 'Edit' links for each ToDo row
$(document).on('click', '.todo_editor-container', function (event) {
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

//-----------------------------------------------------------------
// ToDo Creation Code
//-----------------------------------------------------------------

// Wire put the click event for 'Add' button
//$(document).on('click', '#add-button', function (event) {
//    event.preventDefault();

//    var url = "/Home/AddTodo";

//    $.get(url , function (data) {
//        // Load the data (the partial view) into the modal dialog
//        $('#editor-content-container').html(data);
//        // Show the modal dialog
//        $('#editor-container').modal('show');
//    });
//});