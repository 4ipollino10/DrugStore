namespace DrugStoreAPI.Exceptions
{
    public class DrugNotFoundException : Exception
    {
        public DrugNotFoundException(string message) : base(message) { }

    }
}
