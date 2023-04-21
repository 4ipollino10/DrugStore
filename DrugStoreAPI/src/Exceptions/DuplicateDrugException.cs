namespace DrugStoreAPI.Exceptions
{
    public class DuplicateDrugException : Exception
    {
        public DuplicateDrugException(string message) : base(message) { }
    }
}
