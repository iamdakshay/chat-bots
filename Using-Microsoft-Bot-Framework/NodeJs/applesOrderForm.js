var applesOrder = function (builder) {

  var welcomePrompt = function (session) {

    session.send("Hello there, I am here to help you to order you grocery.<br/>");
    builder.Prompts.text(session, "May I know your good name?");
  };

  var numberOfApplesPrompt = function (session, results) {

    session.dialogData.userName = results.response;
    builder.Prompts.number(session, "Hey " + session.dialogData.userName + ", How many apples you wanna order?");
  };

  var userAddressPrompt = function (session, results) {

    session.dialogData.noOfApples = results.response;
    builder.Prompts.text(session, "What will be delivery address?");
  };

  var deliveryTimePrompt = function (session, results) {

    session.dialogData.userAddress = results.response;
    builder.Prompts.time(session, "What will be your prefered time for delivery? (e.g.: 22nd Oct at 3pm)");
  };

  var goodByePrompt = function (session, results) {

    session.dialogData.deliveryTime = builder.EntityRecognizer.resolveTime([results.response]);
    session.send("Order  has been placed!<br/> It will be delivered to you by %s.", session.dialogData.deliveryTime);
    session.send("Have a good time, " + session.dialogData.userName + "!");
  };

  this.form = [welcomePrompt, numberOfApplesPrompt, userAddressPrompt, deliveryTimePrompt, goodByePrompt];
};

module.exports = function (builder) {
  return new applesOrder(builder);
};