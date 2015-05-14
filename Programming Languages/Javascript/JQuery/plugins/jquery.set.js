//Jacob Weigand
//04-17-2015
//Version 1.0
//
(function ($) {
    $.fn.set = function (variables) {
        this.each(function () {
            //determine if we are using passed in variables or data attributes
            var condition = getCondition(variables);
            var target = getTarget(variables);
            var value = getValue(variables);
            var required = getRequired(variables);
            var source = $(this);

            //Determine the html element type or our source element
            switch (source.prop('tagName')) {
                case "OPTION":
                    source.parent().on("change", function () {
                        //Condition is met so set our target
                        if (source.is(condition)) {
                            setTargets(target, value, required);
                        } else {
                            enableTargets(target);
                        }
                    }).trigger("change");
                    break;
                case "INPUT":
                    //Determine the type of input
                    switch (source.attr("type").toLowerCase()) {
                        case "checkbox":
                            source.on("change", function () {
                                //Condition is met so set our target
                                if (source.is(condition)) {
                                    setTargets(target, value, required);
                                } else {
                                    enableTargets(target);
                                }
                            }).trigger("change");
                            break;
                        case "text":
                            source.on("input", function () {
                                //Condition is met so set our target
                                if (source.is(condition)) {
                                    setTargets(target, value, required);
                                } else {
                                    enableTargets(target);
                                }
                            }).trigger("change");
                            break;
                        default:
                            console.error(target.prop('tagName') + " is unknown target input type for the set library");
                    }
                    break;
                default:
                    console.error(source.prop('tagName') + " is unknown source type for the set library");
            }
        });
        return this;
    }

    //-- START -- Static methods
    $.set = {};

    //Call this to set a field
    $.set.manualSet = function (variables) {
        setTargets($(variables.target), variables.value, variables.required);
    }

    //Call this to allow a field
    $.set.manualUnset = function (variables) {
        enableTargets($(variables.target));
    }
    //-- END -- Static methods

    //-- START -- Functions to determine the source of our variables. First as passed in variables, second data attributes on the HTML 
    function getCondition(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.condition !== "undefined" && variables.condition !== null) {
            condition = variables.condition;
        } else if ($(this).attr("data-set-condition") !== "undefined") {
            condition = $(this).attr("data-set-condition");
        }
        return condition;
    }

    function getTarget(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.target !== "undefined" && variables.target !== null) {
            target = $(variables.target);
        } else if ($(this).attr("data-set-target") !== "undefined") {
            target = $($(this).attr("data-set-target"));
        }
        return target;
    }

    function getValue(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.value !== "undefined" && variables.value !== null) {
            value = variables.value;
        } else if ($(this).attr("data-set-value") !== "undefined") {
            value = $(this).attr("data-set-value");
        }
        return value;
    }

    function getRequired(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.required !== "undefined" && variables.required !== null) {
            required = variables.required;
        } else if ($(this).attr("data-set-required") !== "undefined") {
            required = $(this).attr("data-set-required")=="true";
        }
        return required;
    }
    //-- END -- Functions to determine the source of our variables. First as passed in variables, second data attributes on the HTML

    //-- START -- Reusable Functions
    function setTargets(target, value, required) {
        target.each(function () {
            var singleTarget = $(this);
            //Determine the html elment type or our singleTarget element
            switch (singleTarget.prop('tagName')) {
                case "SELECT":
                    target.val(value);
                    break;
                case "INPUT":
                    switch (singleTarget.attr("type").toLowerCase()) {
                        case "checkbox":
                            singleTarget.prop('checked', value);
                            break;
                        case "text":
                            singleTarget.val(value);
                            break;
                    }
                    break;
                case "TEXTAREA":
                    target.val(value);
                    break;
                default:
                    console.error(singleTarget.prop('tagName') + " is an invalid target type for the set library");
            }
            if (required && singleTarget.attr('disabled') != 'disabled') {
                singleTarget.attr("data-disabledBySet", true);
                singleTarget.attr('disabled', 'disabled');
            }
            singleTarget.trigger("change");
        });
    }

    function enableTargets(target) {
        //If something else disabled this element then we should leave it disabled. We only re-enable this element if we are the ones who origionally disabled it.
        if (target.attr("data-disabledBySet") == "true") {
            target.removeAttr("disabled");
            target.removeAttr("data-disabledBySet");
        }
    }

    //-- END -- Reusable Functions
}(jQuery));