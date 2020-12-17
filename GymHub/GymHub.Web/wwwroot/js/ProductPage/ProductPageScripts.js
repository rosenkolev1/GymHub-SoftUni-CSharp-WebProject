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