
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.DataTransferObjects
{ 
    /**
     * record type is a reference type that provides synthesized implementations of Equals, GetHashCode, and ToString methods.
     */
    public record LinkParameters
    {

        public BookParameters  BookParameters { get; init; }

        public HttpContext HttpContext { get; init; }


    }
}
