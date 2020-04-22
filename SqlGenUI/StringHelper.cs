namespace SqlGenUI
{
    public static class StringHelper
    {
        public static string EmptyCheck(this string variable)
        {
            return variable == null ? "" : variable;
        }
    }
}
