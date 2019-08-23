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
    var ele = jQuery(selector);
    if (ele.length && (ignoreVisible || ele.is(':visible'))) {
        $(function () {
            return callback(ele);
        });
    } else {
        if (attempts < 300) { // Stop trying after 30 seconds
            attempts++;
            setTimeout(function () {
                return WaitForLoaded(selector, callback, failed, ignoreVisible, attempts);
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
function WaitForKDDLoaded(selector, callback, failed) {
    return WaitForKendoLoaded(selector, 'kendoDropDownList', callback, failed);
}

/**
 * @description Keep checking to see if an element is loaded into the dom. if it is loaded into the down, the dom is ready, and the element is visible then call it's callback function. If it is not loaded then check again in 100ms. Times out if the element doesn't exist after 30 seconds.
 * @param {string} selector The selector string to be searched for by JQuery.
 * @param {string} controlType The type of kendo control this is.
 * @param {function} callback Callback function to be called once the item is loaded into the dom
 * @param {function} failed Callback function to be called if the element is never found
 * @returns {kendoControl} Provides the KendoControl as a parameter to the callback once it is ready.
 */
function WaitForKendoLoaded(selector, controlType, callback, failed, attempts = 0) {
    var item = $(selector);
    var kdd = item ? item.data(controlType) : undefined;
    if (item.length && Boolean(kdd) && (controlType != 'kendoDropDownList' || Boolean(kdd.value))) {
        $(function () {
            return callback(kdd);
        });
    } else {
        if (attempts < 300 && selector) { // Stop trying after 30 seconds
            attempts++;
            setTimeout(function () {
                return WaitForKendoLoaded(selector, controlType, callback, failed, attempts);
            }, 100);
        } else {
            if (selector) {
                GlobalHandler(`Unable to bind the "${selector}" ${controlType} due to binding timeout.`, 'debug');
                return Call(failed);
            } else {
                return Call(failed);
            }
        }
    }
}

/**
 * If a dropdown times out when making its ajax call to get its data or just fails for any reason then by adding
 * this click function to the dropdown it will retry to pull its data when clicked. This will either trigger kendo's
 * default ajax read function or you can provide a custom reload function
 * @param {kendoDropDownList} dd The dropdown to reaload
 * @param {any} reloadFunc (Optional) If you need to call a customer function to reload the dropdown
 */
function AutoReloadKDD(dd, reloadFunc) {
    dd.wrapper.on('click.AutoReloadKDD', function () {
        if (dd && dd.dataSource.data().length === 0) {
            GlobalHandler('Reloading kendo dropdown data', 'debug');
            if (IsFunction(reloadFunc)) {
                reloadFunc();
            } else {
                dd.dataSource.read();
            }
        }
    });
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
    return item && typeof (item) === 'object' && 'Init' in item && 'Reset' in item && item.constructor.name.includes('Vm');
}

function IsKoArray(item, ignorePopulated = true) {
    return item && ko.isObservable(item) && 'push' in item && (ignorePopulated || IsNotEmptyArray(item()));
}

/**
 * @description **RECURSIVE** Used to reset all knockout Observables inside of a VM. This will also call the reset function inside of any child VM. This also calls a VM's Init function if it has one. This is to reset variables back to their initial default state.
 * @param {object} vm The knockout VM to be reset.
 * @param {Array} ignoreProperties Array of properties to NOT be reset
 * @returns Promise
 */
function ResetVm(vm, ignoreProperties) {
    return new Promise(function (resolve, reject) {
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

            GlobalHandler(`Reset Vm: ${vm.VmName}`, 'debug');
            resolve();
        } catch (error) {
            reject(error);
        }
    }).catch(function (err) {
        var name = 'VmName' in vm ? ' '+vm.VmName : '';
        throw GlobalHandler(new Error('Resetting Vm' + name + ': ' + err), 'warn');
    });    
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
        throw GlobalHandler(error, 'error');
    }
}

/**
 * @description Generate a list of fields to exclude from serialization
 * @param {Knockout View Model} vm The Vm we want to itterate over and create a black list for
 * @param { Array<string> } vmWhiteList List of Vm names that we want none of the fields included in this black list. The blackList parameter takes precedent over this.
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
        throw GlobalHandler(error, 'error');
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

function formatDate(datetime) {
    return moment(datetime).format('MM-DD-YYYY');
}

/**
 * @description Populate a region kendo dropdown's data based on the country
 * @param {string} selector The selector of the region kendo dropdown
 * @param {string} countryIso The country ISO code
 */
function PopulateRegion(selector, countryIsoCode, defaultValue) {
    WaitForKDDLoaded(selector, async dd => {    
        if (countryIsoCode) {
            var data = await AjaxServices.LocationService.GetRegions(countryIsoCode, dd);

            if (data) {
                dd.dataSource.data(data);
                dd.enable(true);
                if (defaultValue) { dd.value(defaultValue); }
            }
        } else {
            dd.value(' ');
            dd.enable(false);
        }
    });
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
    string = string || '';
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

    self.pause = function () {        
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

    self.startOrContinue = function () {
        if (isNaN(timerSeconds())) { 
            timerSeconds(countDown ? countDown : 0);            
        }
        $(init);
    };       

    self.getTimerSeconds = function () {
        return timerSeconds();
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
	$('[data-toggle="tooltip"]').tooltip({ delay: { 'show': 500, 'hide': 0 } });
}

// Only call a function if it exists
function Call(functionToCall) {
    if (functionToCall && IsFunction(functionToCall)) {
        functionToCall();
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

/**
 * When passed a function with a Promise as it's result this will keep re-executeing until the resulting promise resolves successfully or until the {numOfAttempts} is reached.
 * Make sure the function you are calling is immutable and has no side effects!!!
 * @param {any} PromiseFuncToTry Function to execute that returns a promise
 * @param {any} numOfAttempts How many times to attempt the function
 * @param {any} delayBetweenAttemptsMs Time delay between retry attempts
 */
function RetryPromise(PromiseFuncToTry, numOfAttempts = 4, delayBetweenAttemptsMs = 1000) {
    return new Promise(async function (resolve, reject) {
        if (PromiseFuncToTry && IsFunction(PromiseFuncToTry)) {
            try {
                var result = await PromiseFuncToTry();
                return resolve(result); // We got a successful result so return it.
            } catch (e) { // We failed
                if (numOfAttempts > 0) { // Try again
                    setTimeout(() => {
                        return RetryPromise(PromiseFuncToTry, numOfAttempts - 1, delayBetweenAttemptsMs);
                    }, delayBetweenAttemptsMs);
                } else { // Out of attempts so fail
                    return reject(`All attempts to execute Promise failed. FINAL ERROR: ${e}`);
                }
            }
        } else {
            return reject("The PromiseFuncToTry parameter is not a function")
        }
    });
}


/**
 * @description This Limits how quickly a function can repeat. Be careful on how long you set the rate. 
 * The desired request will on be called when after the "rate" miliseconds after the last request is recieved.
 * !!! Only the last recieved request will be executed !!!
 * @param {function} request The Request to be executed
 * @param {function} requestId unique identifyer of the request being made
 * @param {function} rate How quicly this function is allowed to be called
 */
var TypematicRateRequest = new Array();
function TypematicRate(request, cancelCallback, requestId, rate) {
    if (TypematicRateRequest[requestId]) {
        GlobalHandler(`Preventing request ${requestId}: due to the requests rate limit`, 'debug');
        Call(cancelCallback);
    }
    clearTimeout(TypematicRateRequest[requestId]);
    TypematicRateRequest[requestId] = setTimeout(() => { Call(request); delete TypematicRateRequest[requestId]; }, rate);
}

function WaitForKnockout(knockoutVar, targetValue, callback, failed, attempts = 0) {
    if (knockoutVar() === targetValue) {        
        return Call(callback);
    } else {
        if (attempts < 300) { // Stop trying after 30 seconds
            attempts++;
            setTimeout(function () {
                return WaitForKnockout(knockoutVar, targetValue, callback, failed, attempts);
            }, 100);
        } else {
            GlobalHandler('Variable never became"' + targetValue + '" timing out.', 'debug');
            return Call(failed);
        }
    }
}