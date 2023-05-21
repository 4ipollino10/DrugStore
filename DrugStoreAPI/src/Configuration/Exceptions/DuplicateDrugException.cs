namespace DrugStoreAPI.src.Configuration.Exceptions
{
    public class DuplicateDrugException : Exception
    {
        public DuplicateDrugException(string message) : base(message) { }
    }
}
