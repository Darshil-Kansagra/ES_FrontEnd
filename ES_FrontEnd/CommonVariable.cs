namespace ES_FrontEnd
{
    public class CommonVariable
    {
        private static IHttpContextAccessor _httpContextAccessor;
        static CommonVariable()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public static int? UserId()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("UserId") == null)
            {
                return null;
            }
            return Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
        }
        public static string UserName()
        {
            if(_httpContextAccessor.HttpContext.Session.GetString("UserName")==null)
            {
                return null;
            }
            return _httpContextAccessor.HttpContext.Session.GetString("UserName");
        }
        public static string Email()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("Email") == null)
            {
                return null;
            }
            return _httpContextAccessor.HttpContext.Session.GetString("Email");
        }

        public static string Role()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("Role") == null)
            {
                return null;
            }
            return _httpContextAccessor.HttpContext.Session.GetString("Role");
        }
    }
}
