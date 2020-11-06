namespace GymHub.Common
{
    public static class GlobalConstants
    {
        //Role constants
        public const string AdminRoleName = "Admin";
        public const string NormalUserRoleName = "Normal User";

        //Gender Constants
        public const string MaleGenderName = "Male";
        public const string FemaleGenderName = "Female";

        //User Validation Constants
        public const int UsernameLengthMin = 6;
        public const int UsernameLengthMax = 30;

        public const int PasswordLengthMin = 8;
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
    }
}
