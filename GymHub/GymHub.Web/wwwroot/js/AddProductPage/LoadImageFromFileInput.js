function LoadImageFromFileInputInit(){
    let inputImageUploads = Array.from(document.querySelectorAll('.input-image-upload'));

    inputImageUploads.forEach(inputImageUpload => {
        inputImageUpload.addEventListener('change', e => {
            let inputContainer = inputImageUpload.closest('.input-container');

            //Remove the span which suggests that the image hasn't been modified  if it exists
            let hasNotBeenModified = inputContainer.querySelector('.input-image-hasNotBeenModified');
            if (hasNotBeenModified) hasNotBeenModified.remove();

            let selectedFile = e.target.files[0];
            let imageTag = inputContainer.querySelector('.input-image-upload-image');

            if (selectedFile) {
                readImage(selectedFile, imageTag);
            }
            else {
                imageTag.removeAttribute('src');
                imageTag.setAttribute('hidden', 'true');
            }
        })
    })
    
    function readImage(file, imageTag) {
      // Check if the file is an image.
      if (file.type && file.type.indexOf('image') === -1) {
        console.log('File is not an image.', file.type, file);
        return;
      }

      const reader = new FileReader();
      reader.addEventListener('load', (event) => {
          let dataImageUri = event.target.result;

          imageTag.src = dataImageUri;
          imageTag.removeAttribute('hidden');
      });
      reader.readAsDataURL(file);
    }
}

LoadImageFromFileInputInit();