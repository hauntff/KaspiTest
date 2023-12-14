using System.Globalization;

namespace KaspiTest.Helpers
{
    public static class ParsingHelper
    {
        public static List<string> GetLinks(string html)
        {
            if (html == null || html.Length == 0)
            {
                throw new ArgumentNullException(nameof(html));
            }
            var links = new List<string>();
            //class="tn-link"
            for (int i = 0; i < html.Length - 15; i++)
            {
                if (html[i] == 'c' && html[i + 1] == 'l' && html[i + 2] == 'a' && html[i + 3] == 's' && html[i + 4] == 's' && html[i + 5] == '=' && html[i + 6] == '"' && html[i + 7] == 't' && html[i + 8] == 'n' && html[i + 9] == '-' && html[i + 10] == 'l' && html[i + 11] == 'i' && html[i + 12] == 'n' && html[i + 13] == 'k' && html[i + 14] == '"')
                {
                    //<a href="
                    for (int j = i; j >= 0; j--)
                    {
                        if (html[j] == '<' && html[j + 1] == 'a' && html[j + 2] == ' ' && html[j + 3] == 'h' && html[j + 4] == 'r' && html[j + 5] == 'e' && html[j + 6] == 'f' && html[j + 7] == '=' && html[j + 8] == '"')
                        {
                            links.Add(html.Substring(j + 9, i - j - 12));
                            break;
                        }
                    }
                }
            }
            return links;
        }

        public static DateTime ParseDate(string date)
        {
            DateTime currentDate = DateTime.Now.Date;

            if (date.StartsWith("Сегодня"))
            {
                string timeString = date.Substring("Сегодня, ".Length);
                TimeSpan time = TimeSpan.Parse(timeString);
                return DateTime.Today.Add(time);
            }
            else if (date.StartsWith("Вчера"))
            {
                string timeString = date.Substring("Вчера, ".Length);
                TimeSpan time = TimeSpan.Parse(timeString);
                return DateTime.Today.AddDays(-1).Add(time);
            }
            else
            {
                // Обработка других вариантов (если нужно)
                return DateTime.ParseExact(date, "dd MMMM yyyy, HH:mm", CultureInfo.GetCultureInfo("ru-RU"));
            }
        }
    }
}
