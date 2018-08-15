var restify = require('restify');
var builder = require('botbuilder');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3979, function () {
    console.log('Bot Application is avalable at (%s)', server.url); 
});

// Create chat connector for communicating with the Bot Framework Service
var connector = new builder.ChatConnector({ appId: process.env.MICROSOFT_APP_ID, appPassword: process.env.MICROSOFT_APP_PASSWORD });


// Listen for messages from users 
server.post('/api/order_your_grocery', connector.listen());
var messageHandler = function(session){    
    session.send("Hello there, Let me help you to order your grocery.");    
};  
// Initiate bot with connextor and message handler     
var bot = new builder.UniversalBot(connector, applesOrder.form);

