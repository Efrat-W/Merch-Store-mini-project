using System.Runtime.Serialization;

[Serializable]
public class MissingEntityException : Exception
{
    public MissingEntityException() : base() { }
    public MissingEntityException(string message) : base("MissingEntityException: " + message) { }
    public MissingEntityException(string message, Exception inner) : base("MissingEntityException: " + message, inner) { }
    protected MissingEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
        "MissingEntityException: Requested entity not found.";

}


[Serializable]
public class DoubledEntityException : Exception
{
    public DoubledEntityException() : base() { }
    public DoubledEntityException(string message) : base("DoubledEntityException: " + message) { }
    public DoubledEntityException(string message, Exception inner) : base("DoubledEntityException: " + message, inner) { }
    protected DoubledEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
        "DoubledEntityException: New entity already exists";

}

[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}
