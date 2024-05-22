namespace Mango.Web.Utility
{
    public static class SD
    {
        public static string CouponApiBase { get; set; }
        public static string AuthApiBase { get; set; }

        public static string RoleAdmin = "Admin";
        public static string RoleCustom = "Custom";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3
        }
    }
}
