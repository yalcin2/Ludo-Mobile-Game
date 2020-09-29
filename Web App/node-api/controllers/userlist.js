var express = require('express');
var router = express.Router();
var userlist = require('../models/user.js');

// REST API

// Retrieve all existing values from the database
router.get('/',(req,res) => {
	userlist.find((err, users)=> {
		if(err) 
			res.json({success:false, message: `Failed to load all users. Error: ${err}`});
		else 
			res.write(JSON.stringify({success: true, users:users},null,2));
			res.end();	
	});
});

// Add a new entry to the database with the values
router.post('/', (req,res,next) => {
	console.log(req.body);
	let newUser = new userlist({
		name: req.body.name,
        surname: req.body.surname,
        age: req.body.age,
        gender: req.body.gender,
        mobile: req.body.mobile
	});
	userlist.addUser(newUser,(err, users) => {
		if(err) {
			res.json({success: false, message: `Failed to create a new user. Error: ${err}`});
		}
		else 
			res.json({success:true, message: "Added successfully."});
	});
});

// Delete an existing database entry with its unique identifier
router.delete('/:id', (req,res,next)=> {
	let id = req.params.id;
	console.log(id); // testing
	userlist.deleteById(id,(err,users) => {
		if(err) {
			res.json({success:false, message: `Failed to delete the user. Error: ${err}`});
		}
		else if(users) {
			res.json({success:true, message: "Deleted successfully"});
		}
		else
			res.json({success:false});
	})
});

// Update an existing database entry with its unique identifier
router.put('/:id', function(req, res, next) {
	userlist.findByIdAndUpdate(req.params.id, req.body, function (err, post) {
	  if (err) return next(err);
	  res.json(post);
	});
  });

module.exports = router;