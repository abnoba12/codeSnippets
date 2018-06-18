function IsVmBound(id) {
    if (document.getElementById(id)) {
        return !!ko.dataFor(document.getElementById(id));
    } else {
        return undefined;
    }
} 

/**
 * @description Keep checking to see if an element is loaded into the dom. if it is loaded into the down, the dom is ready, and the element is visible then call it's callback function. If it is not loaded then check again in 100ms. Times out if the element doesn't exist after 30 seconds.
 * @param {string} selector The selector string to be searched for by JQuery.
 * @param {function} callback Callback function to be called once the item is loaded into the dom
 * @param {function} failed Callback function to be called if the element is never found
 * @param {boolean} ignoreVisible If true then don't wait for the element to also be visible before calling the callback
 * @returns {JQuery} Returns the result of the selector as a JQuery object.
 */
function WaitForLoaded(selector, callback, failed, ignoreVisible, attempts = 0) {
    if (jQuery(selector).length && (ignoreVisible || jQuery(selector).is(":visible"))) {
        $(function () {
            return callback(jQuery(selector));
        });
    } else {
        if (attempts < 300) { // Stop trying after 30 seconds
            attempts++;
            setTimeout(function () {
                WaitForLoaded(selector, callback, failed, ignoreVisible, attempts);
            }, 100);
        } else {
            GlobalHandler('Unable to bind "' + selector + '" Due to binding timeout.', 'debug');
            return Call(failed);
        }
    }
}

/**
 * @description Keep checking to see if an element is loaded into the dom. if it is loaded into the down, the dom is ready, and the element is visible then call it's callback function. If it is not loaded then check again in 100ms. Times out if the element doesn't exist after 30 seconds.
 * @param {string} selector The selector string to be searched for by JQuery.
 * @param {function} callback Callback function to be called once the item is loaded into the dom
 * @param {function} failed Callback function to be called if the element is never found
 * @returns {kendoDropDownList} Provides the KDD as a parameter to the callback once it is ready.
 */
function WaitForKDDLoaded(selector, callback, failed, ignoreVisible, attempts = 0) {
    var item = $(selector);
    var kdd = item ? item.data('kendoDropDownList') : undefined;
    if (item.length && Boolean(kdd) && Boolean(kdd.value)) {
        $(function () {
            return callback(kdd);
        });
    } else {
        if (attempts < 300) { // Stop trying after 30 seconds
            attempts++;
            setTimeout(function () {
                WaitForKDDLoaded(selector, callback, failed, attempts);
            }, 100);
        } else {
            GlobalHandler('Unable to bind the "' + selector + '" drop down Due to binding timeout.', 'debug');
            return Call(failed);
        }
    }
}

//Keyboard Navigation Setup for TabStrips
// Next tab: ctrl + Left arrow
// Prev tab: ctrl + Right arrow
function TabStripKeyboardNavigation(parentContainerId, kendoTabStripId) {
    $(document).on('keydown.TabStripKeyboardNavigation keypress.TabStripKeyboardNavigation', function (e) {
        var tabstrip = $(kendoTabStripId).data('kendoTabStrip');
        if (tabstrip) {
            var index = tabstrip.select().index();
            if (e.keyCode == 39 && e.ctrlKey) {
                var nextTabIndex = nextTab(tabstrip, index);
                if (index < nextTabIndex) {
                    tabstrip.select(nextTabIndex);
                }
            } else if (e.keyCode == 37 && e.ctrlKey && index != 0) {
                var prevTabIndex = prevTab(tabstrip, index);
                if (index > prevTabIndex) {
                    tabstrip.select(prevTabIndex);
                }
            }
        }
    });
}

function nextTab(kendoTabStrip, currentTabIndex) {
    var tabStripItems = kendoTabStrip.tabGroup.find('.k-item');
    var tabStripLength = tabStripItems.length;

    for (tabCheckIndex = currentTabIndex; tabCheckIndex < tabStripLength; tabCheckIndex++) {
        var nextTabIndex = tabCheckIndex + 1;
        var nextTab = $(tabStripItems[nextTabIndex]);
        if (!nextTab.hasClass('k-state-disabled') && nextTab.is(':visible')) {
            return nextTabIndex;
        }
    }
    return currentTabIndex;
}

function prevTab(kendoTabStrip, currentTabIndex) {
    var tabStripItems = kendoTabStrip.tabGroup.find('.k-item');

    for (tabCheckIndex = currentTabIndex; tabCheckIndex > 0; tabCheckIndex--) {
        var prevTabIndex = tabCheckIndex - 1;
        var prevTab = $(tabStripItems[prevTabIndex]);
        if (!prevTab.hasClass('k-state-disabled') && prevTab.is(':visible')) {
            return prevTabIndex;
        }
    }
    return currentTabIndex;
}


//Keyboard Navigation
//Make it so you can press enter to dial instead having to click the dial button
$('#make-call-phone-number').ready(function () {
    $('#make-call-phone-number').on('focus', function () {
        $(document).on('keypress.DialPhoneNumber', function (e) {
            if (e.keyCode == 13) { //Enter
                VmStore.HeaderVm.MakeCall();
            }
        });
    });
    //Remove the dial listener when not typing numbers
    $('#make-call-phone-number').on('blur', function () {
        $(document).off('keypress.DialPhoneNumber');
    });
});

function IsViewModel(item) {
    return typeof (item) === 'object' && 'Init' in item && 'Reset' in item && item.constructor.name.includes('Vm');
}

function IsKoArray(item, ignorePopulated = true) {
    return ko.isObservable(item) && 'push' in item && (ignorePopulated || IsNotEmptyArray(item()));
}

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
            if (IsKoArray(vm[key])) { // observable array property
                vm[key]([]);
            } else if (vm.hasOwnProperty(key) && ko.isWriteableObservable(vm[key])) { // observable property
                vm[key](undefined);
            } else if (IsViewModel(vm[key])) { // vm property
                vm[key].Reset();
            }
        }
        if ('Init' in vm) {
            vm.Init();
        }
        if ('TabSelector' in vm) {
            clearValidation(vm.TabSelector);
        }
    } catch (error) {
        GlobalHandler(error, 'error');
    }
}

/**
 * @description Copy all the data from one knockout VM to another.
 * @param {object} fromVm The knockout VM with the current data.
 * @param {object} toVm The destination VM that will hold all the data.
 * @param {Array} ignoreProperties Array of properties to NOT copy.
 * @param {boolean} recur Default: true. If true then it will recursivly copy children Vms
 */
function CopyVm(fromVm, toVm, ignoreProperties, recur = true) {
    try {
        for (var key in fromVm) {
            if (ignoreProperties && ignoreProperties.includes(key)) {
                continue;
            }

            if (IsKoArray(fromVm[key], false) && IsKoArray(toVm[key])) { // observable array
                toVm[key](fromVm[key]());
            } else if (recur && IsViewModel(toVm[key])) { // Vm
                CopyVm(fromVm[key], toVm[key], ignoreProperties);
            } else if (fromVm.hasOwnProperty(key) && ko.isWriteableObservable(fromVm[key]) && toVm.hasOwnProperty(key) && ko.isWriteableObservable(toVm[key])) { // Standard property
                toVm[key](fromVm[key]());
            } 
        }
    } catch (error) {
        GlobalHandler(error, 'error');
    }
}

/**
 * @description Generate a list of fields to exclude from serialization
 * @param {Knockout View Model} vm The Vm we want to itterage over and create a black list for
 * @param { Array<string> } vmWhiteList List of Vm names that we want none of the fields included in this black list. The blackList parameter takes precedent over fields skipped by this.
 * @param { Array<string> } propWhiteList List of fields we do not want on the black list
 * @param { Array<string> } blackList List of fields that we always want on this list. This takes priority over all other parameters passed in.
 */
function GetBlackList(vm, vmWhiteList, propWhiteList, blackList) {
    try {
        if (!blackList || !Array.isArray(blackList)) { blackList = new Array(); }

        //Create a new instances of the VM and use its values to reset the existing vm back to origional defaults
        for (var key in vm) {
            if (vmWhiteList && vmWhiteList.includes(key)) {
                continue;
            }

            var whiteListed = propWhiteList && propWhiteList.includes(key);
            
            if (whiteListed && IsViewModel(vm[key])) { // vm
                blackList = blackList.concat(GetBlackList(vm[key], vmWhiteList, propWhiteList, blackList));
            } else if (!whiteListed && !blackList.includes(key)) {
                blackList.push(key);
            }
        }
        return blackList;
    } catch (error) {
        GlobalHandler(error, 'error');
    }
}

/**
 * @description Format a currency to only show 2 decimal points.
 * @param {object} value The number to be formatted
 */
function formatCurrency(value) {
    if (!isNaN(parseFloat(value))) {
        return '$' + parseFloat(value).toFixed(2);
    } else {
        return value;
    }    
}

/**
 * @description Populate a region kendo dropdown's data based on the country
 * @param {string} selector The selector of the region kendo dropdown
 * @param {string} countryIso The country ISO code
 */
function PopulateRegion(selector, countryIsoCode) {
    var dd = $(selector).data('kendoDropDownList');
    if (dd) {
        if (countryIsoCode) {
            $.ajax({
                url: '/Address/GetRegions',
                type: 'GET',
                data: {
                    countryIsoCode: countryIsoCode
                },
                beforeSend: function () {
                    kendoSpinner(dd, true);
                },
                cache: true,
                complete: function () {
                    kendoSpinner(dd, false);
                },
            }).done(function (data) {
                dd.dataSource.data(data);
                dd.enable(true);
            });
        } else {
            dd.value(' ');
            dd.enable(false);
        }
    }
}

/**
 * @description Turn on and off the data loading spinner. Currently only supports dropdowns
 * @param {kendoObject} kendoObject The kendo object resulting from .data
 * @param {string} loading Boolean to turn on and off of the spinner
 */
function kendoSpinner(kendoObject, loading) {
    if (kendoObject.ns === '.kendoDropDownList') {
        if (loading) {
            $(kendoObject._arrowIcon).addClass('k-i-loading');
        } else {
            $(kendoObject._arrowIcon).removeClass('k-i-loading');
        }
    }
}

/**
 * @description Flatten an array of arrays.
 *https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/Reduce#Flatten_an_array_of_arrays
 * @param {array} arrayOfArrays An array of arrays
 */
function flatten(arrayOfArrays) { 
    return arrayOfArrays.reduce(
        (acc, cur) => acc.concat(cur), []);
}

/**
 * @description Retrieves the value for a given query parameter key.
 * @param {string} name name of the parameter key
 */
function getUrlParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [null, ''])[1].replace(/\+/g, '%20')) || null;
}

function hideInput(selector, hide) {
    var ele = $(selector);
    if (hide && ele.is('input[type="text"]')) {
        ele.attr('type', 'password');
        ele.prop('disabled', true);
    } else if (!hide && ele.is('input[type="password"]')) {
        ele.attr('type', 'text');
        ele.prop('disabled', false);
    }
}

/**
 * @description Returns true if the string contains the starts with value 
 * @param {string} string string to be validated
 * @param {string} startsWith test match
 */
function stringStartsWith(string, startsWith) {
    string = string || "";
    if (startsWith.length > string.length)
        return false;
    return string.substring(0, startsWith.length) === startsWith;
}

/**
 * @description Timer function that counts in seconds 
 * @param {Knockout Observerable} timerSeconds Time elapsed since start() in seconds
 * @param {Knockout Observerable} timerFormatted (Optional) Time elapsed formatted in h:mm:ss
 * @param {number} countDown (Optional) Makes the timer count down from the countDown number provided
 */
function timer(timerSeconds, timerFormatted, countDown) {
    var self = this;
    timerSeconds(undefined);
    var updateTimer = function () {
        var time = moment().utcOffset(0);
        time.set({ hour: 0, minute: 0, second: timerSeconds(), millisecond: 0 });

        if (time.hour() > 0 && timerSeconds() >= 0 && timerFormatted) {
            timerFormatted(time.format('h:mm:ss'));
        } else if (timerSeconds() >= 0 && timerFormatted) {
            timerFormatted(time.format('m:ss'));
        }

        if (countDown) {
            timerSeconds(timerSeconds() - 1);
        } else {
            timerSeconds(timerSeconds() + 1);
        }
    };

    var init = function () {
        self.Timer = $.timer(updateTimer, 1000, true);
    };

    self.stopAndReset = function () {
        timerSeconds(undefined);
        timerFormatted ? timerFormatted(undefined) : undefined;
        if (self.Timer) {
            self.Timer.stop().once();
        }
    };

    self.start = function () {
        if (isNaN(timerSeconds())) { //Can only start a timer if it is stopped
            timerSeconds(countDown ? countDown : 0);
            $(init);
        }
    };
}

function tossCookies() {
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookieName = cookies[i].split('=')[0].trim();
        if (cookieName && cookieName.startsWith('CCA_')) {
            Cookies.remove(cookieName);
        }
    }
}

// Quick fix for scrolling on opening a dropdown - All credit goes to Dylan
window.onkeydown = function (e) {
    if (e.keyCode == 32) {
        var dropdowns = $('.k-widget.k-dropdown');
        for (var i = 0; i < dropdowns.length; i++) {
            if (e.target == dropdowns[i]) {
                e.preventDefault();
                break;
            }
        }
        if (e.target == $('#tabstrip-left-sales')[0] ) {
            e.preventDefault();
        }
    }
};

//binds all tooltips when called
function BindTooltips() {
	$('[data-toggle="tooltip"]').tooltip({ delay: { "show": 500, "hide": 0 } });
}

// Only call a function if it exists
function Call(functionToCall) {
    if (functionToCall && IsFunction(functionToCall)) {
        functionToCall();
    } else {
        GlobalHandler('Function doesnt exist ' + functionToCall, 'debug');
    }
}

/**
 * @description Take a kockout vm path such as 'VmStore.PricingPaneVm.PricingVm' and return its vm
 * @param {string} vmString Path to a vm
 * @returns {Kockout Vm} Corresponding Vm that matches the provied path
 */
function VmStringToVm(vmString) {
    var vm = undefined;
    var parts = vmString.split('.');
    if (IsNotEmptyArray(parts)) {
        parts.forEach(part => {
            if (vm && part in vm) {
                vm = vm[part];
            } else if (part in window) {
                vm = window[part];
            } else {
                vm = undefined;
            }
        });
    }
    return vm;
}

//function VmFromJSON(data, vm) {
//    VmFromJS(JSON.parse(data), vm);
//}

//function VmFromJS(data, vm) {
//    //Don't convert these things into observerables
//    var mapping = { copy: ["Name"] }; // Why not work?
//    var result = ko.mapping.fromJS(data, mapping);
//    CopyVm(result, vm);
//}