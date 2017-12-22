//Add validation rules by adding data-RuleName-msg="Error message" to your html inputs
function KendoValidator(selector, validCallback, invalidCallback) {
    var getType = {};
    var domElement = $(selector);
    if (domElement.length) {
        try {
            return domElement.kendoValidator({
                validatedfistTime: false,
                validate: function (e) {
                    //Everything is filled out and valid 
                    HandleInputValidationStyling(e, e.valid);
                    this.options.validatedfistTime = true;

                    if (e.valid && IsFunction(validCallback)) {
                        validCallback();
                    } else if (IsFunction(invalidCallback)) {
                        invalidCallback();
                    }
                },
                validateInput: function (e) {
                    HandleInputValidationStyling(e, e.valid);
                    //If a field becomes invalid after we completed the welcome tab then disable the tabs again
                    if (!e.valid && IsFunction(invalidCallback)) {
                        invalidCallback();
                    }
                    //After we have completed the form the first time, validate all fields every time we change any field
                    if (this.options.validatedfistTime) {
                        this.validate();
                    }
                },
                rules: {
                    isValidDate: kendoValidatorCustomRules.isValidDate,
                    isAlpha: kendoValidatorCustomRules.isAlpha,
                    isName: kendoValidatorCustomRules.isName,
                    isTag: kendoValidatorCustomRules.isTag,
                    isZip: kendoValidatorCustomRules.isZip,
                    isEmail: kendoValidatorCustomRules.isEmail,
                    isSSN: kendoValidatorCustomRules.isSSN,
                }
            }).data('kendoValidator');
        } catch (error) {
            GlobalWarningHandler(error);
        }
    } else {
        GlobalWarningHandler(new Error('kendoValidator: Unable to find element "' + selector + '" for binding'));
        return undefined;
    }
}

function HandleInputValidationStyling(e, isValid) {
    var validationStyle = "custom-invalid";
    if (e.input) {

        var elementToStyle
        var elementsToStyle = [];

        //handle textbox styling
        elementToStyle = e.input.hasClass('k-textbox')
        if (elementToStyle) {
            elementsToStyle.push(e.input.siblings('label'));
        }

        //handle dropdown styling
        elementToStyle = $(e.input).siblings('.k-dropdown-wrap')
        if (elementToStyle.length) {
            elementsToStyle.push(elementToStyle.parent().siblings('label'));
            elementsToStyle.push(elementToStyle);
        }

        //handle datepicker styling
        elementToStyle = $(e.input).parent('.k-picker-wrap')
        if (elementToStyle.length) {
            elementsToStyle.push(elementToStyle);
            //style the datepickers internal border
            elementsToStyle.push(e.input.siblings('.k-select'));
            //style the label
            elementsToStyle.push(elementToStyle.parent().siblings('label'));
        }

        if (isValid) {
            elementsToStyle.forEach(function (anElement) {
                anElement.removeClass(validationStyle);
            });
        }
        else {
            elementsToStyle.forEach(function (anElement) {
                anElement.addClass(validationStyle);
            });
        }
    }
}

var kendoValidatorCustomRules = {

    isValidDate: function (input) {
        if (input.is('[data-isValidDate-msg]') && input.val() != '') {
            return kendo.parseDate(input.val());
        }
        return true;
    },

    isAlpha: function (input) {
        if (input.is('[data-isAlpha-msg]') && input.val() != '') {
            var re = new RegExp(/^[a-zA-Z]*$/);
            return re.test(input.val());
        }
        return true;
    },

    isName: function (input) {
        if (input.is('[data-isName-msg]') && input.val() != '') {
            //Allow Letters, white space, and hyphen
            var re = new RegExp(/^[a-zA-Z\s-]*$/);
            return re.test(input.val());
        }
        return true;
    },

    isTag: function (input) {
        if (input.is('[data-isTag-msg]') && input.val() != '') {
            var re;
            switch (VmStore.GlobalCustomerVm.GetDefaultCountry()) {
                case 'CA':
                    // Canada hasn't been done
                    return true;
                    break;
                default:
                    //Allow Letters, digits, white space, and hyphen
                    re = new RegExp(/^[A-Z\d\s-]{4,8}$/);
            }
            return re.test(input.val());
        }
        return true;
    },

    isSSN: function (input) {
        if (input.is('[data-isSSN-msg]') && input.val() != '') {
            var re;
            switch (VmStore.GlobalCustomerVm.GetDefaultCountry()) {
                case 'CA':
                    // Canada hasn't been done
                    return true;
                    break;
                default:
                    //Allow Letters, digits, white space, and hyphen
                    re = new RegExp(/^\d{3}-\d{2}-\d{4}$/);
            }
            return re.test(input.val());
        }
        return true;
    },

    isVIN: function (input) {
        if (input.is('[data-isSSN-msg]') && input.val() != '') {
            var re;
            switch (VmStore.GlobalCustomerVm.GetDefaultCountry()) {
                case 'CA':
                    // Canada hasn't been done
                    return true;
                    break;
                default:
                    //Allow Letters, digits, white space, and hyphen
                    re = new RegExp(/^[A-Z\d]{11,17}$/);
            }
            return re.test(input.val());
        }
        return true;
    },

    isZip: function (input) {
        if (input.is('[data-isZip-msg]') && input.val() != '') {
            return IsValidPostalCode(input.val());
        }
        return true;
    },

    isEmail: function (input) {
        if (input.is('[data-isEmail-msg]') && input.val() != '') {
            var re = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
            return re.test(input.val());
        }
        return true;
    },
};

//#region validation methods
//Returns true if the variable passed in is a function. Used for callbacks
function IsFunction(f) {
    var getType = {};
    return f && getType.toString.call(f) === '[object Function]';
}

//Returns true if the GUID is valid or empty
function IsValidGuid(guid) {
    var re = new RegExp(/^[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$/);
    return !guid || (guid !== '00000000-0000-0000-0000-000000000000' && re.test(guid.toUpperCase()));
}

function IsValidPostalCode(postalCode) {
    var re;
    switch (VmStore.GlobalCustomerVm.GetDefaultCountry()) {
        case 'CA':
            re = new RegExp(/^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$/);
            break;
        default:
            re = new RegExp(/^\d{5}(?:[-\s]\d{4})?$/);
    }

    return re.test(postalCode);
}

//#endregion validation methods