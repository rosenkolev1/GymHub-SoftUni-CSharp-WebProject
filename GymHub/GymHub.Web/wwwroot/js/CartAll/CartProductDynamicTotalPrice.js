function SetProductTotalPrice() {
    let quantityInputs = document.querySelectorAll(".cart-input-quantity");

    quantityInputs.forEach(input => {
        input.addEventListener("change", e => {
            let productRow = input.closest('.cart-tr-row');

            let productQuantityInStock = parseInt(productRow.querySelector('.cart-input-product-quantityInStock').value);
            
            let productQuantity = parseInt(input.value);

            if (productQuantity <= 0) {
                productQuantity = 1;
                input.value = 1;
            }
            else if (productQuantity > productQuantityInStock) {
                productQuantity = productQuantityInStock;
                input.value = productQuantityInStock;
            }

            //Change one product total price
            let singlePriceEl = productRow.querySelector("#singlePrice").querySelector("p");
            let singlePrice = parseFloat(singlePriceEl.textContent.split("$")[0].trim());
            productRow.querySelector(".totalPrice").textContent = `$${(productQuantity * singlePrice).toFixed(2)}`;  

            //Now change all products total price
            SetAllProductsTotalPrice();
        })
    })
}

function SetAllProductsTotalPrice(){
    let allTotalPrices = document.querySelectorAll(".totalPrice");
    let allProductsTotalPrice = 0;

    allTotalPrices.forEach(totalPriceEl => {
        let totalPrice = parseFloat(totalPriceEl.textContent.substring(1));
        allProductsTotalPrice += totalPrice;
    })

    let allProductsTotalPriceEl = document.querySelector("#allProductsTotalPrice");
    allProductsTotalPriceEl.textContent = `$${allProductsTotalPrice.toFixed(2)}`;
}

SetAllProductsTotalPrice()
SetProductTotalPrice();