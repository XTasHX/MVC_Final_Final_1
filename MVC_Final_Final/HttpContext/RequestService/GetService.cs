using System;
using MVC_Final_Final.Models.Docs;

namespace HttpContext.RequestService
{
    internal class GetService : DocsContext
    {
        private Type type;

        public GetService(Type type)
        {
            this.type = type;
        }
    }
}