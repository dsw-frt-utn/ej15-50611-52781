using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        protected BaseEntity(Guid? id = null) 
        { 
            id = id ?? Guid.NewGuid(); //Id si no es null. NewGuid si es null
        }
    }
}
