function ChangeProductImageInit(){
    let additionalImages = Array.from(document.querySelectorAll('.product-additionalImage'));
    let mainImage = document.querySelector('.product-mainImage');
    
    additionalImages.forEach(additionalImage => {
        additionalImage.addEventListener('click', e => {
            let additionalImageSrc = additionalImage.src;
            let mainImageSrc = mainImage.src;

            additionalImage.src = mainImageSrc;
            mainImage.src = additionalImageSrc;
        })
    })
}

ChangeProductImageInit();