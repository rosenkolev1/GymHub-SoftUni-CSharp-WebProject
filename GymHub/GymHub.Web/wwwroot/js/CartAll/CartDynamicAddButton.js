function CartDynamicAddButtonInit() {

    let addButton = document.querySelector('.cart-form-buyButton-inactive');

    if (addButton) addButton.addEventListener('click', disableBuyButtonEvent);

    let quantityInputs = document.querySelectorAll('.cart-input-quantity');

    quantityInputs.forEach(quantityInput => {
        quantityInput.addEventListener('change', e => {
            let quantityInStockForProductEl = quantityInput.closest('.cart-tr-row').querySelector('.cart-input-product-quantityInStock');
            let quantityInStockForProduct = parseInt(quantityInStockForProductEl.value);

            let purchaseQuantity = parseInt(quantityInput.value);

            //If the product's quantity doesn't exceed the quantity in stock then remove the on screen errors and enable the buy button
            if (purchaseQuantity <= quantityInStockForProduct) {

                //Hide the span validation for the product
                let productValidationSpan = quantityInput.closest('#quantity').querySelector('.validation-span-cart');
                productValidationSpan.textContent = "";
                productValidationSpan.classList.add('validation-span-hidden');
                productValidationSpan.classList.remove('field-validation-error');
                productValidationSpan.classList.add('field-validation-valid');

                //If all span validations are hidden, then enable the buy button
                let allSpanValidationsAreValid = true;
                Array.from(document.querySelectorAll('.validation-span-cart')).forEach(spanValidation => {
                    if (spanValidation.classList.contains('field-validation-error')) allSpanValidationsAreValid = false; 
                })

                if (allSpanValidationsAreValid) {
                    //Enable buy button
                    let buyButton = document.querySelector('#cart-buyButton');
                    buyButton.className = 'cart-form-buyButton';
                    enableBuyButtonEvent();
                }
            }
        })
    })

    function enableBuyButtonEvent() {
        let addButtonActive = document.querySelector('.cart-form-buyButton');
        addButton.removeEventListener('click', disableBuyButtonEvent);
    }

    function disableBuyButtonEvent(e) {
        e.preventDefault();
        e.stopImmediatePropagation();
    }
}

CartDynamicAddButtonInit();