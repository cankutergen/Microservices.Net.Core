using System;
using System.Collections.Generic;
using System.Text;

namespace Tesodev.Core.Entities
{
    public interface IResponsable
    {
        public int Id { get; }

        public string Message { get; set; }
    }
}
