File Locations:
	\views\Shared\_Layout.cshtml
	\views\linkscheatsheet\_links.cshtml
	\views\sales\index.cshtml
	\scripts\pages\linkscheatsheet\_links.js
	\extensions\htmlextensions.cs

Directions:
Partial Views do not support "@section Scripts". If you want to include javascript related to a partial view then you need to include the script in the parent view. This code gets you arround this limitation.

This script will only include the script once no matter how many times the partial view is added.

1. create your partial view _Links.cshtml and it's corrasponding javascript _links.js
2. Add your javascript to the partial view with the helper:
	@Html.AddScript("~/Scripts/Pages/LinksCheatsheet/_links.js")
3. In a location you want all of the scripts from all of the partial views to be written to the page add the line:
	@Html.RenderPartialScripts()
This will gather and output only the javascript needed for the partial views on the page that is being loaded. It will only load the javascript once and not per partial view.
4. Add your partial view to your desired view:
	@Html.Partial("~/Views/LinksCheatsheet/_Links.cshtml")