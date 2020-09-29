var mongoose = require('mongoose');

var Schema = mongoose.Schema;

// Create a user model
var UserSchema = new Schema({
    name: String,
    surname: String,
    age: Number,
    gender: String,
    mobile: Number
});

var UserList = module.exports = mongoose.model('Users', UserSchema );

// Create a entry within the database
module.exports.addUser = (newUser, callback) => {
	newUser.save(callback);
}

// Delete an existing user with id
module.exports.deleteById = (id, callback) => {
    // Search for the user to delete with the identifier to be removed
    let query = {_id: id};
	UserList.remove(query, callback);
}