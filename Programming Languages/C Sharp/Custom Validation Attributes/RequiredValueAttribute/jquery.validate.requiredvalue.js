(function ($) {
    jQuery.validator.addMethod('requiredvalue', function (value, element, parameters) {
        // get the target value (as a string,
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = (targetvalue == null ? '' : targetvalue).toString();

        
        // if the condition is true, reuse the existing
        // required field validator functionality
        if ($.trim(targetvalue.toLowerCase()) == $.trim(value.toLowerCase()))
            return $.validator.methods.required.call(this, value, element, parameters);
        return true;
    });

    jQuery.validator.unobtrusive.adapters.add(
        'requiredvalue',['targetvalue'],
        function (options) {
            options.rules['requiredvalue'] = {
                targetvalue: options.params['targetvalue']
            };
            options.messages['requiredvalue'] = options.message;
        });
} (jQuery));