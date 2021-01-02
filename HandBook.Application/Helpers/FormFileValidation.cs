using Microsoft.AspNetCore.Http;
using FluentValidation.Validators;

namespace HandBook.Application.Helpers
{
    public class FormFileValidation : PropertyValidator
    {
        public FormFileValidation() : base("File format should be image/jpeg, image/jpg or image/png type.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var property = context.PropertyValue as IFormFile;
            if (property == null)
                return false;

            if ((property.ContentType.ToLower() == "image/jpeg" ||
                 property.ContentType.ToLower() == "image/jpg" ||
                 property.ContentType.ToLower() == "image/png") &&
                 property.Length > 0)
                return true;

            return false;
        }
    }
}
