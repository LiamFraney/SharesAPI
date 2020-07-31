

// $('.message a').click(function(){
//     $('form').animate({height: "toggle", opacity: "toggle"}, "slow");
//  });

 // Get the modal
var modal = document.getElementById('id01');

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

window.onload = function() {
    this.refreshTable();
    this.refreshUser();
    this.getCurrency();
}

function storeCredentials(username, password){
    //localStorage.setItem("username", username);
    //localStorage.setItem("password", password);
    localStorage.setItem("username", "liam");
    localStorage.setItem("password", "password");
    console.log(localStorage.getItem("username"), localStorage.getItem("password"));
}

function buyStock(symbolIn) {
    var purchase = {
        symbol: symbolIn,
        quantity: 1
    }
    $.ajax({
        type: "POST",
        contentType: 'application/json',
        url: "https://localhost:5001/purchase",
        data: JSON.stringify(purchase),
        headers: {"username": localStorage.getItem("username"),
                  "password": localStorage.getItem("password")},
        error: function(msg){
            // will fire when timeout is reached
        },
        success: function(msg){
            refreshTable();
            refreshUser();
        },
        timeout: 3000 // sets timeout to 3 seconds
    }); 
}

function getCurrency(){
    var currency = null;
    $.ajax({
        dataType: "json",
        async: false,
        url: "https://localhost:8000/currency_converter/?format=json",
        error: function(data){
            // will fire when timeout is reached
        },
        success: function(msg){
            currency = msg;
        },
        timeout: 3000 // sets timeout to 3 seconds
    });
    return currency;
}

function getUser() {
    var savedUser = null;
    $.ajax({
        dataType: "json",
        async: false,
        url: "https://localhost:5001/user",
        headers: {"username": localStorage.getItem("username"),
                  "password": localStorage.getItem("password")},
        error: function(data){
            // will fire when timeout is reached
        },
        success: function(msg){
            savedUser = msg;
        },
        timeout: 3000 // sets timeout to 3 seconds
    });
    return savedUser;
}

function refreshTable() {
    user = getUser();
    console.log(user);
    $.ajax({
        url: "https://localhost:5001/shares?money=USD",
        error: function(data){
            // will fire when timeout is reached
        },
        success: function(data){
            //do something
            console.log(data);
            var template = $('#stocks-template').html();
            var html = "";
            for (i = 0; i < data.length; i++)
            {
                for (j = 0; j < user.aquiredShares.length; j++)
                {
                    if (user.aquiredShares[j].type == data[i].symbol)
                    {
                        data[i].owned = user.aquiredShares[j].quantity;
                    }
                }
                html += Mustache.to_html(template, data[i]);
            }
            $('#tableBody').html(html);
        },
        timeout: 3000 // sets timeout to 3 seconds
    });
}
function refreshUser() {
    user = getUser();
    console.log(user);
    $.ajax({
        url: "https://localhost:5001/user",
        headers: {"username": localStorage.getItem("username"),
                  "password": localStorage.getItem("password")},

        error: function(data){
            // will fire when timeout is reached
        },
        success: function(data){
            //do something
            console.log(data);
            var html = "";
            var template = $('#user-template').html();
            
                html += Mustache.to_html(template, data);
            
            $('#user-info').html(html);
        },
        timeout: 3000 // sets timeout to 3 seconds
    });
}


