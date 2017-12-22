/**
 * @description Will convert the knockout variable to upper to lower case as it is input.
 * Example Usage: self.vehicleVIN = ko.observable().extend({ changecase: 'upper' });
 * @param {ko.observable} target The knockout observable we want to convert the case of
 * @param {string} option Can pass 'upper' or 'lower' to convert case accordingly
 */
ko.extenders.changecase = function (target, option) {
	if (option === 'upper') {
		target.subscribe(function (newValue) {
			target(newValue.toUpperCase());
		});
	} else if (option === 'lower') {
		target.subscribe(function (newValue) {
			target(newValue.toLowerCase());
		});
	}
	return target;
};