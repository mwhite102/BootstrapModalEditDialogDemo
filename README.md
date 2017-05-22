# Bootstrap Modal Edit Dialog Demo
A demo using Bootstrap 3 modal dialogs in an ASP.NET application

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
