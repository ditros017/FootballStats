using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }

    public abstract class EntityBase : IEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsNew()
        {
            return EntityExtensions.IsNew(this);
        }
    }

    public static class EntityExtensions
    {
        public static bool IsNew(this IEntity self)
        {
            return self.Id == 0;
        }
    }
}