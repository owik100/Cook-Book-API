//using Microsoft.OpenApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Swashbuckle.Swagger;
//using System.Web.Http.Description;

//namespace Cook_Book_API.Helpers
//{
//    public class AuthorizationOperationFilter : IOperationFilter
//    {
//        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
//        {
//            if (operation.parameters == null)
//            {
//                operation.parameters = new List<Parameter>();
//            }

//            operation.parameters.Add(new Parameter
//            {
//                name = "Authorization",
//                @in = "header",
//                description = "access token",
//                required = false,
//                type = "string"
//            });
//        }
//    }
//}
