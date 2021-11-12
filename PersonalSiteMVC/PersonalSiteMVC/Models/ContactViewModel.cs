using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//Add for server-side validation

namespace PersonalSiteMVC.Models

{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Name is required")] //[Required] makes the field required & ErrorMessage can be whatever we want
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email is required")]
        [DataType(DataType.EmailAddress)] //[DataType(DataType.EmailAddress)] pattern matches the input to ensure it is a valid email
        //[RegularExpression()] is similar and allows the use of RegEx patter matching
        public string Email { get; set; }

        //Optional if you would like to require subject
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required")]
        [DataType(DataType.MultilineText)]
        //[UIHint("MultilineText")] makes any <input> elements for this property into <textarea>
        public string Message { get; set; }


    }
}