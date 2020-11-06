function SetTotalPrice() {
    let quantityInputs = document.querySelectorAll(".cart-input-quantity");

    quantityInputs.forEach(input => {
        input.addEventListener("input", e => {
            //Change one product total price
            var productRow = input.parentElement.parentElement;
            let productQuantity = input.value;
            let singlePriceEl = productRow.querySelector("#singlePrice").querySelector("p");
            let singlePrice = parseFloat(singlePriceEl.textContent.split("$")[0].trim());
            productRow.querySelector(".totalPrice").textContent = (parseInt(productQuantity) * singlePrice).toFixed(2);  

            //Now change all products total price
            InitializeAllProductsTotalPrice();
        })
    })


    //let productsRows = document.querySelectorAll(".cart-tr-row-even");
    //productsRows.forEach(productRow => {
    //    if (productRow.id != "allProductsTotalPriceTr") {
    //        let productQuantity = productRow.querySelector("#quantity").querySelector(".cart-input-quantity").value;
    //        let singlePriceEl = productRow.querySelector("#singlePrice").querySelector("p");
    //        let singlePrice = parseFloat(singlePriceEl.textContent.split("$")[0].trim());
    //        productRow.querySelector(".totalPrice").textContent = (parseInt(productQuantity) * singlePrice).toFixed(2);
    //    }
    //})
    //

}

function InitializeAllProductsTotalPrice(){
    let allTotalPrices = document.querySelectorAll(".totalPrice");
    let allProductsTotalPrice = 0;

    allTotalPrices.forEach(totalPriceEl => {
        let totalPrice = parseFloat(totalPriceEl.textContent);
        allProductsTotalPrice += totalPrice;
    })

    let allProductsTotalPriceEl = document.querySelector("#allProductsTotalPrice");
    allProductsTotalPriceEl.textContent = allProductsTotalPrice.toFixed(2);
}

//setInterval(SetTotalPrice, 100);
InitializeAllProductsTotalPrice()
SetTotalPrice();