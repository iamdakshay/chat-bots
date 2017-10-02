var groceryOrder = function (builder) {

  var groceryMenuItems = ["Rice", "Soap", "Corn Flakes"];

  this.welcomeDialog = {
    title: "Welcome",
    functions: [
      function (session) {
        builder.Prompts.text(session, "Welcome, may I know your good name?");
      },
      function (session, results) {
        session.userData.User = results.response;
        session.send("Hello %(User)s,<br/>I am here to take your order!", session.userData);
        session.beginDialog("Menu");
      }
    ]
  };

  this.menuDialog = {
    title: "Menu",
    functions: [
      function (session, flags) {
        if (flags && flags.IsMoreOrder) {
          builder.Prompts.choice(session, "Select more..", groceryMenuItems, { listStyle: builder.ListStyle.button });
        }
        else {
          session.userData.groceryItems = [];
          builder.Prompts.choice(session, "Please select grocery item from list:", groceryMenuItems, { listStyle: builder.ListStyle.button });
        }
      },
      function (session, results) {
        session.userData.groceryItems.push(results.response.entity);
        session.replaceDialog("Menu", { IsMoreOrder: true });
      }
    ]
  };

  this.checkoutDialog = {
    title: "Checkout",
    functions: [
      function (session) {
        builder.Prompts.text(session, "Where should we deliver your order?");
      },
      function (session, results) {
        session.userData.Address = results.response;
        builder.Prompts.time(session, "What time will you prefer?");
      },
      function (session, results) {
        session.userData.deliveryTime = builder.EntityRecognizer.resolveTime([results.response]);
        session.send("Thanks for your order %(User)s.", session.userData);
        console.log(session.userData.groceryItems);
        session.send("Your order of %(groceryItems)s will be delivered by %(deliveryTime)s", session.userData);
        session.endDialog();
      }
    ],
  };
};

module.exports = function (builder) {
  return new groceryOrder(builder);
};