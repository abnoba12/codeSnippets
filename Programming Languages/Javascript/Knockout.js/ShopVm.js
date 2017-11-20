//Good all around examples of knockout.js
function ShopVm() {
	var self = this;

	//#region Variables & Observables
	self.ZipToUse = ko.observable("Home");
	self.OtherZip = ko.observable();
	self.ViewMap = ko.observable(true);

	self.ShopLocations = ko.observableArray().extend({ rateLimit: 500 }); //Rate limited to give time for ajax updates
	self.CompetitorLocations = ko.observableArray().extend({ rateLimit: 500 }); //Rate limited to give time for ajax updates

	//This is just to temporarly hold the selected appointment until they book it
	self.selectedAppointment = new GlobalAppointmentVm();

	self.GetZipCode = ko.computed(function () {
		if (VmStore.GlobalCustomerVm.HomeZip() && self.ZipToUse() === "Home") {
			return VmStore.GlobalCustomerVm.HomeZip();
		} else if (VmStore.GlobalCustomerVm.WorkZip() && self.ZipToUse() === "Work") {
			return VmStore.GlobalCustomerVm.WorkZip();
		} else if (self.ZipToUse() === "Other") {
			return self.OtherZip();
		} else if (VmStore.GlobalCustomerVm.HomeZip()) {
			return VmStore.GlobalCustomerVm.HomeZip();
		}
	});
	//#endregion Variables & Observables

	//#region Methods
	self.loaded = function (e) {
		self.validator();
		initStoreMap('#shop-locations-map', false, false);
		self.ViewMap(false);
	}

	self.validatedfistTime = false;
	self.validator = function () {
		return $().kendoValidator({
			validate: function (e) {
				//Everything is filled out and valid 
				if (e.valid) {
					self.validatedfistTime = true;
				}
			},
			validateInput: function (e) {
				//After we have completed the form the first time, validate all fields every time we change any field
				if (self.validatedfistTime) {
					self.validator().validate();
				}
			}
		}).data("kendoValidator");
	}

	self.loadStoreData = function (done) {
		$.ajax({
			type: 'GET',
			url: '/Sales/GetStoreById',
			data: {
				storeId: self.selectedAppointment.shopId()
			}
		})
			.done(function (data) {
				self.selectedAppointment.shopShortName(data.ShortName);
				self.selectedAppointment.shopLegalName(data.LegalName);
				self.selectedAppointment.shopImage();
				self.selectedAppointment.shopHoursOfOperation(data.HoursOfOperation);
				self.selectedAppointment.shopPhone(data.PhoneNumber);
				self.selectedAppointment.shopNotes(data.Message);
				self.selectedAppointment.shopPaymentMethods();
				self.selectedAppointment.shopCountry();
				self.selectedAppointment.shopAddress1(data.Address.Address1);
				self.selectedAppointment.shopAddress2(data.Address.Address2);
				self.selectedAppointment.shopCity(data.Address.City);
				self.selectedAppointment.shopState(data.Address.Region);
				self.selectedAppointment.shopZip(data.Address.PostalCode);
				done();
			})
			.fail(function (error) { console.log(error); });
	}

	self.ResetMaps = function () {
		//Update the maps with the new location data
		if (mapData['#shop-locations-map']) {
			mapData['#shop-locations-map'].ResetMap();
		}
		if (mapData['#salesMiniMap']) {
			mapData['#salesMiniMap'].ResetMap();
		}
	}

	self.GetShopLocations = function () {
		self.ShopLocations.removeAll();
		var country = VmStore.GlobalCustomerVm.GetDefaultCountry();
		var zip = self.GetZipCode();

		if (VmStore.CompanyId() && country && zip) {
			$.ajax({
				type: 'POST',
				url: '/Sales/GetShopLocationsByZipCode',
				data: {
					companyId: VmStore.CompanyId(),
					countryCode: country,
					zipCode: zip,
				}
			})
			.done(function (data) {
				for (item of data) {
					self.ShopLocations.push(item);
				}
			})
			.fail(function (request, status, error) {
				console.log(error);
			});
		}
	}

	self.GetCompetitorLocations = function () {
		self.CompetitorLocations.removeAll();
		var country = VmStore.GlobalCustomerVm.GetDefaultCountry();
		var zip = self.GetZipCode();

		if (VmStore.CompanyId() && country && zip) {
			$.ajax({
				type: 'POST',
				url: '/Sales/GetCompetitorLocationsByZipCode',
				data: {
					companyId: VmStore.CompanyId(),
					countryCode: country,
					zipCode: zip,
				}
			})
				.done(function (data) {
					for (item of data) {
						self.CompetitorLocations.push(item);
					}
				})
				.fail(function (request, status, error) {
					console.log(error);
				});
		}
	}

	self.Reset = function () {
		for (var key in self) {
			if (ko.isObservable(self[key]) && 'push' in self[key]) { //for resetting observerable arrays
				self[key]([]);
			} else if (self.hasOwnProperty(key) && ko.isWriteableObservable(self[key])) { // Set Observables to undefined
				self[key](undefined);
			} else if (typeof (self[key]) === "object" && 'Reset' in self[key]) { // Call subVMs reset function
				self[key].Reset();
			}
		}
	};
	//#endregion Methods

	//#region Subscribers
	self.ZipToUse.subscribe(function (newValue) {
		$('#shop-locations').data("kendoGrid").dataSource.read();
	});

	self.GetZipCode.subscribe(function (newValue) {
		//Fetch new location data if we have a new zip code
        if (newValue && newValue.match(/^\d{5}$/)) {
            self.GetShopLocations();
            self.GetCompetitorLocations();
		}
	});

	self.ViewMap.subscribe(function (newValue) {
		setTimeout(function () { //zoom level won't work until the map is visable on the page. Wait unti map is visible'
			//Reset the zoom level on the map when we open it
			if (newValue && mapData['#shop-locations-map'] && mapData['#shop-locations-map'].markers) {
				mapData['#shop-locations-map'].CenterMapAroundMarkers(mapData['#shop-locations-map'].markers)
			}
		}, 10);
	});

	self.ShopLocations.subscribe(self.ResetMaps);
	self.CompetitorLocations.subscribe(self.ResetMaps);
	//#endregion Subscribers
}
