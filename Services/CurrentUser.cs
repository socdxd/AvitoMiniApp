using AvitoMiniApp.Models;

namespace AvitoMiniApp.Services
{
    public static class CurrentUser
    {
        public static User? User { get; set; }

        public static bool IsAuthenticated => User != null;
    }
}
