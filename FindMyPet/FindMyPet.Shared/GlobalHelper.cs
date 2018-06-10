using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.Shared
{
    public interface IGlobalHelper
    {
        string GetAppSettingByEnvironment(string key);
        string GetConnectionStringByEnvironment(string key);
    }

    public class GlobalHelper : IGlobalHelper
    {
        public string GetAppSettingByEnvironment(string key)
        {
            var env = ConfigurationManager.AppSettings["Environment"].ToString();
            EnvironmentEnum myEnvironment = (EnvironmentEnum)Enum.Parse(typeof(EnvironmentEnum), env, true);

            switch (myEnvironment)
            {
                case EnvironmentEnum.Local: break;
                case EnvironmentEnum.Development:
                    key = $"{key}Dev";
                    break;
                case EnvironmentEnum.Production:
                    key = $"{key}Prod";
                    break;
            }

            return ConfigurationManager.AppSettings[key].ToString();
        }

        public string GetConnectionStringByEnvironment(string key)
        {
            var env = ConfigurationManager.AppSettings["Environment"].ToString();
            EnvironmentEnum myEnvironment = (EnvironmentEnum)Enum.Parse(typeof(EnvironmentEnum), env, true);

            switch (myEnvironment)
            {
                case EnvironmentEnum.Local: break;
                case EnvironmentEnum.Development:
                    key = $"{key}Dev";
                    break;
                case EnvironmentEnum.Production:
                    key = $"{key}Prod";
                    break;
            }

            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
