namespace DrugStoreAPI.Exceptions
{
    [Serializable]
    public class ComponentRelationshipException : Exception { 
        public ComponentRelationshipException(string message) 
            : base(message) {}
    }
}
