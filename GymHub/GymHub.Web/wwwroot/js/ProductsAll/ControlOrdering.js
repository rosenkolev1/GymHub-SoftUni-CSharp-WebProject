function ControlOrderingInit() {
    let orderByDescButton = document.querySelector('.products-all-order-desc');
    let orderByAscButton = document.querySelector('.products-all-order-asc');
    let IsDescendingInput = document.querySelector('.products-all-input-IsDescending');

    orderByAscButton.addEventListener('click', e => {
        IsDescendingInput.setAttribute('value', 'false');
        orderByAscButton.classList.add('gold');
        orderByDescButton.classList.remove('gold');
    })

    orderByDescButton.addEventListener('click', e => {
        IsDescendingInput.setAttribute('value', 'true');
        orderByDescButton.classList.add('gold');
        orderByAscButton.classList.remove('gold');
    })
}

ControlOrderingInit();