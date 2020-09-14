var express = require('express');
var router = express.Router();
var registrationlist = require('../models/register.js');

router.get('/',(req,res) => {
	registrationlist.find((err, users)=> {
		if(err) 
			res.json({success:false, message: `Failed to load all users. Error: ${err}`});
		else 
			res.write(JSON.stringify({success: true, users:users},null,2));
			res.end();	
	});
});

router.post('/', (req,res,next) => {
	console.log(req.body);
	let newUser = new registrationlist({
		firstName: req.body.firstName,
        lastName: req.body.lastName,
        email: req.body.email,
        mobile: req.body.mobile,
        catalog: req.body.catalog,
        notification: req.body.notification
	});
	registrationlist.addUser(newUser,(err, users) => {
		if(err) {
			res.json({success: false, message: `Failed to create a new user. Error: ${err}`});
		}
		else 
			res.json({success:true, message: "Added successfully."});
	});
});

module.exports = router;