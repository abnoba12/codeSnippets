/**
 * @description Used to reset all knockout Observables inside of a VM. This will also call the reset function inside of any child VM. This also calls a VM's Init function if it has one. This is to reset variables back to their initial default state.
 * @param {object} vm The knockout VM to be reset.
 * @param {Array} ignoreProperties Array of properties to NOT be reset
 */
function ResetVm(vm, ignoreProperties) {
    try {
        //Create a new instances of the VM and use its values to reset the existing vm back to origional defaults
        for (var key in vm) {
            if (ignoreProperties && ignoreProperties.includes(vm[key])) {
                continue;
            }
            if (ko.isObservable(vm[key]) && 'push' in vm[key]) { // observable array property
                vm[key]([]);
            } else if (vm.hasOwnProperty(key) && ko.isWriteableObservable(vm[key])) { // observable property
                vm[key](undefined);
            } else if (typeof (vm[key]) === 'object' && 'Reset' in vm[key]) { // vm property
                vm[key].Reset();
            }
        }
        if ('Init' in vm) {
            vm.Init();
        }
    } catch (error) {
        GlobalErrorHandler(error);
    }
}

/**
 * @description Copy all the data from one knockout VM to another
 * @param {object} fromVm The knockout VM with the current data.
 * @param {object} toVm The destination VM that will hold all the data.
 */
function CopyVm(fromVm, toVm) {
    try {
        for (var key in fromVm) {
            if (fromVm.hasOwnProperty(key) && ko.isWriteableObservable(fromVm[key]) &&
                toVm.hasOwnProperty(key) && ko.isWriteableObservable(toVm[key])) {
                toVm[key](fromVm[key]());
            }
        }
    } catch (error) {
        GlobalErrorHandler(error);
    }
}

/**
 * @description C# passes dates in this format "/Date(-62135575200000)/", this will parse them into Javascript Date objects
 * @param {string} date The date from C# in "/Date(-62135575200000)/" format.
 * @returns {Date} Returns a javascript date object
 */
function ParseDateFromNet(date) {
    try {
        if (date) {
            var parsedDate = new Date(parseInt(date.substr(6)));
            return parsedDate && parsedDate.getFullYear() > 1 ? parsedDate : undefined;
        } else {
            return undefined;
        }
    } catch (error) {
        GlobalWarningHandler(error);
    }
}