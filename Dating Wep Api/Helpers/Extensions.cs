using System.Runtime.CompilerServices;

namespace Dating_Wep_Api.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Haders", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalcuateAge(this DateTime DOB)
        {
            var age = DateTime.Today.Year - DOB.Year;
            if (DOB.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}
