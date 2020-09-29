var app = new function() {

    var table = document.getElementById('profiles');

    // Declaring local arrays
    var ids = [];
    var names = [];
    var surnames = [];
    var ages = [];
    var genders = [];
    var mobiles = [];

    // Count the total number of user profiles
    var Count = function(data) {
        var counter   = document.getElementById('counter');
        var id = 'row';

        // Check if the data is not empty 
        // Also check if there is one row, no rows or multiple rows to display different text
        if (data) {
            if (data > 1) {
                id = 'rows'; 
            }
            counter.innerHTML = data + ' ' + id ;
            } else {
            counter.innerHTML = 'No rows';
        }
    };

    this.GetUserProfiles = function(){
        //Retrieve the user profiles
        GetAllData();

        // Retrieve a string from the URL and remove the leading '?'
        const queryString = window.location.search.substr(1);;
        
        // If the query is not empty or null, execute the edit function
        // This will open the same page state as before
        if(queryString != null && queryString != ""){

            queryArray = queryString.split("%20")
            
            // Check that there are a total of 6 values within the string value
            if(queryArray.length == 6){
                // Set the boolean parameter to true, as to indicate that this was retrieved from a URL
                this.Edit(queryString,true);
            }
        }
    }

    // Retrieve all the data from the array lists to be displayed as a list
    var GetAllData = function() {
        // Reset the variables, to be re-filled
        var data = '';
        ids = [];
        names = [];
        surnames = [];
        ages = [];
        genders = [];
        mobiles = [];

        // Post request to send the values
        var xhr = new XMLHttpRequest();
        xhr.onload = function() {

            var myArr = JSON.parse(this.responseText);
            if (myArr.users.length > 0) {
                for (i = 0; i < myArr.users.length ; i++) {
                    // Check if any of the values are null or empty
                    if(myArr.users[i]._id != null && myArr.users[i]._id != "" && 
                        myArr.users[i].name != null && myArr.users[i].name != "" && 
                        myArr.users[i].surname != null && myArr.users[i].surname != "" && 
                        myArr.users[i].age != null && myArr.users[i].age != "" &&  
                        myArr.users[i].gender != null && myArr.users[i].gender != "" && 
                        myArr.users[i].mobile != null && myArr.users[i].mobile != "")
                        
                        // Add the retrieved values to the appropriate arrays
                        ids.push(myArr.users[i]._id);
                        names.push(myArr.users[i].name);
                        surnames.push(myArr.users[i].surname);
                        ages.push(myArr.users[i].age);
                        genders.push(myArr.users[i].gender);
                        mobiles.push(myArr.users[i].mobile);
                }
            }

            // First check if 'ids' array has values inside it
            // After loop for the total size of the array
            // Each iteration adds 5 seperate data cells filled with values
            if (ids.length > 0) {
                for (i = 0; i < ids.length; i++) {
                    data += '<tr class="item">' +
                    '<td>' + names[i] + '</td>' +
                    '<td>' + surnames[i] + '</td>' +
                    '<td>' + ages[i] + '</td>' +
                    '<td>' + genders[i] + '</td>' +
                    '<td>' + mobiles[i] + '</td>' +
                    '<td><a id="myBtnModal" data-name="?'+ids[i]+' '+names[i]+' '+surnames[i]+' '+ages[i] +' '+genders[i]+' '+mobiles[i]+'" onclick="app.Edit(' + i + ', false)">Edit</a></td>' + 
                    '<td><a onclick="app.Delete(' + i + ')">Delete</a></td>' +
                    '</tr>';
                }
            }        

            // Count the total number of rows with the user identifiers
            Count(ids.length);

            // Attach the data cells to the table
            return table.innerHTML = data;    
        };

        var url = "http://localhost:3000/users";
        xhr.open("GET", url, true);
        xhr.send();             
    };

    // Create a new user profile by sending an html post request with the inputted field values 
    this.Add = function () {
        addName = document.getElementById('add-name');
        addSurname = document.getElementById('add-surname');
        addAge = document.getElementById('add-age');
        addGender = document.getElementById('add-gender');
        addMobile = document.getElementById('add-mobile');

        // Get the values from the input fields
        var name = addName.value;
        var surname = addSurname.value;
        var age = addAge.value;
        var gender = addGender.value;
        var mobile = addMobile.value;

        // Check if edited values are empty, null or contain any special characters
        var format = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
        if(name == "" || format.test(name) ||
            surname == "" || format.test(surname) ||
            age == null || age == "" || 
            gender == "" || format.test(gender) ||
            mobile == null || mobile == ""){
            alert("Error missing values and/or special characters detected!");
        }
        else{
            // Confirmation window
            var r = confirm("Are you sure you want to create this profile?");
            if (r == true) { 
                // Add the new values to the array lists
                names.push(name.trim());
                surnames.push(surname.trim());
                ages.push(age);
                genders.push(gender.trim());
                mobiles.push(mobile);

                // Reset input fields values to default
                addName.value = '';
                addSurname.value = '';
                addAge.value = '';
                addMobile.value = '';

                // Post request to send the values
                var xhr = new XMLHttpRequest();
                // Display the updated data after the request has loaded
                xhr.onload = function() {
                    GetAllData();
                }
                var url = "http://localhost:3000/users";
                xhr.open("POST", url, true);
                xhr.setRequestHeader("Content-Type", "application/json");
                // Convert values to JSON
                var data = JSON.stringify({"name": name, 
                                        "surname": surname,
                                        "age":age,
                                        "gender":gender,
                                        "mobile":mobile});
                // Initiate the post request
                xhr.send(data);

            }
        }
    };

    // This function updates an existing user profile by sending an html put request with the updated input values 
    // The function retrieves the chosen profile values and displays them within the edit input field
    // The second parameter is used to check if their are multiple values, indicating that they were retrieved from the URL 
    this.Edit = function (item, hasItems) {

        // Get edit input fields
        editName = document.getElementById('edit-name');
        editSurname = document.getElementById('edit-surname');
        editAge = document.getElementById('edit-age');
        editGender = document.getElementById('edit-gender');
        editMobile = document.getElementById('edit-mobile');

        // Check if the url has string values prepared
        if(hasItems){
            // Split the values into an arraylist
            itemsArray = item.split("%20")

            // Convert the values that should be numbers
            itemsArray[3] = parseInt(itemsArray[3]); // Age
            itemsArray[5] = parseInt(itemsArray[5]); // Mobile

            // Place the values within the input fields
            editName.value = itemsArray[1];
            editSurname.value = itemsArray[2];
            editAge.value = itemsArray[3];
            editGender.value = itemsArray[4];
            editMobile.value = itemsArray[5];
        }
        else{    
            editName.value = names[item];
            editSurname.value = surnames[item];
            editAge.value = ages[item];
            editGender.value = genders[item];
            editMobile.value = mobiles[item];
        }

        // Get the modal
        var modal = document.getElementById("myModal");
        // Show the modal and animate it
        modal.classList.add("showModal");
        modal.classList.remove("hideModal");
        modal.style.display = "block";

        // When the update button is clicked, this function will be called
        document.getElementById('saveEdit').onsubmit = function() {
            // Get value from the edit input fields
            var eName = editName.value;
            var eSurname = editSurname.value;
            var eAge = editAge.value;
            var eGender = editGender.value;
            var eMobile = editMobile.value;

            // Check if edited values are empty, null or contain any special characters
            var format = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
            if(eName == "" || format.test(eName) 
                || eSurname == "" || format.test(eSurname) 
                || eAge == null || eAge == "" 
                || eGender == "" || format.test(eGender) 
                || eMobile == null || eMobile == "") {
                alert("Error missing values and/or special characters detected!");
            }
            else{ 
                // Post request to send the values
                var xhr = new XMLHttpRequest();

                // Display the updated data after the request has loaded
                xhr.onload = function() {
                    // Refresh the table
                    GetAllData();

                    // Hide modal pop-up
                    CloseInput();
                }

                // Retrieve the identifier from array with its position number
                if(hasItems){ // Get Id from URL retrieved objects
                    var id = itemsArray[0];
                }
                else{  // Get Id from database retrieved id's
                    var id = ids[item]; 
                }
                var url = "http://localhost:3000/users/" + id;

                // Set the request method to PUT which will update the user with the URL given and allow this to run in the background
                xhr.open("PUT", url, true);
                xhr.setRequestHeader("Content-Type", "application/json");
                // Convert to JSON 
                var data = JSON.stringify({"name": eName.trim(), 
                                        "surname": eSurname.trim(),
                                        "age":eAge,
                                        "gender":eGender.trim(),
                                        "mobile":eMobile});
                // Initiate PUT request                     
                xhr.send(data);
                
                
            }
        }
    };

    // Delete a user profile with its identifier
    this.Delete = function (item) {
        // Confirmation window
        var r = confirm("Are you sure you want to delete this profile?");
        if (r == true) { 
            // Post request to send the values
            var xhr = new XMLHttpRequest();

            // Retrieve the identifier from array with its position number
            var id = ids[item];
            var url = "http://localhost:3000/users/" + id;

            // Wait for the delete request to go through before loading the data again.
            xhr.onload = function() {
                // Display the updated data
                GetAllData();
            }
            xhr.open("DELETE", url, true);
            xhr.setRequestHeader("Content-Type", null);
            xhr.send(null);
        }
    };

}

// Modal pop-up
// REFERENCE: https://www.w3schools.com/howto/tryit.asp?filename=tryhow_css_modal2

// Get the modal
var modal = document.getElementById("myModal");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on <span> (x), close the modal
span.onclick = function() {
    CloseInput();
}

// When the user presses the "Esc" key the modal is closed
window.onkeyup = function (event) {
    if (event.keyCode == '27' && modal.style.display=='block') {
        CloseInput();
    }
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  if (event.target == modal) {
    CloseInput();
  }
}

// History API
// REFERENCE: https://css-tricks.com/using-the-html5-history-api/

// Get the rows data within the table
var container = document.querySelector('#profiles');

// When the edit button/s is clicked, execute the function
container.addEventListener('click', function(e) {
    // Check if a button has been clicked
    if (e.target != e.currentTarget) {
        e.preventDefault();
        // Get the data from the button
        var data = e.target.getAttribute('data-name'),
        // Attach the data to the URL
        url = data;
        // Update the URL with the data
        history.pushState(null, null, url);
    }
    e.stopPropagation();
}, false);

// Table sorting
// REFERENCE: https://stackoverflow.com/questions/10683712/html-table-sort

let tid = "#usersTable";
let headers = document.querySelectorAll(tid + " th");

w3.sortHTML(tid, ".item", "td:nth-child(" + (0 + 1) + ")");

// Sort the table element when clicking on the table headers
headers.forEach(function(element, i) {
  element.addEventListener("click", function() {
    w3.sortHTML(tid, ".item", "td:nth-child(" + (0 + 1) + ")");
  });
});

// On entry, get user profiles and do some validation
app.GetUserProfiles();

function CloseInput() {
    // Reset the URL to its default
    history.pushState(null, null, "index.html");

    // Show animations and hide the modal pop-up
    modal.classList.add("hideModal");
    modal.classList.remove("showModal");
    // Set a delay to show the hiding animation and then hide the modal
    setTimeout(() => { modal.style.display = "none"; }, 300);
}


