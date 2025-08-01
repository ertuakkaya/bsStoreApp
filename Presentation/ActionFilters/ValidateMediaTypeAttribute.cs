﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext context)
        {

            // Accept header var mı kontrol et
            var acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");


            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult("Accept header is missing!");
                return;
            }



            // Accept header varsa media type'ı kontrol et
            var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not acceptable! Please add Accept header with required media type!");
            }

            // Accept header varsa media type'ı context'e ekle
            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);


        }

    }
}
