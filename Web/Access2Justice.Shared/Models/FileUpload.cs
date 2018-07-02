using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            //if (operation.OperationId.ToLower() == "apitopicscreatetopicuploadpost")
            if (operation.OperationId.ToLower(CultureInfo.CurrentCulture) == "apitopicscreatetopicsuploadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }

    public class FileUploadOperationResource : IOperationFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower(CultureInfo.CurrentCulture) == "apitopicscreateresourcesuploadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }

}
