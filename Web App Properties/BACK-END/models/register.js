var mongoose = require('mongoose');

var Schema = mongoose.Schema;
var RegisterSchema = new Schema({
    firstName: { type: String, required: true},
    lastName: { type: String, required: true},
    email: { type: String, required: true},
    mobile : { type: Number, required: true},
    catalog : Boolean,
    notification : String
});

var RegistrationList = module.exports = mongoose.model('signUp', RegisterSchema );

module.exports.addUser = (newUser, callback) => {
	newUser.save(callback);
}