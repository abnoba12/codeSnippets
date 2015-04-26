//Jacob Weigand
//04-13-2015
//Version: 1.1
//Documentation: http://jhwiki.jhilburn.com/~jhilburn/index.php?title=Disallow_JQuery_Plugin
//
//TODO: Add a hoverover to all the disallowed targets telling the user why the target is disabled. 
//To get nice english wording you can find the target's label element and pull the text. If the form field has a label.
//
(function ($) {    
    $.fn.disallow = function (variables) {
        this.each(function (index) {
            //determine if we are using passed in variables or data attributes
            var condition = getCondition(variables);
            var target = getTarget(variables);
            var hide = getHide(variables);
            var source = $(this);

            //Determine the html element type or our source element
            switch (source.prop('tagName')) {
                case "OPTION":
                    source.parent().on("change", function () {
                        //Condition is met so disallow our target
                        if (source.is(condition)) {
                            disallowTargets(source, target, hide);
                        } else {
                            allowTargets(source, target);
                        }
                    }).trigger("change");
                    break;
                case "INPUT":
                    //Determine the type of input
                    switch (source.attr("type").toLowerCase()) {
                        case "checkbox":
                            source.on("change", function () {
                                //Condition is met so disallow our target
                                if (source.is(condition)) {
                                    disallowTargets(source, target, hide);
                                } else {
                                    allowTargets(source, target);
                                }
                            }).trigger("change");
                            break;
                        case "text":
                            source.on("input", function () {
                                //Condition is met so disallow our target
                                if (source.is(condition)) {
                                    disallowTargets(source, target, hide);
                                } else {
                                    allowTargets(source, target);
                                }
                            }).trigger("change");
                            break;
                        default:
                            console.error(target.prop('tagName') + " is unknown target input type for the disallow library");
                    }
                    break;
                default:
                    console.error(source.prop('tagName') + " is unknown source type for the disallow library");
            }
        });
        return this;

    };

    //-- START -- Static methods
    $.disallow = {};

    //Call this to disallow a field
    $.disallow.manualDisallow = function (variables){
        var source = $("<input type=\"text\" name=\"" + variables.disallowName + "\"></input>");
        variables.hide = typeof variables.hide !== "undefined" ? variables.hide : true;
        disallowTargets(source, $(variables.target), variables.hide);
    }

    //Call this to allow a field
    $.disallow.manualAllow = function (variables) {
        var source = $("<input type=\"text\" name=\"" + variables.disallowName + "\"></input>");
        allowTargets(source, $(variables.target));
    }
    //-- END -- Static methods

    //-- START -- reusable functions
    //Enable targets
    function allowTargets(source, target) {
        var sourceName = getSourceName(source);
        target.each(function () {
            var singleTarget = $(this);
            var currentDisallows = singleTarget.attr("data-disallow-from");
            var currentDisallowsArray = Array();

            //Check to see if this source element disallowed this singleTarget element
            if (typeof currentDisallows !== typeof undefined && currentDisallows !== false) {
                currentDisallowsArray = currentDisallows.split(',');
                //The source did disallow this singleTarget
                if ($.inArray(sourceName, currentDisallowsArray) !== -1) {
                    removeDisallowLabel(source, singleTarget);
                    if (!hasDisallows(singleTarget)) {
                        singleTarget.removeAttr('disabled');
                        singleTarget.show();
                    }
                    singleTarget.trigger("change");
                }
            }
        });
    }
    
    //Disallow targets
    function disallowTargets(source, target, hide) {
        target.each(function () {
            var singleTarget = $(this);
            //Determine the html elment type or our singleTarget element
            switch (singleTarget.prop('tagName')) {
                case "SELECT":
                    disableSelect(singleTarget);
                    break;
                case "OPTION":
                    disableSelectOption(singleTarget);
                    break;
                case "INPUT":
                    disableInput(singleTarget);
                    break;
                default:
                    console.error(singleTarget.prop('tagName') + " is unknown singleTarget type for the disallow library");
            }
            if (hide) {
                singleTarget.hide();
            }
            addDisallowLabel(source, singleTarget);
            singleTarget.trigger("change");
        });
    }

    //Disable entire select list
    function disableSelect(element) {
        //set the select list's value to empty if possible
        element.val("");
        element.attr('disabled', 'disabled');
    }
    
    //Disable a select list option
    function disableSelectOption(selectOption) {
        //If our target was currently selected when the condition was met, move the selection to the first in the dropdown
        if (selectOption.is(":selected")) {
            selectOption.attr('selected', false).attr('disabled', 'disabled');
            selectOption.parent().find("option").first().attr('selected', true).trigger("click").trigger("change");
        } else {
            selectOption.attr('disabled', 'disabled');
        }
    }

    //Disable input
    function disableInput(inputElement) {
        inputElement.attr("disabled", true);
        //Determine the input type or our inputElement element
        //then remove any user entered data
        switch (inputElement.attr("type").toLowerCase()) {
            case "checkbox":
                inputElement.prop('checked', false);
                break;
            case "text":
                inputElement.val("");
                break;
        }
    }

    //Add a data attribute to list What elements are causing this element to me disallowed
    function addDisallowLabel(source, target) {
        var currentDisallows = target.attr("data-disallow-from");
        var currentDisallowsArray = Array();
        var sourceName = getSourceName(source);

        if (typeof currentDisallows !== typeof undefined && currentDisallows !== false) {
            currentDisallowsArray = currentDisallows.split(',');
            currentDisallowsArray.push(sourceName);
        } else {
            currentDisallowsArray.push(sourceName);
        }
        target.attr("data-disallow-from", currentDisallowsArray.join(","));
    }

    //Removed the source element from the list of data elements causing an target element from being disallowed 
    function removeDisallowLabel(source, target) {
        var currentDisallows = target.attr("data-disallow-from");
        var currentDisallowsArray = Array();
        var sourceName = getSourceName(source);
        
        if (typeof currentDisallows !== typeof undefined && currentDisallows !== false) {
            currentDisallowsArray = currentDisallows.split(',');
            currentDisallowsArray = jQuery.grep(currentDisallowsArray, function (i) {
                return (i !== sourceName);
            });
            currentDisallows = currentDisallowsArray.join(",");
            if (currentDisallows == "") {
                target.removeAttr("data-disallow-from");
            } else {
                target.attr("data-disallow-from", currentDisallows);
            }
        }
    }

    function getSourceName(source) {
        var sourceName = source.attr("name");

        if (typeof sourceName == typeof undefined || sourceName == false) {
            sourceName = source.parent().attr("name") + "-" + source.attr("value");
        }
        return sourceName;
    }

    function hasDisallows(element) {
        var currentDisallows = element.attr("data-disallow-from");
        if (typeof currentDisallows !== typeof undefined && currentDisallows !== false && currentDisallows != "") {
            return true;
        }
        return false;
    }
    //-- START -- reusable functions

    //-- START -- Functions to determine the source of our variables. First as passed in variables, second data attributes on the HTML 
    function getCondition(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.condition !== "undefined" && variables.condition !== null) {
            condition = variables.condition;
        } else if ($(this).attr("data-disallow-condition") !== "undefined") {
            condition = $(this).attr("data-disallow-condition");
        }
        return condition;
    }

    function getTarget(variables) {
        if (typeof variables !== "undefined" && variables !== null && typeof variables.target !== "undefined" && variables.target !== null) {
            target = $(variables.target);
        } else if ($(this).attr("data-disallow-target") !== "undefined") {
            target = $($(this).attr("data-disallow-target"));
        }
        return target;
    }

    function getHide(variables) {
        if ((typeof variables !== "undefined" && variables !== null && variables.hide == true) ||
                (typeof $(this).attr("data-disallow-hide") !== "undefined" && $(this).attr("data-disallow-hide") !== null && $(this).attr("data-disallow-hide") == "true") ||
                ((typeof variables.hide == "undefined" || variables.hide == null) && (typeof $(this).attr("data-disallow-hide") == "undefined" || $(this).attr("data-disallow-hide") == null))) {
            hide = true;
        } else {
            hide = false;
        }
        return hide;
    }
    //-- END -- Functions to determine the source of our variables. First as passed in variables, second data attributes on the HTML

}(jQuery));