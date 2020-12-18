function LoadImageFromFileInputInit(){
    let inputImageUploads = Array.from(document.querySelectorAll('.input-image-upload'));

    inputImageUploads.forEach(inputImageUpload => {
        inputImageUpload.addEventListener('change', e => {
            let selectedFile = e.target.files[0];
            let imageTag = inputImageUpload.closest('.input-container').querySelector('.input-image-upload-image');
            readImage(selectedFile, imageTag);
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