var express = require('express');
var router = express.Router();
var propertylist = require('../models/property.js');

router.get('/',(req,res) => {
	propertylist.find((err, props)=> {
		if(err) 
			res.json({success:false, message: `Failed to load all properties. Error: ${err}`});
		else 
			res.write(JSON.stringify({success: true, props:props},null,2));
			res.end();	
	});
});

router.post('/', (req,res,next) => {
	console.log(req.body);
	let newProp = new propertylist({
		propertyType: req.body.propertyType,
        location: req.body.location,
        price: req.body.price,
        description: req.body.description,
        imageUrl: req.body.imageUrl
	});
	propertylist.addProperty(newProp,(err, props) => {
		if(err) {
			res.json({success: false, message: `Failed to create a new property. Error: ${err}`});
		}
		else 
			res.json({success:true, message: "Added successfully."});
	});
});

router.delete('/:id', (req,res,next)=> {
	let id = req.params.id;
	console.log(id);
	propertylist.deleteById(id,(err,props) => {
		if(err) {
			res.json({success:false, message: `Failed to delete the property. Error: ${err}`});
		}
		else if(props) {
			res.json({success:true, message: "Deleted successfully"});
		}
		else
			res.json({success:false});
	})
});

router.put('/:id', function(req, res, next) {
	propertylist.findByIdAndUpdate(req.params.id, req.body, function (err, post) {
	  if (err) return next(err);
	  res.json(post);
	});
  });

/*
app.put('/:id', function(req, res) {
	propertylist.update({_id: req.params.propertylist}, req.body, function(err, props) {
	  if (err)
		  res.send(err);

	  // get and return all the feedbacks after you edit one
	  getfeedbacks(res);
  });
});
*/

module.exports = router;