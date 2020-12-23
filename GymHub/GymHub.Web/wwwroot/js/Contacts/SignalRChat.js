function SignalRChatInit() {
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
                }
            })

            //Mark the message as seen
            let anitforgeryTokenValue = document.querySelector('#antiforgeryToken-form').querySelector('[name=__RequestVerificationToken]').value;

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
        else {
            //Add to the unseen count from the view
            let unseenMessagesCountSpan = document.querySelector(`.unseen-messages-count-${senderId}`);

            unseenMessagesCountSpan.textContent = parseInt(unseenMessagesCountSpan.textContent) + 1;
        }

    }
}

SignalRChatInit();
