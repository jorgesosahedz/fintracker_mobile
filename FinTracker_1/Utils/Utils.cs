using System;
using System.Collections.Generic;
using System.Text;

namespace FinTracker_1.Utils
{
    class Utils
    {
        public static string getMonth(int month)
        {
            string montht_str = "";
            switch (month)
            {
                case 1:
                    montht_str = "Jan";
                    break;
                case 2:
                    montht_str = "Feb";
                    break;
                case 3:
                    montht_str = "Mar";
                    break;
                case 4:
                    montht_str = "Apr";
                    break;
                case 5:
                    montht_str = "May";
                    break;
                case 6:
                    montht_str = "Jun";
                    break;
                case 7:
                    montht_str = "Jul";
                    break;
                case 8:
                    montht_str = "Aug";
                    break;
                case 9:
                    montht_str = "Sep";
                    break;
                case 10:
                    montht_str = "Oct";
                    break;
                case 11:
                    montht_str = "Nov";
                    break;
                case 12:
                    montht_str = "Dec";
                    break;
            }

            return montht_str;
        }

        public static string getColorSelection(int color)
        {
            string color_str = "";
            switch (color)
            {
                case 1:
                    color_str = "#b23b5b";
                    break;
                case 2:
                    color_str = "#c5c5f1";
                    break;
                case 3:
                    color_str = "#6464c6";
                    break;
                case 4:
                    color_str = "#32cd8e";
                    break;
                case 5:
                    color_str = "#3d90d1";
                    break;
                case 6:
                    color_str = "#cd1aa4";
                    break;
                case 7:
                    color_str = "#009000";
                    break;
                case 8:
                    color_str = "#eaea8f";
                    break;
                case 9:
                    color_str = "#FFB500";
                    break;
                case 10:
                    color_str = "#FFDA9C";
                    break;
                case 11:
                    color_str = "#005356";
                    break;
                case 12:
                    color_str = "#addd10";
                    break;
                case 13:
                    color_str = "#b8e4dc";
                    break;
            }
            return color_str;
        }
    }
}
