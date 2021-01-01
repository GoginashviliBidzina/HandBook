using FluentValidation.Validators;

namespace HandBook.Application.Helpers
{
    public class LanguageValidation : PropertyValidator
    {
        public static char[] GeorgianLetters = "აბგდევზთიკლმნოპჟრსტუფქღყშჩძწჭხჯჰ".ToCharArray();
        public static char[] EnglishLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public LanguageValidation() : base("Only Georgian and Latin letters allowed")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var property = context.PropertyValue as string;

            var containsGeorgian = false;
            var containsLatin = false;

            foreach (var georgianLetter in GeorgianLetters)
            {
                if (property.Contains(georgianLetter))
                {
                    containsGeorgian = true;
                    break;
                }
            }

            foreach (var englishLetter in EnglishLetters)
            {
                if (property.Contains(englishLetter))
                {
                    containsLatin = true;
                    break;
                }
            }

            var valid = containsLatin ^ containsGeorgian;

            return valid ? true : false;
        }
    }
}
