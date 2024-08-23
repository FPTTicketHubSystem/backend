namespace backend.Helper
{
    public class StringHelpers
    {
        public static string NormalizeSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string trimmedInput = input.Trim();

            string[] words = trimmedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words);
        }
    }
}
