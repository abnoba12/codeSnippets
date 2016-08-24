export class CampaignProperties{
    public constructor(){
        //This will run the custom rules on "CallWindows". 
		this.schema.path('callWindows').validate(callWindows => {
            if(!callWindows){return false}
            else if(callWindows.length === 0){return false}
            return true;
        });
    }

    public schema:Schema = new Schema({
        campaignName: { type: String, required: [true, '{PATH} is required' ]},
        callerIdPhoneNumber: { type: String, required: [true, '{PATH} is required' ]},
        numberOfCallAttempts: { type: Number, required: [true, '{PATH} is required' ]},
        callWindows: [CallWindow.schema],
        HospitalName: { type: String, required: false },
        RecordingFrequency:{ type: String, required: false },
        Holidays: [Date],
        ScheduleType: { type: String,  enum:["asap", "random"] ,required: false }
    });
}