
function GoogleMap(containerId, markerInfoFunction, options) {
    //Options:
    // ResetCallback (mapData) : function
    // MarkerClicked (mapData, marker) : function
    // MarkerAdded (mapData, marker) : function
    // markerClusterEnabled : bool
    // maxZoom : int  ( Greater means more zoomed in )
    // DisableMapInteraction : bool 

    if (!options) {
        options = {};
    }

    // Set any options defaults desired
    if (!options.maxZoom) {
        options.maxZoom = 15;
    }

    // The map's element which it will be contained within and hold the data
    var container = $(containerId);
    
    // Initialize a new object to hold the map data
    container.data().mapData = {};

    // Store a local reference to mapData
    var mapData = container.data().mapData;

    // Array to hold all added markers
    mapData.markers = [];

    // Define the style for the marker clusterer
    mapData.clusterStyle = {
        url: window.applicationBaseUrl + 'Content/Images/pin.png',
        height: 48,
        width: 30,
        anchor: [-18, 0],
        textColor: '#ffffff',
        textSize: 10,
        iconAnchor: [15, 48]
    };

    // Define the options for the markerClusterer
    mapData.clusterOptions = {
        maxZoom: 8, 
        gridSize: 40,
        zoomOnClick: false,
        styles: [mapData.clusterStyle]
    };

    // Construct the google map
    mapData.map = new google.maps.Map(container[0], {
        streetViewControl: true,
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
            mapTypeIds: [
              google.maps.MapTypeId.ROADMAP,
              google.maps.MapTypeId.SATELLITE
            ]
        },
        // We center on a reasonable point to make the map look good when no markers exist
        center: { lat: 18.3154961, lng: -15.1611112 },
        zoom: 6
    });

    // If the option of DisableMapInteraction is passed, disable all map interaction
    if (options.DisableMapInteraction) {
        mapData.map.setOptions({
            draggable: false,
            zoomControl: false,
            scrollwheel: false,
            disableDoubleClickZoom: true
        });
    }

    // Function that can be called to resize the map if the screen resizes
    // This currently is not used but could be used when we detect users changing their window size
    // 
    mapData.TriggerResize = function() {
        google.maps.event.trigger(mapData.map, 'resize');
    };

   
    mapData.imageSmActive = window.applicationBaseUrl + 'Content/Images/Icon-Map-active-1x.png';
    mapData.imageSmInactive = window.applicationBaseUrl + 'Content/Images/Icon-Map-inactive-1x.png';
    mapData.imageSmSelected = window.applicationBaseUrl + 'Content/Images/Icon-Map-selected-1x.png';

    if (options.markerClusterEnabled) {
        mapData.markerClusterer = new MarkerClusterer(mapData.map, mapData.markers, mapData.clusterOptions);

        google.maps.event.addListener(mapData.markerClusterer, 'clusterclick', function (cluster) {
            mapData.CenterMapAroundMarkers(cluster.markers_);
        });
    }

    mapData.GetMarkerInfoAndDraw = function() {
        var markerGpsCords = [];
        if (markerInfoFunction) {
            markerGpsCords = markerInfoFunction();
        }

        mapData.AddMarkersToMap(markerGpsCords);

        mapData.CenterMapAroundMarkers(mapData.markers);
    };

    mapData.ResetAndDrawSlow = function () {
        //If any rows were previously selected, unselect them.
        if (options && options.ResetCallback) {
            options.ResetCallback(mapData);
        }

        //Center the map around the existing markers as we're going to redraw them.
        mapData.CenterMapAroundMarkers(mapData.markers);

        //Clear the map
        mapData.ClearMarkers();

        //Get an array of the markers to add.
        var markerGpsCords = [];
        if (markerInfoFunction) {
            markerGpsCords = markerInfoFunction();
        }

        //Loop through the markers and draw them at a 1 second interval.
        var redrawCounter = 0;
        function RedrawLoop() {
            setTimeout(function () {
                mapData.AddMarker(markerGpsCords[redrawCounter]);
                redrawCounter++;
                if (redrawCounter < markerGpsCords.length) {
                    RedrawLoop();
                }                
            }, 1000);
        }

        //Start the loop.
        RedrawLoop();
    };
    
    mapData.ResetMap = function () {
        mapData.ClearMarkers();
        mapData.GetMarkerInfoAndDraw();
        if (options && options.ResetCallback) {
            options.ResetCallback(mapData);
        }
    };

    mapData.AddMarkersToMap = function(gpsCoordinates) {

        mapData.ClearMarkers();

        if(gpsCoordinates != null){
            for (var i = 0; i < gpsCoordinates.length; i++) {
                mapData.AddMarker(gpsCoordinates[i]);
            }
        }
    };

    function getMarkerIconForAngleAndHexColor(angle, hex) {
        var icon = {

            //this is the SVG path for an arrow symbol
            path: "M12,2L4.5,20.29L5.21,21L12,18L18.79,21L19.5,20.29L12,2Z",
            fillColor: hex,
            fillOpacity: 1,
            //the arrow is 24 x 24 so I am offsetting by 12 so that they stack on top of each other
            anchor: new google.maps.Point(12, 12),
            strokeWeight: 0,
            scale: 1,
            rotation: angle
        }

        return icon;
    }

    mapData.AddMarker = function(gpsCoordinates) {

        var marker = new google.maps.Marker({
            position: { lat: gpsCoordinates.latitude, lng: gpsCoordinates.longitude },
            map: mapData.map,
            icon: mapData.imageSmActive
        });

        if (gpsCoordinates.angle) {
            marker.angle = gpsCoordinates.angle;
            marker.icon = getMarkerIconForAngleAndHexColor(gpsCoordinates.angle, '#fc5b55');
        }

        if(gpsCoordinates.infowindow){
            marker.infowindow = gpsCoordinates.infowindow;
        }

        if (gpsCoordinates.markerId) {
            marker.id = gpsCoordinates.markerId;
        }

        if (gpsCoordinates.uncertainty) {
            marker.uncertainty = gpsCoordinates.uncertainty;
        }

        if (gpsCoordinates.icon) {
            marker.icon = gpsCoordinates.icon;
        }

        if (options && options.MarkerClicked && !options.DisableMapInteraction) {
            marker.addListener('click', function () {
                options.MarkerClicked(mapData, marker);
            });
        }

        if (options && options.MarkerAdded) {
            options.MarkerAdded(mapData, marker);
        }

        mapData.markers.push(marker);

        if (mapData.markerClusterer) {
            mapData.markerClusterer.addMarker(marker);
        }
    };

    mapData.GetMarkerById = function (markerId) {
        for (var i = 0; i < mapData.markers.length; i++) {

            var marker = mapData.markers[i];
            if (marker.id === markerId) {
                return marker;
            }
        }
    };

    mapData.RemoveMarker = function (marker) {
        marker.setMap(null);

        if (marker.circle) {
            marker.circle.setMap(null);
            marker.circle = null;
        }

        mapData.markers.splice(mapData.markers.indexOf(marker), 1);
        mapData.markerClusterer.removeMarker(marker);
    }

    mapData.ClearMarkers = function () {
        var len = mapData.markers.length;
        for (var i = 0; i < len; i++) {
            var removedMarker = mapData.markers.pop();

            if (removedMarker.circle) {
                removedMarker.circle.setMap(null);
                removedMarker.circle = null;
            }

            removedMarker.setMap(null);
        }

        if (mapData.markerClusterer != null) {
            mapData.markerClusterer.clearMarkers();
        }

    }
    mapData.ResetAllMakersExcept = function (exceptMarker) {
        for (var i = 0; i < mapData.markers.length; i++) {
            var tmpMarker = mapData.markers[i];

            if (tmpMarker !== exceptMarker) {
                if (tmpMarker.circle) {
                    tmpMarker.circle.setMap(null);
                    tmpMarker.circle = null;
                }
            }
        }
    }

    mapData.CenterMapAroundMarkers = function(markers, zoomLevel) {

        if (markers && markers.length < 1) {
            var latLng = new google.maps.LatLng(19.8107153, -34.6728299);
            mapData.map.setCenter(latLng);
            return mapData.map.setZoom(1);
        } else if (markers && markers.length === 1 && !zoomLevel) {
            zoomLevel = 15;            
        }
        var bounds = new google.maps.LatLngBounds();
        $.each(markers, function() { bounds.extend(this.getPosition()); });

        mapData.map.setCenter(bounds.getCenter()); //or use custom center

        if (markers && markers.length === 1 && markers[0].circle) {
            mapData.map.fitBounds(markers[0].circle.getBounds());
        } else {
            mapData.map.fitBounds(bounds);
        }


        if (zoomLevel) {
            google.maps.event.addListenerOnce(mapData.map, 'bounds_changed', function (event) {
                    mapData.map.setZoom(zoomLevel);
            });
        } else if (options.maxZoom) {
            google.maps.event.addListenerOnce(mapData.map, 'bounds_changed', function (event) {
                if (mapData.map.zoom > options.maxZoom) {
                    mapData.map.setZoom(options.maxZoom);
                }
            });
        }
    }


    // Uncertainty is displayed by adding a circle around the selected marker
    mapData.CreateCircle = function (mapData, clickedMarker) {

        // Read the lat/lng of which we center our circle around
        var lat = clickedMarker.position.lat();
        var lng = clickedMarker.position.lng();
        // Read out the uncertainty
        var uncertainty = clickedMarker.uncertainty;

        // Ensure there is not already a circle, that we have a lat/lng, and that we have a map ready
        if (!clickedMarker.circle && uncertainty && lat && lng && mapData && mapData.map) {
            var rangeCircle = new google.maps.Circle({
                strokeColor: '#0000FF',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#0000FF',
                fillOpacity: 0.20,
                map: mapData.map,
                center: { lat: lat, lng: lng },
                radius: parseInt(uncertainty)
            });
            
            // Store the circle to not duplicate and to clean up
            clickedMarker.circle = rangeCircle;
        }
    }

    mapData.GetMarkerInfoAndDraw();
    
    return mapData;
}