using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }

        protected BaseEntity(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
    }
}
