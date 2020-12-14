// Create an instance of the Stripe object with your publishable API key
var stripe = Stripe("pk_test_51HyIVdGDFvbWUUC2YDVt9tB0T4Qu46EVI8dC9zVLCAKVd549RP5EE10Hk7hayu47bYHX5s482deXwCalgr17F6hy00RlStvzbp");

var checkoutButton = document.getElementById("checkout-button");

let checkoutForm = document.querySelector('#checkout-form');

checkoutForm.addEventListener("submit", function (e) {
    e.preventDefault();
    e.stopImmediatePropagation();

    fetch("/Sales/Checkout", {
        method: "POST",
        body: new FormData(checkoutForm)
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (session) {
            //Add new payment method here for future reference
            if (session.paymentMethod === "Debit or credit card") {
                return stripe.redirectToCheckout({ sessionId: session.id });
            }
            else if (session.paymentMethod === "Cash on Delivery") {
                //Split the query string from the path
                let urlParts = session.redirectPath.split('?', 2);
                let urlPath = urlParts[0];
                let urlQueryString = `?${urlParts[1]}`;

                window.location.href = window.location.protocol + '//' + window.location.host + urlPath + urlQueryString;
            }
            else {
                window.location.pathname = "/";
            }
        })
        .then(function (result) {
            // If redirectToCheckout fails due to a browser or network
            // error, you should display the localized error message to your
            // customer using error.message.
            if (result.error) {
                alert(result.error.message);
            }
        })
        .catch(function (error) {
            console.error("Error:", error);
        });

    return false;
});