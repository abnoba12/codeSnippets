using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace CorporateAdminV2.Validation.FileAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class FileAttribute : ValidationAttribute
    {
        private int maxFileSizeKB = 0;
        private string[] AllowedFileExtensions;

        public FileAttribute(){}

        /// <summary>
        /// Maximum file size for uploaded file in kilobytes
        /// </summary>
        /// <param name="maxFileSizeKB"></param>
        public FileAttribute(int maxFileSizeKB){
            this.maxFileSizeKB = maxFileSizeKB;
        }

        /// <summary>
        /// Allowed file extentions 
        /// </summary>
        /// <param name="AllowedFileExtensions"></param>
        public FileAttribute(string[] AllowedFileExtensions){
            this.AllowedFileExtensions = AllowedFileExtensions;
        }

        //Usage Example:
        //[Display(Name = "File")]
        //[File(51200, ErrorMessage = "Your file is too large. Max file size is 50MB")]
        //[File(new string[] {".pdf", ".jpg", ".jpeg", ".gif", ".png", ".tiff", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx"}, ErrorMessage = "Invalid file type")]
        //public HttpPostedFileBase file { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;

            if (file != null)
            {
                if (AllowedFileExtensions != null && !AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                }
                else if (maxFileSizeKB != 0 && (file.ContentLength / 1024) > maxFileSizeKB)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                }
            }
                
            return ValidationResult.Success;
        }  
    }
}