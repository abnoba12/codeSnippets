//Usage example for googleMapWrapper.js

var mapData = [];
function initStoreMap(selector, enableClustering, disableMapInteraction) {
    var self = this;
    var trip;
    
    function markerClicked(mapData, clickedMarker) {
        
        //mapData.ResetAllMakersExcept(clickedMarker);
        
        clickedMarker.infowindow.open(mapData, clickedMarker);

        //mapData.displayStreetViewOption(clickedMarker);
    };

    var mapOptions = {
        MarkerClicked: markerClicked,
        //MarkerAdded: markerAdded,
        markerClusterEnabled: enableClustering,
        DisableMapInteraction: disableMapInteraction
    };

    function getMarkerInfo() {
        var gpsCoordinates = [];

        var pinColor = "66cc33";
        var pinImage = new google.maps.MarkerImage("http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + pinColor,
            new google.maps.Size(21, 34),
            new google.maps.Point(0, 0),
            new google.maps.Point(10, 34));
        var data = VmStore.SalesSectionVm.ShopVm.ShopLocations();
        for (var i = 0; i < data.length; i++) {
            var name = data[i].LegalName?"<div>" + data[i].LegalName + "</div>":"";
            var hours = data[i].HoursOfOperation?"<div>" + data[i].HoursOfOperation + "</div>":"";

            var coord = {
                'markerId': i,
                'latitude': parseFloat(data[i].Latitude),
                'longitude': parseFloat(data[i].Longitude),
                'icon': pinImage,
                'infowindow': new google.maps.InfoWindow({
                    content: name + hours
                }),
            };

            gpsCoordinates.push(coord);
        }

        var pinColor = "E8E8E8";
        var pinImage = new google.maps.MarkerImage("http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + pinColor,
            new google.maps.Size(21, 34),
            new google.maps.Point(0, 0),
            new google.maps.Point(10, 34));
        data = VmStore.SalesSectionVm.ShopVm.CompetitorLocations();
        for (var i = 0; i < data.length; i++) {
            var coord = {
                'markerId': i,
                'latitude': parseFloat(data[i].Latitude),
                'longitude': parseFloat(data[i].Longitude),
                'icon': pinImage,
                'infowindow': new google.maps.InfoWindow({
                    content: "<div>" + data[i].Name + "</div><div>Company: " + data[i].Company + "</div>"
                }),
            };

            gpsCoordinates.push(coord);
        }

        return gpsCoordinates;
    }

    mapData[selector] = GoogleMap(selector, getMarkerInfo, mapOptions);
}