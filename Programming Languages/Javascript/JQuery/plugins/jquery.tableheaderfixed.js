//Jacob Weigand
//05-01-2015
//Version: 1
//Documentation:

(function ($) {    
    $.fn.tableheaderfixed = function (variables) {
        if (variables == "Destroy") {
            $("#header-fixed").remove();
            $(window).unbind('.tableheaderfixedScroll');
        } else {

            //Dummy table for the header
            var dummyTable = "<table id=\"header-fixed\" style=\"display: none;\"></table>"
            $(dummyTable).insertAfter(this);

            //determine if this table use thead or not
            var tableHead;
            if (typeof this.find("> thead") == "undefined") {
                tableHead = this.find("th:first").parent();
            } else {
                tableHead = this.find("> thead");
            }

            var table = this;
            var tableOffset = this.offset().top;
            var header = tableHead.clone();
            var fixedHeader = $("#header-fixed").append(header);
            
            $(window).bind("scroll.tableheaderfixedScroll", function () {
                var offset = $(this).scrollTop();

                if (offset >= tableOffset && fixedHeader.is(":hidden")) {
                    matchCSS(tableHead, fixedHeader)
                    fixedHeader.show();
                }
                else if (offset < tableOffset) {
                    fixedHeader.hide();
                }
            });
        }

        return this;
    }

    function matchCSS(header, fixedHeader) {
        copyCSS($(header), $(fixedHeader));
        $(fixedHeader).css("position", "fixed");
        $(fixedHeader).css("top", "0");
        $(fixedHeader).css("display", "none");

        var headerTH = $(header).find("th");
        var fixedHeaderTH = $(fixedHeader).find("th");
        for (i = 0; i < fixedHeaderTH.length; i++) {
            copyCSS($(headerTH[i]), $(fixedHeaderTH[i]));
        }
    }

    function copyCSS(source, destination) {
        var dom = $(source).get(0);
        var dest = {};
        var style, prop;
        if (window.getComputedStyle) {
            var camelize = function (a, b) {
                return b.toUpperCase();
            };
            if (style = window.getComputedStyle(dom, null)) {
                var camel, val;
                if (style.length) {
                    for (var i = 0, l = style.length; i < l; i++) {
                        prop = style[i];
                        camel = prop.replace(/\-([a-z])/, camelize);
                        val = style.getPropertyValue(prop);
                        dest[camel] = val;
                    }
                } else {
                    for (prop in style) {
                        camel = prop.replace(/\-([a-z])/, camelize);
                        val = style.getPropertyValue(prop) || style[prop];
                        dest[camel] = val;
                    }
                }
                return destination.css(dest);
            }
        }
        if (style = dom.currentStyle) {
            for (prop in style) {
                dest[prop] = style[prop];
            }
            return destination.css(dest);
        }
        if (style = dom.style) {
            for (prop in style) {
                if (typeof style[prop] != 'function') {
                    dest[prop] = style[prop];
                }
            }
        }
        return destination.css(dest);
    };
}(jQuery));