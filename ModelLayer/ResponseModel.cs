using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class ResponseModel<T>
    {
        public Boolean success { get; set; }
        public String message { get; set; }
        public T Data { get; set; }
    }
}

