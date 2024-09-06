namespace PseRanNum.Handlers
{
    internal static class Validator
    {
        public static string Validate(string i, string m, string a, string c, string x0)
        {
            if (!long.TryParse(i, out _) || Int64.Parse(i) < 0)
            {
                return "Кількість ітерацій не вірна!";
            }

            if (!long.TryParse(m, out _) || Int64.Parse(m) <= 0)
            {
                return "Значення модуля порівння не вірне";
            }

            if (!long.TryParse(a, out _) || Int64.Parse(a) < 0 || Int64.Parse(a) >= Int64.Parse(m))
            {
                return "Значення множника не вірне";
            }

            if (!long.TryParse(c, out _) || Int64.Parse(c) < 0 || Int64.Parse(c) >= Int64.Parse(m))
            {
                return "Значення приросту не вірне";
            }

            if (!long.TryParse(x0, out _) || Int64.Parse(c) < 0 || Int64.Parse(x0) >= Int64.Parse(m))
            {
                return "Значення початкового числа не вірне";
            }

            return string.Empty;
        }
    }
}
