function PreviewPageInit() {
    let previewPageButton = document.querySelector('.product-addProduct-previewPage-button');

    previewPageButton.addEventListener('click', e => {
        e.preventDefault;
        e.StopPropagation;

        let [name, model, warranty, price, description, quantityInStock] =
            [
                document.querySelector('#Name').value,
                document.querySelector('#Model').value,
                parseInt(document.querySelector('#Warranty').value),
                parseFloat(document.querySelector('#Price').value),
                document.querySelector('#Description').value,
                parseInt(document.querySelector('#QuantityInStock').value)
            ];

        let mainImage;
        let additionalImages;
        let isImageUploadMode = document.querySelector("#ImagesAsFileUploads").value.toLowerCase();

        //Get the main image value depending on the image mode
        mainImage = document.querySelector('.input-image-link-main').value;
        //if (isImageUploadMode === "false") mainImage = document.querySelector('.input-image-link-main').value;
        //else mainImage = document.querySelector('.image-upload-main').src;

        //Get the additional images depending on the image mode
        additionalImages = Array.from(document.querySelectorAll('.product-addProduct-additionalImage')).map(x => x.value);
        //if (isImageUploadMode === "false") additionalImages = Array.from(document.querySelectorAll('.product-addProduct-additionalImage')).map(x => x.value);
        //else additionalImages = Array.from(document.querySelectorAll('.image-upload-additional')).map(x => x.src);

        let productCategoriesInputs = Array.from(document.querySelectorAll('.category-select'))
            .reduce((acc, x) => {
                if (x.value && x.value !== 'null') acc.push(x.value);
                return acc;
            }, []);

        //Simple validation for these stuff
        if (isNaN(price)) price = 0;
        if (isNaN(warranty)) warranty = 0;
        if (isNaN(quantityInStock)) quantityInStock = 0;

        let dataObject = {
            Name: name,
            Model: model,
            MainImage: mainImage,
            Warranty: warranty,
            Price: price,
            Description: description,
            QuantityInStock: quantityInStock,
            AdditionalImages: additionalImages,
            ProductCategoriesNames: productCategoriesInputs
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

        //If we are in image upload mode, then manually place the images in the correct spots
        let isImageUploadMode = document.querySelector("#ImagesAsFileUploads").value.toLowerCase();
        if (isImageUploadMode === "true") {
            let mainImageSrc = document.querySelector('.image-upload-main').src;
            let additionalImagesSources = Array.from(document.querySelectorAll('.image-upload-additional')).map(x => x.src);

            let productMainImagePreview = document.querySelector('.product-mainImage');
            productMainImagePreview.src = mainImageSrc;

            let additionalImagesPreview = Array.from(document.querySelectorAll('.product-additionalImage'));
            additionalImagesPreview.forEach((additionalImage, index) => {
                additionalImage.src = additionalImagesSources[index];
            })
        }
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