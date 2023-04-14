namespace DrugStoreAPI.Exceptions
{
    public class OrderBadRequestException : Exception
    {
        public OrderBadRequestException(string message) : base(message) { }

    }
}
