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
        if (isImageUploadMode === "false") mainImage = document.querySelector('.input-image-link-main').value;
        else {
            let mainImageEl = document.querySelector('.image-upload-main');
            mainImage = mainImageEl.src;

            //If image comes from user image upload then don't send it to the server, but instead handle it client side
            if (mainImage.startsWith('data:image')) mainImage = '';
        }

        //Get the additional images depending on the image mode
        if (isImageUploadMode === "false") additionalImages = Array.from(document.querySelectorAll('.product-addProduct-additionalImage')).map(x => x.value);
        else {
            additionalImages = Array.from(document.querySelectorAll('.image-upload-additional')).map(x => {
                let imageEl = x;
                let image = imageEl.src;

                //If image comes from user image upload then don't send it to the server, but instead handle it client side
                if (image.startsWith('data:image')) image = '';

                return image;
            });
        }

        let productCategoriesInputs = Array.from(document.querySelectorAll('.category-select'))
            .reduce((acc, x) => {
                if (x.value && x.value !== 'null') {
                    let optionsInSelect = Array.from(x.querySelectorAll('option'));
                    let optionSelected = optionsInSelect.find(y => y.value === x.value);

                    acc.push(optionSelected.textContent);
                }
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
            CategoriesNames: productCategoriesInputs
        };

        let dataObjectJSON = JSON.stringify(dataObject);

        $.ajax({
            method: "GET",
            url: "/Administration/Products/LoadAddProductPreview",
            data: { inputModelJSON: dataObjectJSON },
            success: addPagePreview,
            error: addPageReviewError
        })
    })

    function CheckIfImageElHasNotBeenModifiedAndIsUnlocked(imageEl) {

        //If image has not been modified and the lock is lifted, then show empty image. Otherwise return the original src
        let inputContainer = imageEl.closest('.input-container');
        let hasNotBeenModified = inputContainer.querySelector('.input-image-hasNotBeenModified');
        let inputLock = inputContainer.querySelector('.product-edit-image-upload-lock .fas');
        let originalImagePath = inputContainer.querySelector('.input-image-originalImagePath');
        let originalImagePathValue;
        if (originalImagePath) originalImagePathValue = originalImagePath.value;

        if (hasNotBeenModified && inputLock.classList.contains('fa-unlock')) return '';
        else if (!hasNotBeenModified && inputLock) {
            if (inputLock.classList.contains('fa-lock')) {
                return originalImagePathValue;
            }
        }

        return imageEl.src;
    }

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
            let mainImage = document.querySelector('.image-upload-main');

            let mainImageSrc = CheckIfImageElHasNotBeenModifiedAndIsUnlocked(mainImage);
            let additionalImagesSources = Array.from(document.querySelectorAll('.image-upload-additional'))
                .map(imageEl => {
                    return CheckIfImageElHasNotBeenModifiedAndIsUnlocked(imageEl);
                });

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