namespace BookAuthor.Api.Model.Interfaces
{
    public interface IApproved
    {
        public bool Approved { get; set; }
        // when a non admin user creates any object that implements this interface
        // it will wait for approval before being in any result of the api call
    }
}
