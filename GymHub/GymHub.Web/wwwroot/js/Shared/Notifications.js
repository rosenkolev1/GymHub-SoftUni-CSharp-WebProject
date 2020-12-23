function NotificationsInit() {
    let button = document.querySelector("#notificationMessage");
    if (button != null) {
        button.addEventListener("click", fadeOutNotification, false);

        $(document)
            .ready(x => {
                setInterval(fadeOutNotification, 5000);
            })

        function fadeOutNotification() {
            $(button)
                .fadeOut(1000);
        }
    }

    $('.carousel').carousel()

    let carouselNext = document.querySelector('.carousel-control-next');
    let carouselPrev = document.querySelector('.carousel-control-prev');

    carouselNext.addEventListener('click', e => {
        let something = "";
    })

    carouselPrev.addEventListener('click', e => {
        let something = "";
    })
};


NotificationsInit();