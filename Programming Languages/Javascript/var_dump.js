//recursively dump this object

var o = "variable name"; 

function recusiveDump(obj) {
    for(var i in obj) {
        if(obj.hasOwnProperty(i)) {
            if(typeof obj[i] == 'object') {
                recusiveDump(obj[i]);
            } else {
                console.log(i + " = " + obj[i]);
            }
        }
    }
    return;
}
recusiveDump(o);