using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Common
{
    public class ResponseViewModel<T>
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
