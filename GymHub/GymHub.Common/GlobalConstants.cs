﻿namespace GymHub.Common
{
    public static class GlobalConstants
    {
        /*IMPORTANT:
         * WHEN CHANGING VALIDATION CONSTANTS CHANGE ERROR MESSAGES IN MODELS
        */

        //Site metadata
        public const string SystemName = "GymHub.com";
        public const string SiteName = "GymHub";
        public const string CompanyAddress = "455 Foggy Heights, AZ 85004, US";
        public const string CompanyContactNumber = "(123) 456-789";
        public const string CompanyContactEmail = "rosenandreevkolev1@gmail.com";
        public const string PathToSiteLogo = "/GymHub/GymHub logo.png";

        //Errors from POST request temp data
        public const string ErrorsFromPOSTRequest = "ErrorsFromPOSTRequest";

        //Input Model from POST request temp data
        public const string InputModelFromPOSTRequest = "InputModelFromPOSTRequest";
        public const string InputModelFromPOSTRequestType = "InputModelFromPOSTRequestType";

        //Notification temp data
        public const string NotificationText = "NotificationText";
        public const string NotificationType = "NotificationType";

        //Role constants
        public const string AdminRoleName = "Admin";
        public const string NormalUserRoleName = "Normal User";

        //Gender Constants
        public const string MaleGenderName = "Male";
        public const string FemaleGenderName = "Female";

        //User Validation Constants
        public const int UsernameLengthMin = 6;
        public const int UsernameLengthMax = 30;

        //Min length should be 8. Any other value is for debugging purposes only
        public const int PasswordLengthMin = 6;
        public const int PasswordLengthMax = 30;

        public const int FirstNameLengthMin = 2;
        public const int FirstNameLengthMax = 30;

        public const int LastNameLengthMin = 2;
        public const int LastNameLengthMax = 30;

        public const int MiddleNameLengthMax = 30;

        public const int EmailLengthMax = 60;
        public const int EmailLengthMin = 4;

        //Product Validation Constants
        public const int ProductNameLengthMin = 3;
        public const int ProductNameLengthMax = 100;

        //Product Comments Pagination DEFAULT value is 8
        public const int CommentsPerPage = 1;

        //Default comment removal email text
        public const string CommentRemovalJustificationText = "Hello, {username}, your comment for this product: {productName:model} has been deleted because it was deemed against our TOS policy.";

        //CheckoutViewModelInfo for checkout
        public const string CheckoutViewModelInfo = "CheckoutViewModelInfo";

        //Stripe paymentIntentId
        public const string PaymentIntentId = "PaymentIntentId";

        //CartBuyButtonErrorForQuantity
        public const string CartBuyButtonErrorForQuantity = "CartBuyButtonErrorForQuantity";
        //PaymentMethods
        public const string DebitOrCreditCard = "Debit or credit card";
        public const string Paypal = "Paypal";
        public const string DirectBankTransfer = "Direct Bank Transfer";
        public const string CashOnDelivery = "Cash on Delivery";

        //Sale temp data
        public const string SaleInfo = "SaleInfo";
        public const string SaleProductsInfo = "SaleProductsInfo";
        public const string ConfirmSaleToken = "ConfirmSaleToken";

        //Sale statuses
        public const string PendingSaleStatus = "Pending";
        public const string ConfirmedSaleStatus = "Confirmed";
        public const string DeclinedSaleStatus = "Declined";
    }
}
