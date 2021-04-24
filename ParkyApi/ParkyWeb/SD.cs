using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string ApiBaseUrl = "http://localhost:33591/";
        public static string NationalParkUrl = ApiBaseUrl + "api/v1/nationalPark/";        
        public static string TrailUrl = ApiBaseUrl + "api/v1/trail/";
        public static string AccountUrl = ApiBaseUrl + "api/v1/users/";

    }
}
