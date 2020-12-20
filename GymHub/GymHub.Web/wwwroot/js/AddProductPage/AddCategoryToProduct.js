function AddCategoryToProductInit() {
    let addCategory = document.querySelector('#addCategoryToProduct');

    addCategory.addEventListener('click', x => {
        //Get number of currently added categories
        let categoriesContainers = Array.from(document.querySelectorAll('.product-addCategory-container'));
        let categoryInputCounters = [];

        categoriesContainers.forEach(container => {
            let counterInput = container.querySelector('.addCategory-input-counter');
            categoryInputCounters.push(parseInt(counterInput.value));
        })

        let categoriesCount = 0;
        if (categoryInputCounters.length > 0) {
            categoriesCount = Math.max(...categoryInputCounters);
        }

        $.ajax({
            url: `/Administration/Products/LoadCategoryInput?Counter=${categoriesCount+1}`,
            method: 'get',
            success: LoadCategoryInputSuccess
        })
    })

    function LoadCategoryInputSuccess(result) {

        addCategory.closest('#addCategoryToProduct-container').insertAdjacentHTML('beforebegin', result);

        SetRemoveCategoryButtonsEventHandlers();
    }

    function SetRemoveCategoryButtonsEventHandlers() {
        let removeButtons = Array.from(document.querySelectorAll('.removeCategoryButton'));

        removeButtons.forEach(button => {
            button.removeEventListener('click', RemoveCategoryButtonHandler);
            button.addEventListener('click', RemoveCategoryButtonHandler);
        })
    }

    function RemoveCategoryButtonHandler(e) {
        let categoryInputContainer = e.target.closest('.product-addCategory-container');
        categoryInputContainer.remove();
    }

    let addProductForm = document.querySelector('.product-addProduct-form');

    $(addProductForm)
        .submit(e => {
            let categorySelectInputs = Array.from(document.querySelectorAll('.category-select'));
            categorySelectInputs.forEach((selectInput, i) => {
                let selectName = selectInput.getAttribute('name');
                selectName = selectName.split('[', 2)[0] + `[${i}]` + selectName.split('[')[1].split(']')[1];
                selectInput.setAttribute('name', selectName);
            })
        })

    SetRemoveCategoryButtonsEventHandlers();
}

AddCategoryToProductInit();