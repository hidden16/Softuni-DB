namespace BookShop.Data.Models.Constants
{
    public static class GlobalConstant
    {
        public const int AUTHOR_FIRST_NAME_MIN_LENGTH = 3;
        public const int AUTHOR_FIRST_NAME_MAX_LENGTH = 30;
        public const int AUTHOR_LAST_NAME_MIN_LENGTH = 3;
        public const int AUTHOR_LAST_NAME_MAX_LENGTH = 30;
        public const string AUTHOR_PHONE_REGEX = @"^\d{3}-\d{3}-\d{4}$";
        public const int BOOK_NAME_MIN_LENGTH = 3;
        public const int BOOK_NAME_MAX_LENGTH = 30;
        public const int BOOK_PAGES_MIN_LENGTH = 3;
        public const int BOOK_PAGES_MAX_LENGTH = 30;
    }
}
