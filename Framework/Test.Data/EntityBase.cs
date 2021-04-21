using Test.Data.Context;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Data
{
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}