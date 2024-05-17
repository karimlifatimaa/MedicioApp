using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicio.Business.Exceptions
{
    public class PhotoFileNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public PhotoFileNotFoundException(string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
