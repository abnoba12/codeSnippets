function VmNameVm() {
    var self = this;

    //#region Variables & Observables
    self.Validator;
    //#endregion Variables & Observables

    //#region Methods
    //Use this to initialize the default values of any variables.
    //This gets called on creation and on reset
    self.Init = function () { };
    self.Init(); //Call Init on creation

    //This gets called when this Vms content gets loaded into the dom
    self.Loaded = function () {
        //We use kendo's form validator
        self.Validator = KendoValidator('selector', self.Valid, self.Invalid);
    };
    self.Valid = function () { }; // Valid Callback
    self.Invalid = function () { }; // Invalid Callback

    //This will reset all knockout observerables in this VM.
    //This will also call the reset function in any child VMs.
    self.Reset = function () {
        ResetVm(self);
    };
    //#endregion Methods

    //#region Subscribers
    //#endregion Subscribers
}
