using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace NAIKI.Backbone
{
    public class ConnectionString
    {

        private static String _Naiki = String.Empty;
        public static String SerBeeConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_Naiki))
                {
                    string userName = WebConfigurationManager.AppSettings["DB"];

                    _Naiki = userName;
                }

                var result = _Naiki;



                return result;

            }
        }

    }
}
