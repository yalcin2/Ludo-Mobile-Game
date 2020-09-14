var express = require('express');
var mongoose = require('mongoose');
var bodyParser = require('body-parser');
var cors = require('cors');
var app = express();
var propertylist = require('./controllers/propertylist');
var registrationlist = require('./controllers/registrationlist');

app.use(cors());

app.use(bodyParser.json());

mongoose.connect('mongodb://admin:123@ds131997.mlab.com:31997/assignment_database', {useMongoClient:true}, err=>{
    if(!err){
        console.log('connected to mongodb');
    }
});

app.get('/', (req,res) => {
    res.send("Client Side Scripting /propertylist /registrationlist");
})

app.use('/propertylist',propertylist);

app.use('/registrationlist',registrationlist);

app.listen(3000)