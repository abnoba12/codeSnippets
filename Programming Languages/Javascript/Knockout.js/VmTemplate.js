function VmNameVm() {
    var self = this;
    self.Name = self.constructor.name;
	
    //#region Variables & Observables
    self.TabSelector = 'selector of this tabs content';
    self.Validator;
    self.TryToValidate = function () {
        if (self.Validator) {
            //Wait until the tab's content is loaded before we try and validate it
            WaitForLoaded(self.TabSelector, () => { self.Validator.validate(); });
        }
    };
    //Automatically Validate when we have all fields
    ko.computed(self.TryToValidate);
    //#endregion Variables & Observables

    //#region Functions
    //Use this to initialize the default values of any variables.
    //This gets called on creation and on reset
    self.Init = function () { };
    self.Init(); //Call Init on creation

    //This gets called when this VMs content gets loaded into the dom for the first time.
    self.Loaded = function () {
        //We use kendo's form validator
        self.Validator = KendoValidator(self.TabSelector, self.Valid, self.Invalid);
        self.LoadKeyboardControls();
        self.LoadInputMasks();
    };
    self.Valid = function () { }; // Valid Callback
    self.Invalid = function () { }; // Invalid Callback
	
	//This gets call every time this VMs tab gets selected. Not just the first time it is loaded like "Loaded".
	self.Selected = function(){
		WaitForLoaded(self.TabSelector, () => { 
            self.TryToValidate();
            //Set the focus to the first text input of the tab for users who use the keyboard
			$('#textBox:text').get(0).focus();
        });
	}

    self.LoadKeyboardControls = function () {
        //If there are any keyboard controls attach them here
    };

    self.LoadInputMasks = function () {
        //Attach your input mask to your dom elements if you have any
    };

    //This will reset all knockout observables in this VM.
    //This will also call the reset function in any child VMs.
    self.Reset = function () {
        ResetVm(self);
    };
    //#endregion Functions

    //#region Subscribers & Computeds
    //#endregion Subscribers
}
