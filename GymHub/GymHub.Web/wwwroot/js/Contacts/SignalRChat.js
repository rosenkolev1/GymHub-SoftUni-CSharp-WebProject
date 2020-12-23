function SignalRChatInit() {
    //Make chat be scrolled to the bottom on page load by default
    ScrollChatToBottom();
    //When the user has a new message and he scroll the chat to the bottom, then remove that text.
    removeNewMessagesTextOnReachingChatBottomInit();

    let connection = new signalR.HubConnectionBuilder().withUrl("/contactsChat").build();
    let receiverEl = document.querySelector('#targetUser-id');
    let senderEl = document.querySelector('#currentUser-id')

    //An error handler for connection errors
    connection.start().catch(function (err) {

        return console.error(err.toString());

    });

    // The Receive Message Client event. This will trigger, when the Back-End calls the ReceiveMessage method
    connection.on("NewMessage", ReceivedNewMessageHandler);

    // The Send Message DOM event. This will trigger the Back-End SendMessage method
    document.querySelector(".send_btn").addEventListener("click", ClickSendMessageButtonHandler);

    function ClickSendMessageButtonHandler(e) {

        let messageTextarea = document.querySelector('.type_msg');
        let messageText = messageTextarea.value;
        messageTextarea.setAttribute('value', "");
        messageTextarea.value = null;

        if (messageText == false) {
            return;
        }

        connection.invoke('Send', messageText, receiverEl.value).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function ReceivedNewMessageHandler(messageInputModel) {

        let currentUserId = document.querySelector('#currentUser-id').value;

        let receiverId = messageInputModel.receiverId;
        let senderId = messageInputModel.senderId;
        let belongsToSender = currentUserId == senderId;
        let messageText = messageInputModel.message;
        let sentOn = messageInputModel.sentOn;
        let messageId = messageInputModel.messageId;

        if ((receiverId == senderEl.value && senderId == receiverEl.value) || (receiverId == receiverEl.value && senderId == senderEl.value)) {
            $.ajax({
                url: "/Home/LoadMessage",
                data: { BelongsToSender: belongsToSender, Text: messageText, SentOn: sentOn, SenderId: senderId },
                method: "post",
                success: (messageHtml) => {
                    let totalMessages = document.querySelector('.messages-count');
                    let totalMessagesTextContentParts = totalMessages.textContent.split(" ");
                    totalMessages.textContent = `${parseInt(totalMessagesTextContentParts[0]) + 1} ${totalMessagesTextContentParts[1]}`;

                    let messagesList = document.querySelector('.msg_card_body');
                    messagesList.insertAdjacentHTML('beforeend', messageHtml);

                    //If it is the current user that typed, then auto scroll the chat to the bottom.
                    if (receiverId == receiverEl.value && senderId == senderEl.value) {
                        ScrollChatToBottom();
                    }
                    //If the current user is the receiver, then show a message box which says that there are new messages for him
                    else {
                        //If the current user, who is a receiver, is not already at the bottom of the page, then show the new message box and append new messages
                        if (userIsAtBottomOfTheChat(messagesList) == false) {
                            let newMessageArrivedHTML = '<p style="font-size:10px; text-align:center;" class="alert-danger newMessageArrived">You have a new message</p>';
                            let newMessageArrived = document.querySelector('.newMessageArrived');
                            if (!newMessageArrived) {
                                messagesList.insertAdjacentHTML('beforebegin', newMessageArrivedHTML);
                            };
                        }
                        //If the current user, who is a receiver, is already at the bottom of the page, then dont show the new message box and automatically scroll to the bottom
                        else {
                            ScrollChatToBottom();
                        }
                    }
                }
            })

            //Mark the message as seen
            let anitforgeryTokenValue = document.querySelector('#antiforgeryToken-form').querySelector('[name=__RequestVerificationToken]').value;

            if (receiverId == senderEl.value && senderId == receiverEl.value) {
                $.ajax({
                    url: "/Home/MarkAsSeen",
                    data: { messageId: messageId, __RequestVerificationToken: anitforgeryTokenValue },
                    method: "post",
                    success: (success) => {
                        if (success === "Success") {

                        }
                        else {
                            throw "Some error occured here";
                        }
                    },
                    error: () => {
                        throw "An error occured here";
                    }
                })
            }
        }
        else {
            //Add to the unseen count from the view
            let unseenMessagesCountSpan = document.querySelector(`.unseen-messages-count-${senderId}`);

            unseenMessagesCountSpan.textContent = parseInt(unseenMessagesCountSpan.textContent) + 1;
        }

    }

    //Make the chat be scrolled to the bottom by default on load of page
    function ScrollChatToBottom() {
        $(".msg_card_body").stop().animate({ scrollTop: $(".msg_card_body")[0].scrollHeight }, 1);
    }


    //When the user has a new message and he scroll the chat to the bottom, then remove that text.
    function removeNewMessagesTextOnReachingChatBottomInit() {
        let messagesList = document.querySelector('.msg_card_body'); 

        $(messagesList).scroll(removeNewMessagesTextOnReachingChatBottomHandler(messagesList));
    }

    function removeNewMessagesTextOnReachingChatBottomHandler(messagesList) {
        return () => {
            if (userIsAtBottomOfTheChat(messagesList)) {
                let newMessageArrived = document.querySelector('.newMessageArrived');
                if (newMessageArrived) {
                    newMessageArrived.remove();
                };
            }
        }
    }

    function userIsAtBottomOfTheChat(messagesList) {
        let scrollTop = $(messagesList).scrollTop();
        let elementHeight = $(messagesList).height();
        let scrollTopAndElHeightSum = scrollTop + elementHeight;
        let scrollHeight = $(messagesList)[0].scrollHeight;

        return scrollTopAndElHeightSum + 150 >= scrollHeight;
    }
}

SignalRChatInit();
