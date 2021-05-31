using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml.Serialization;

namespace RESTServices.Models
{
    public interface ICRUD<T>
    {
        object Create(T entity);
        T Get(object var);
        IEnumerable<T> GetAll();
        bool Update(T entity);
        object Delete(object var);
    }
}