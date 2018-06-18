function GenericYesNoModalVm() {
    var self = this;

    self.ContentSelector;
    self.YesCallback; // Required
    self.NoCallback; // Required
    self.CloseCallback; // Optional
    self.DialogTitle; // Optional, requires CloseCallback to be set
    self.PreventClose = false; // Used if validation failed and we need to prevent closing
    self.DontShowAgain = ko.observable(false);

    //#region Methods
    self.Open = function () {
        if (Cookies.get('DontShowModalAgain' + self.ContentSelector) === 'true') {
            self.Yes();
            self.Close();
        } else {
            if (self.ContentSelector) {
                self.DontShowAgain(false);
                var dialog = $('#GenericYesNo').data('kendoDialog');
                var content = $(self.ContentSelector);
                dialog.content(content.html());
                $(dialog.wrapper).find('#GenericYesNo').children().each(function () {
                    ko.applyBindings(VmStore, $(this)[0]);
                });                

                // Pull the title from the data-modal-title attribute if we don't have it passed in
                if (!self.DialogTitle && content.attr('data-modal-title')) {
                    self.DialogTitle = content.attr('data-modal-title');
                }

                if (self.DialogTitle) {
                    $('#GenericYesNo[data-role="dialog"]').parent().find('.k-window-titlebar span.k-window-title.k-dialog-title').text(self.DialogTitle);
                }

                // Only Show the titlebar if we have a close action for the X
                if (IsFunction(self.CloseCallback)) {
                    dialog.wrapper.find('.k-dialog-titlebar').show();
                } else {
                    dialog.wrapper.find('.k-dialog-titlebar').hide();
                }

                // Only Show the Yes/No buttons if have have yes and no actions
                if (IsFunction(self.YesCallback) && IsFunction(self.NoCallback)) {
                    dialog.wrapper.find('.k-button-group').show();
                } else {
                    dialog.wrapper.find('.k-button-group').hide();
                }
                dialog.open();
            } else {
                GlobalHandler(new Error('Content selector not set'), 'warn');
            }
        }
    };

    self.Yes = function () {
        if (IsFunction(self.YesCallback)) {
            self.YesCallback();
        }
    };

    self.No = function () {
        if (IsFunction(self.NoCallback)) {
            self.NoCallback();
        }       
    };

    self.Close = function () {
        dialog = $('#GenericYesNo').data('kendoDialog');
        if (dialog) {
            dialog.close();
        }
    };

    self.OnClose = function (e) {
        if (self.PreventClose) {
            //We prevent the dialog from closing if our note doesn't pass validation
            e.preventDefault();
            self.PreventClose = false;
        } else {
            if (IsFunction(self.CloseCallback)) {
                self.CloseCallback();
            }

            //The kendo modal is greedy and moves the content into itself. This puts it back so it can be used again later.
            $('#GenericYesNo > div').appendTo(self.ContentSelector);
            self.Reset();
        }
    };

    self.Reset = function () {
        self.ContentSelector = undefined;
        self.YesCallback = undefined;
        self.NoCallback = undefined;
        self.CloseCallback = undefined;
        self.DialogTitle = undefined;
        self.PreventClose = false;
    };
    //#endregion Methods

    self.DontShowAgain.subscribe(function (newValue) {
        if (self.ContentSelector) {
            Cookies.set('DontShowModalAgain' + self.ContentSelector, newValue, { expires: 0.5 });
        }
    });
}