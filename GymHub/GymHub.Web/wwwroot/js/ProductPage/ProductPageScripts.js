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
        ProductCommentsOrderingInit,
        ProductPageBuyingQuantityInit
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