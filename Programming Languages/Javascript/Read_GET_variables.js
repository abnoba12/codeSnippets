//Taken from http://papermashup.com/read-url-get-variables-withjavascript
//Use
var first = getUrlVars()["id"];
var second = getUrlVars()["page"];
 
alert(first);
alert(second);

//Function to pull GET variables form the URL
function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
        vars[key] = value;
    });
    return vars;
}