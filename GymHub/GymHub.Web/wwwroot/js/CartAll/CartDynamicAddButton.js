function CartDynamicAddButtonInit() {

    let addButton = document.querySelector('.cart-form-buyButton-inactive');

    addButton.addEventListener('click', e => {
        e.preventDefault();
        e.stopImmediatePropagation();
    })

}
CartDynamicAddButtonInit();