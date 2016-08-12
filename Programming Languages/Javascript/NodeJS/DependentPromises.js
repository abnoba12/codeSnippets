//Steps: 
// 1. Read the Dialer file
// 2. Write the data to the database
// 3. Move the dialer file to the complete folder
// 4. Display message that the process is done.

// Each step of this process returns a Promise and allows the next "then"
// to be executed if the current Promise is resolved successfully.

ingestion.ReadDialer(path).then(value => {
	//This Promise is nested because we are dependent on the data read in from
	//the dialer file returned by the "ReadDialer" Promise.
	return database.writeDialer(value);
}).then(() => {
	//This Promise is not nested becaue it is not dependent on any data. It only
	//requires that "ReadDialer" and "WriteDialer" is complete before execution.
	return ingestion.DialerComplete(path);
}).then(() => {
	//This "then" executes last when all the Promises above successfully resolve.
	console.log("Dialer file processed");
});