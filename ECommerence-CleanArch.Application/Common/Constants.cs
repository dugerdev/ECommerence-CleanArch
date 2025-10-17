namespace ECommerence_CleanArch.Application.Common;

public static class Constants
{
    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int MinPageSize = 1;
    }

    public static class Cache
    {
        public const int DefaultCacheExpirationMinutes = 30;
        public const int LongCacheExpirationMinutes = 60;
    }

    public static class Validation
    {
        public const int MaxNameLenght = 100;
        public const int MaxDscriptionLenght = 500;
        public const int MaxEmailLenght = 100;
        public const int MaxPhoneLenght = 20;
    }
}
