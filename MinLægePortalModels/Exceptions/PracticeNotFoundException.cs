using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Exceptions
{
    [Serializable]
    public class PracticeNotFoundException : Exception
    {
        public PracticeNotFoundException() { }
        public PracticeNotFoundException(string message) : base(message) { }
        public PracticeNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PracticeNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
