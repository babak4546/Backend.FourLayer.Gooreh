using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.Exceptions
{
    public class AddValidationException :Exception
    {
        public Dictionary<string, string>? Errors { get; }
        public AddValidationException(Dictionary<string, string> errors)
            : base("Add Validations Failed")
        {
            Errors = errors;
        }
    }
}
