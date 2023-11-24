namespace MySpot.Core.Exceptions
{
    public class InvalidEntityIdException : CustomExcption
    {
        public Guid Id { get; set; }

        public InvalidEntityIdException(Guid id) : base($"Entity id: {id} is invalid.")
        {
            Id = id;
        }
    }
}
