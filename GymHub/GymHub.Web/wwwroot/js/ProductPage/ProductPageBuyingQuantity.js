function ProductPageBuyingQuantityInit(){
    let productQuantity = document.querySelector('.product-page-addToCart-quantity');
    let productQuantityInStock = parseInt(document.querySelector('.product-page-addToCart-quantityInStock').value);

    productQuantity.addEventListener('change', e => {
        if (parseInt(productQuantity.value) <= 0) {
            productQuantity.value = 1;
        }
        else if (parseInt(productQuantity.value) > productQuantityInStock) {
            productQuantity.value = productQuantityInStock;
        }
    })
}

//ProductPageBuyingQuantityInit()

