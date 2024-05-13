using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateField(string value, string fieldName, bool isRequired = false, bool canContainNumbers = true)
        {
            if (isRequired && string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{fieldName} is required.");
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!canContainNumbers && value.Any(char.IsDigit))
                {
                    throw new ArgumentException($"{fieldName} cannot contain numbers.");
                }

                if (fieldName == "Country" && !IsValidCountry(value))
                {
                    throw new ArgumentException($"{fieldName} must be a valid country in Europe.");
                }

                if (fieldName == "Postcode" && !IsValidPostcode(value))
                {
                    throw new ArgumentException($"{fieldName} cannot contain special symbols/characters.");
                }
            }
        }
        private static bool IsValidCountry(string country)
        {
            // List of European countries
            var europeanCountries = new List<string> { "Austria", "Belgium", "Bulgaria", "Croatia", "Cyprus", "Czech Republic", "Denmark", "Estonia", "Finland", "France", "Germany", "Greece", "Hungary", "Ireland", "Italy", "Latvia", "Lithuania", "Luxembourg", "Malta", "Netherlands", "Poland", "Portugal", "Romania", "Slovakia", "Slovenia", "Spain", "Sweden" };

           
            var formattedCountry = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(country.ToLower());

           
            return europeanCountries.Contains(formattedCountry);
        }

        private static bool IsValidPostcode(string value)
        {
            // Define special symbols/characters that are not allowed in the postcode
            var specialCharacters = new char[] { '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '=', '{', '}', '[', ']', '|', '\\', ';', ':', '\'', '"', '<', '>', ',', '.', '/', '?' };

            // Check if the postcode contains any of the special characters
            foreach (var character in specialCharacters)
            {
                if (value.Contains(character))
                {
                    return false;
                }
            }

            // If no special characters are found, the postcode is valid
            return true;
        }

        public static string FormatAddress(string line1, string line2)
        {
            int maxLength = 13; // Max length for each line
            string formattedAddress = "";

            // If Line1 exceeds maxLength, break it into new lines
            if (line1.Length > maxLength)
            {
                int index = 0;
                while (index < line1.Length)
                {
                    // Add maxLength characters or until the end of the string
                    int length = Math.Min(maxLength, line1.Length - index);
                    formattedAddress += line1.Substring(index, length);

                    // Add line break if Line1 continues
                    if (index + length < line1.Length)
                        formattedAddress += "\n";

                    index += length;
                }
            }
            else
            {
                formattedAddress = line1;
            }

            // Add Line2 if it exists
            if (!string.IsNullOrEmpty(line2))
            {
                // Add line break if Line1 exists
                if (!string.IsNullOrEmpty(formattedAddress))
                    formattedAddress += "\n";
                formattedAddress += line2;
            }

            return formattedAddress;
        }
    }
}
