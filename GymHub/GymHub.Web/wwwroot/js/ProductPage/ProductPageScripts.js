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
        ProductPageBuyingQuantityInit,
        RemoveProductInit,
        SetUrlCommentsOrderingOption
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