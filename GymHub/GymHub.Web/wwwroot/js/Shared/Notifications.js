function NotificationsInit() {
    var button = document.querySelector("#notificationMessage");
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
};


NotificationsInit();