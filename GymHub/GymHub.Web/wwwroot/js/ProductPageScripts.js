function Init() {
    let initFunctions = [
        AddLikingComments,
        SetFormDataPageFragment,
        ShowRepliesButton,
        EditButtonsInit,
        DynamicRating,
        ProductPageCommentBoxInit,
        ProductRemoveCommentPopupInit,
        ProductReplyCommentInit,
        ProductPageCommentsPaginationInit
    ]

    initFunctions.forEach(initFunction => {
        try {
            initFunction();
        }
        catch {
        }
    })
}

Init();