var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

// The Receive Message Client event. This will trigger, when the Back-End calls the ReceiveMessage method

connection.on("NewMessage", function () {
    
});

//An error handler for connection errors

connection.start().catch(function (err) {

    return console.error(err.toString());

});

// The Send Message DOM event. This will trigger the Back-End SendMessage method

document.getElementById("sendButton").addEventListener("click", function () {
    
});
