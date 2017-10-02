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
server.post('/api/order_apples', connector.listen());
// Load apples order form
var applesOrder = require('./applesOrderForm.js')(builder);
// Initialize bot with connector and array of tasks in apples order form
var bot = new builder.UniversalBot(connector, applesOrder.form);