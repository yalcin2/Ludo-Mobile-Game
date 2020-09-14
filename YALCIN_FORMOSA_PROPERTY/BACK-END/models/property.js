var mongoose = require('mongoose');

var Schema = mongoose.Schema;
var PropertySchema = new Schema({
    propertyType: String,
    location: String,
    price: Number,
    description: String,
    imageUrl: String
});

var PropertyList = module.exports = mongoose.model('Properties', PropertySchema );

module.exports.addProperty = (newProp, callback) => {
	newProp.save(callback);
}

module.exports.deleteById = (id, callback) => {
	let query = {_id: id};
	PropertyList.remove(query, callback);
}