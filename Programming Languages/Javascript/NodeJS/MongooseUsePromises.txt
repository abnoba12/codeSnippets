//Force Mongoose to use javascript promises and not mongoose promises
var mongoose = require('mongoose');
mongoose.Promise = global.Promise;