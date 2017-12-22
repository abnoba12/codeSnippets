function GenericYesNoModalVm() {
    var self = this;

    self.ContentSelector;
    self.YesCallback; // Required
    self.NoCallback; // Required
    self.CloseCallback; // Optional
    self.DialogTitle; // Optional, requires CloseCallback to be set
    self.PreventClose = false; // Used if validation failed and we need to prevent closing

    //#region Methods
    self.Open = function () {
        if (self.ContentSelector) {
            var dialog = $('#GenericYesNo').data('kendoDialog');
            var content = $(self.ContentSelector);
            dialog.content(content.html());

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
            GlobalWarningHandler(new Error('Content selector not set'));
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

    self.Close = function (e) {
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
}



//Example Usage
// --------------- Open the generic Yes/No Dialog ---------------
VmStore.GenericYesNoModalVm.ContentSelector = "<h1>This is a test of the generic modal system. Normally this would be a jquery selector and not html.</h1>";
VmStore.GenericYesNoModalVm.YesCallback = () => {console.log("YES Callback Called")};
VmStore.GenericYesNoModalVm.NoCallback = () => {console.log("No Callback Called")};
VmStore.GenericYesNoModalVm.CloseCallback = () => {console.log("Closed Callback Called")};
VmStore.GenericYesNoModalVm.DialogTitle = "Testing the generic modal dialog";
VmStore.GenericYesNoModalVm.Open();