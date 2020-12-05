function PreviewPageInit() {
    let previewPageButton = document.querySelector('.product-addProduct-previewPage-button');

    previewPageButton.addEventListener('click', e => {
        e.preventDefault;
        e.StopPropagation;

        let [name, model, mainImage, warranty, price, description, quantityInStock] =
            [
                document.querySelector('#Name').value,
                document.querySelector('#Model').value,
                document.querySelector('#MainImage').value,
                parseInt(document.querySelector('#Warranty').value),
                parseFloat(document.querySelector('#Price').value),
                document.querySelector('#Description').value,
                parseInt(document.querySelector('#QuantityInStock').value)
            ];
        let additionalImages = Array.from(document.querySelectorAll('.product-addProduct-additionalImage')).map(x => x.value);

        let dataObject = {
            Name: name,
            Model: model,
            MainImage: mainImage,
            Warranty: warranty,
            Price: price,
            Description: description,
            QuantityInStock: quantityInStock,
            AdditionalImages: additionalImages
        };

        let dataObjectJSON = JSON.stringify(dataObject);

        $.ajax({
            method: "GET",
            url: "/Products/LoadAddProductPreview",
            data: { inputModelJSON: dataObjectJSON },
            success: addPagePreview,
            error: addPageReviewError
        })
    })

    function addPagePreview(result) {
        let addProductForm = document.querySelector('.product-addProduct-form');
        let previewContainer = document.querySelector('.product-addProduct-preview');

        if (previewContainer !== null) {
            previewContainer.remove();
        }

        addProductForm.insertAdjacentHTML('afterend', result);
        ChangeProductImageInit();
    }

    function addPageReviewError(error) {
        let addProductForm = document.querySelector('.product-addProduct-form');
        let previewContainer = document.querySelector('.product-addProduct-preview');

        if (previewContainer !== null) {
            previewContainer.remove();
        }

        addProductForm.insertAdjacentHTML('afterend', "<p>An error has occured. Can't show preview</p>");
    }

}

PreviewPageInit();