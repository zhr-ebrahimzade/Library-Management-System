using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper
{
    public static class ValidationHelper
    {
        public static void Validate(object dto)
        {
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                var errorMessages = new List<string>();
                foreach (var validationResult in validationResults)
                {
                    errorMessages.Add(validationResult.ErrorMessage);
                }

                throw new ArgumentException("DTO validation failed", string.Join(", ", errorMessages));
            }
        }
    }
}
