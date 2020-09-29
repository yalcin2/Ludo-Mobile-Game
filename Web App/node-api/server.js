// Node module imports
var express = require('express');
var mongoose = require('mongoose');
var bodyParser = require('body-parser');
var cors = require('cors');
var app = express();

// REST Api import
var userlist = require('./controllers/userlist.js');

// Use the imported node modules
app.use(cors());
app.use(bodyParser.json());

// Connect to the database using the database url and admin login
mongoose.connect('mongodb+srv://user1:123@besedowebapp.0pgl2.mongodb.net/BesedoWebApp?retryWrites=true&w=majority',{ useNewUrlParser: true } , err=>{
    if(!err){
        console.log('connected to mongodb');
    }
});

app.get('/', (req,res) => {
    res.send("Web App /users");
})

// Attach the retrieved JSON data to the '/users' endpoint
app.use('/users',userlist);

// Connect to port 3000
app.listen(3000)


