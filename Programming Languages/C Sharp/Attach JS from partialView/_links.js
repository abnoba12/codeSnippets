$(document).ready(function () {
    var countryDD = $(".country");
    var countries = countryDD.kendoDropDownList({
        optionLabel: "Select a country",
        dataTextField: "name",
        dataValueField: "iso",
        dataSource: {
            dataType: "json",
            transport: {
                read: "/LinksCheatsheet/GetCountries",
            },
            schema: {
                data: "countries"
            }
        }
    }).data("kendoDropDownList");

    //Update the region DD when a country is selected
    countryDD.change(function () {
        if (countryDD.val()) {
            regions.dataSource.transport.options.read.url = "/LinksCheatsheet/GetRegionByCountry?countryIsoCode=" + countryDD.val();
            regions.dataSource.read();
            regions.enable(true);
        } else {
            regions.value("");
            regions.enable(false);
            $(".regionLinkList").html("");
        }
    });


    var regionDD = $(".region").html("");
    var regions = regionDD.kendoDropDownList({
        autoBind: false,
        cascadeFrom: "countries",
        optionLabel: "Select a region",
        dataSource: {
            dataType: "json",
            transport: {
                read: ""
            },
            schema: {
                data: "regions"
            }
        }
    }).data("kendoDropDownList");

    //Update the Region link list when the region is selected
    regionDD.change(function () {
        $(".regionLinkList").html("");
        var selectedCountry = countries.value();
        var selectedRegion = regions.value();
        if (selectedRegion) {
            $.ajax({
                type: "GET",
                url: "/LinksCheatsheet/GetByCountryAndRegion",
                data: {
                    "countryIsoCode": selectedCountry,
                    "region": selectedRegion
                },
                error: function (xhr, status, error) {
                    console.log("Error: " + error);
                },
                success: function (response) {
                    var regionLinks = response.regionLinks;
                    var regionLinkList = $(".regionLinkList").html("");
                    var table = $('<table class="table table-striped">');
                    table.append($('<tbody>'));
                    if (regionLinks && regionLinks.length > 0) {
                        $.each(regionLinks, function (index, value) {
                            var label = value.Label ? value.Label : value.Href;
                            var link = $('<a target="_blank">').text(label).attr("href", value.Href);
                            var col = $('<td>');
                            var row = $('<tr>');
                            table.append(row.append(col.append(link)));
                        });
                    } else {
                        var link = $("<span>").text("No Links");
                        var col = $('<td>');
                        var row = $('<tr>');
                        table.append(row.append(col.append(link)));
                    }
                    regionLinkList.append(table);
                }
            });
        }
    });
});