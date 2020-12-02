namespace Orpheus.Utilities
{
    public class ThemeSelector
    {
        public (string, string, string) SelectTheme(string ThemeName)
        {
            (string, string, string) vals = ("", "", "");
            switch (ThemeName)
            {
                case "Default (Light)":
                    {
                        vals.Item1 = "#000000";
                        vals.Item2 = "#FFFFFF";
                        vals.Item3 = "#FF0000";
                        break;
                    }
                case "Luna":
                    {
                        vals.Item1 = "#00FFFF";
                        vals.Item2 = "#0000FF";
                        vals.Item3 = "#00FF00";
                        break;
                    }
                case "Candyland":
                    {
                        vals.Item1 = "#ADD8E6";
                        vals.Item2 = "#FFC0CB";
                        vals.Item3 = "#EE82EE";
                        break;
                    }
                case "Thanksgiving":
                    {
                        vals.Item1 = "#FFA500";
                        vals.Item2 = "#A52A2A";
                        vals.Item3 = "#FFFF00";
                        break;
                    }
                case "Christmas":
                    {
                        vals.Item1 = "#FFFF00";
                        vals.Item2 = "#00FF00";
                        vals.Item3 = "#FF0000";
                        break;
                    }
                case "Reveille (Light)":
                    {
                        vals.Item1 = "#500000";
                        vals.Item2 = "#FFFFFF";
                        vals.Item3 = "#500000";
                        break;
                    }
                case "Reveille (Dark)":
                    {
                        vals.Item1 = "#500000";
                        vals.Item2 = "#999999";
                        vals.Item3 = "#500000";
                        break;
                    }
                case "Hunter (Light)":
                    {
                        vals.Item1 = "#018744";
                        vals.Item2 = "#FFFFFF";
                        vals.Item3 = "#C24444";
                        break;
                    }
                case "Hunter (Dark)":
                    {
                        vals.Item1 = "#018744";
                        vals.Item2 = "#999999";
                        vals.Item3 = "#C24444";
                        break;
                    }
                case "Classic":
                    {
                        vals.Item1 = "#234508";
                        vals.Item2 = "#DDA0DD";
                        vals.Item3 = "#C67676";
                        break;
                    }
                case "Classic Gold":
                    {
                        vals.Item1 = "#234656";
                        vals.Item2 = "#8F7034";
                        vals.Item3 = "#C67878";
                        break;
                    }
                case "Classic Teal":
                    {
                        vals.Item1 = "#234444";
                        vals.Item2 = "#038387";
                        vals.Item3 = "#C67171";
                        break;
                    }
                case "Classic Plum":
                    {
                        vals.Item1 = "#234848";
                        vals.Item2 = "#854085";
                        vals.Item3 = "#C67879";
                        break;
                    }
                case "Cool Grey":
                    {
                        vals.Item1 = "#233147";
                        vals.Item2 = "#737373";
                        vals.Item3 = "#C67870";
                        break;
                    }
                case "Warm Grey":
                    {
                        vals.Item1 = "#234348";
                        vals.Item2 = "#867365";
                        vals.Item3 = "#C57278";
                        break;
                    }
                case "Default (Dark)":
                default:
                    {
                        vals.Item1 = "#FFFFFF";
                        vals.Item2 = "#999999";
                        vals.Item3 = "#FF0000";
                        break;
                    }
            }
            return vals;
        }
    }
}
