using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Exceptions
{
    class ConsultationNotFoundException : Exception
    {
        public ConsultationNotFoundException() { }
        public ConsultationNotFoundException(string message) : base(message) { }
        public ConsultationNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected ConsultationNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }
}
