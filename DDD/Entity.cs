
namespace DDD
{
    /// In DDD terms, entities are objects that have a conceptual identity. For example, two "Person" objects 
    /// are not the same object just because they have all the same properties. In such cases, Entities have
    /// some for of unique identifier. 
    /// See: http://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/

    public interface IEntity
    {
        string Id { get; set; }
    }

    public abstract class Entity : IEntity
    {
        public string Id { get; set; }

        public Entity()
        {
            Id = Base62.NewBase62Guid();
        }
    }
}
