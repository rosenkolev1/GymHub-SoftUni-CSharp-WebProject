function Init() {
    let initFunctions = [
        ProductCommentLikeInit,
        SetFormDataPageFragment,
        ProductCommentShowRepliesButtonInit,
        ProductEditCommentInit,
        ProductInfoReviewDynamicRatingInit,
        ProductPageCommentBoxInit,
        ProductRemoveCommentPopupInit,
        ProductReplyCommentInit,
        ProductPageCommentsPaginationInit,
        ProductCommentsOrderingInit
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