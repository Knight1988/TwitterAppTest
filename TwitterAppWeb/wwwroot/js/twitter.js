var connection = new signalR.HubConnectionBuilder().withUrl("/twitterHub").build();
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});