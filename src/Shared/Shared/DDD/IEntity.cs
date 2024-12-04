namespace Shared.DDD
{

    public interface IEntity<T> : IEntity
    { 
        public T Id { get; set; } // generic property representing the Identifier
    }
    
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }  
        public string? CreatedBy { get; set; }
        public DateTime? LastModified {  get; set; }    
        public string? LastModifiedBy { get; set;}
    }
}
