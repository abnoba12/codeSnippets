var urlParams;
$(document).ready(function () {
    getUrlParams();
	var id = urlParams["id"];
});

//Used to fetch the parameters out of the URL
function getUrlParams() {
	var match,
    pl = /\+/g,  // Regex for replacing addition symbol with a space
    search = /([^&=]+)=?([^&]*)/g,
    decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
    query = window.location.search.substring(1);

	urlParams = {};
	while (match = search.exec(query))
	    urlParams[decode(match[1])] = decode(match[2]);
};