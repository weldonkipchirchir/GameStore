using System;
using Microsoft.AspNetCore.Http;

namespace GameStore.Api.Endpoints
{
    public static class HttpResponseExtension
    {
        public static void AddPaginationHeader(this HttpResponse response, int pageSize, int totalCount)
        {
            var paginationHeader = new 
            {
                totalPages = (int)Math.Ceiling((double)totalCount / (double)pageSize),
            };

            response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationHeader));
        }
    }
}
