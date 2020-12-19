function ManageImageUploadsLockInit() {
    let imageLocks = Array.from(document.querySelectorAll('.product-edit-image-upload-lock'));

    imageLocks.forEach(imageLock => {
        imageLock.addEventListener('click', e => {
            let imageContainer = imageLock.closest('.input-container');

            let isBeingModifiedInput = imageContainer.querySelector('.input-image-isBeingModified');

            if (isBeingModifiedInput.value.toLowerCase() === 'true') {
                isBeingModifiedInput.setAttribute('value', false);
            }
            else if (isBeingModifiedInput.value.toLowerCase() === 'false') {
                isBeingModifiedInput.setAttribute('value', true);
            }

            let imageInput = imageContainer.querySelector('.input-image-upload');

            //Change appearance of now activated/deactivated input
            if (imageInput.classList.contains('edit-deactivated')) {
                imageInput.classList.replace('edit-deactivated', 'edit-activated');
                imageLock.querySelector('i').classList.replace('fa-lock', 'fa-unlock');

                //Also show the img tag for an already uploaded image file when you press the unlock button to make it clear that the image is going to get used
                let imageTag = imageContainer.querySelector('.input-image-upload-image');
                if (imageTag.hasAttribute('src')) imageTag.removeAttribute('hidden');
            }
            else {
                imageInput.classList.replace('edit-activated', 'edit-deactivated');
                imageLock.querySelector('i').classList.replace('fa-unlock', 'fa-lock');

                //Also hide the img tag for an already uploaded image file when you press the lock button to make it clear that the image isn't going to get used even if selected
                imageContainer.querySelector('.input-image-upload-image').setAttribute('hidden', 'true');
            }

            StopUserFromUploadingToLockedImageUploads();
        })
    })

    StopUserFromUploadingToLockedImageUploads();

    function StopUserFromUploadingToLockedImageUploadsHandler(e) {
        if (e.target.classList.contains('edit-deactivated')) {
            e.preventDefault();
            e.stopImmediatePropagation();
        }
    }

    function StopUserFromUploadingToLockedImageUploads() {
        let allImageUploadsInputs = Array.from(document.querySelectorAll('.input-image-upload'));

        allImageUploadsInputs.forEach(imageUploadInput => {
            imageUploadInput.removeEventListener('click', StopUserFromUploadingToLockedImageUploadsHandler);
            imageUploadInput.addEventListener('click', StopUserFromUploadingToLockedImageUploadsHandler);
        })
    }
}

ManageImageUploadsLockInit();