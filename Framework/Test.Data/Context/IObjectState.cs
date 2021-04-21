
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Data.Context
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}