using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Shared
{
    public class GeneralHelper
    {
        public static string GetAnonymousTitle(int alertType)
        {
            var title = "Mascota ";

            switch (alertType)
            {
                case 1:
                    title += "Perdida";
                    break;
                case 2:
                    title += "Abandonada";
                    break;
                case 3:
                    title += "Herida";
                    break;
                case 4:
                    title += "Encontrada";
                    break;
                case 5:
                    title += "En Adopcion";
                    break;
            }

            return title;
        }
    }
}