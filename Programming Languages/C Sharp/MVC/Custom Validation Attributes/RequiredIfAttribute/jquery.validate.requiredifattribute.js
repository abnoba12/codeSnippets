(function ($) {
    jQuery.validator.addMethod('requiredif', function (value, element, parameters) {
        //This has to grab the elements using a property selector because microsoft is dumb
        //and adds multiple elements to a page with the same ID. Like radio buttons.
        var id = '[id="' + parameters['dependentproperty'] + '"]';

        // get the target value (as a string,
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = (targetvalue == null ? '' : targetvalue).toString();

        // get the actual value of the target control
        // note - this probably needs to cater for more
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue = "";
        if (controltype == 'checkbox') {
            actualvalue = control.attr('checked').toString();
        } else if (controltype == 'radio') {
            //loop through the radio buttons to find the checked one
            control.each(function (index) {
                if ($(this).is(':checked'))
                    actualvalue = $(this).val();
            });
        } else {
            actualvalue = control.val();
        }

        // if the condition is true, reuse the existing
        // required field validator functionality
        if ($.trim(targetvalue.toLowerCase()) == $.trim(actualvalue.toLowerCase()) || ($.trim(targetvalue) == '*' && $.trim(actualvalue) != ''))
            return $.validator.methods.required.call(this, value, element, parameters);
        return true;
    });

    
    jQuery.validator.unobtrusive.adapters.add(
    'requiredif', ["dependentproperty0", "targetvalue0"],
    function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params["dependentproperty0"],
            targetvalue: options.params["targetvalue0"]
        };
        options.messages['requiredif'] = options.message;
    });

} (jQuery));