namespace Halcyon.Api.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string str, object obj)
        {
            var result = str;

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj, null).ToString();
                result = result.Replace($"{{{property.Name}}}", value);
            }

            return result;
        }
    }
}
