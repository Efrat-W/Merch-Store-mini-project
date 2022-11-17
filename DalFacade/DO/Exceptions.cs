using System.Runtime.Serialization;

[Serializable]
public class MissingEntityException : Exception
{
    public MissingEntityException() : base() { }
    public MissingEntityException(string message) : base(message) { }
    public MissingEntityException(string message, Exception inner) : base(message, inner) { }
    protected MissingEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() => 
        "Requested entity not found.";

}


[Serializable]
public class DoubledEntityException : Exception
{
    public DoubledEntityException() : base() { }
    public DoubledEntityException(string message) : base(message) { }
    public DoubledEntityException(string message, Exception inner) : base(message, inner) { }
    protected DoubledEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
        "New entity already exists";

}