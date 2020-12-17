function SwitchImageModeInit()
{
    let imageLinksContainer = document.querySelector('.image-links-container');
    let imageUploadsContainer = document.querySelector('.image-uploads-container');
    let imageModeSwitcher = document.querySelector('.image-mode-container');



    imageModeSwitcher.addEventListener('click', e => {
        imageLinksContainer.toggleAttribute('hidden');
        imageUploadsContainer.toggleAttribute('hidden');

        //Change the text content of the button
        if (imageLinksContainer.hasAttribute('hidden')) {
            imageModeSwitcher.querySelector('.image-mode-button').textContent = 'Switch to image links';
        }
        else if (imageUploadsContainer.hasAttribute('hidden')) {
            imageModeSwitcher.querySelector('.image-mode-button').textContent = 'Switch to image uploads';
        }
    });
}

SwitchImageModeInit();