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

// Load grocery order form 
var groceryOrderForm = require('./groceryOrderForm.js')(builder);

// Initialize bot with connector and default dialog
bot = new builder.UniversalBot(connector, [
    function (session) {
      session.beginDialog(groceryOrderForm.welcomeDialog.title);
    }
  ]);

bot.dialog(groceryOrderForm.welcomeDialog.title, groceryOrderForm.welcomeDialog.functions);
bot.dialog(groceryOrderForm.menuDialog.title, groceryOrderForm.menuDialog.functions);
bot.dialog(groceryOrderForm.checkoutDialog.title, groceryOrderForm.checkoutDialog.functions).triggerAction({
    matches: /^check out$/i,
    confirmPrompt: "This will cancel your request. Are you sure?"
});