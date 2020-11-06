function SetTotalPrice() {
    let productsRows = document.querySelectorAll(".cart-tr-row-even");
    productsRows.forEach(productRow => {
        if (productRow.id != "allProductsTotalPriceTr") {
            let productQuantity = productRow.querySelector("#quantity").querySelector("p").textContent;
            let singlePriceEl = productRow.querySelector("#singlePrice").querySelector("p");
            let singlePrice = parseFloat(singlePriceEl.textContent.split("$")[0].trim());
            productRow.querySelector(".totalPrice").textContent = (parseInt(productQuantity) * singlePrice).toFixed(2);
        }
    })

    let allTotalPrices = document.querySelectorAll(".totalPrice");
    let allProductsTotalPrice = 0;
    allTotalPrices.forEach(totalPriceEl => {
        let totalPrice = parseFloat(totalPriceEl.textContent);
        allProductsTotalPrice += totalPrice;
    })

    let allProductsTotalPriceEl = document.querySelector("#allProductsTotalPrice");
    allProductsTotalPriceEl.textContent = allProductsTotalPrice.toFixed(2);
}

setInterval(SetTotalPrice, 100);