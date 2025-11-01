namespace Camden_Car_Park.Common.Utilities
{
    public static class RegexPatterns
    {
        public const string UkVehicleRegistrationNumber = @"^(?:[A-Z]{2}\d{2}\s?[A-Z]{3}|[A-Z]\d{1,3}\s?[A-Z]{3}|[A-Z]{3}\s?\d{1,3}[A-Z]?|[A-Z]{1,3}\s?\d{1,4}|\d{1,4}\s?[A-Z]{1,3}|[A-Z]{1,2}\s?\d{1,4}\s?[A-Z]{1,3})$";
    }
}
