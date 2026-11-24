namespace GymManagementSystem_BLL.Validation
{
    public static class ValidationPatterns
    {
        public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string EgyptianMobile = @"^(010|011|012|015)\d{8}$";
        public const string Letters = @"^[a-zA-Z\u0600-\u06FF\s]+$";
        public const string LettersAndDigits = @"^[a-zA-Z\u0600-\u06FF0-9\s]+$";
    }
}