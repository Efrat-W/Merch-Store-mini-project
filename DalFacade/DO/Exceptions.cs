using System.Runtime.Serialization;
namespace DO;

[Serializable]
public class MissingEntityException : Exception
{
    public MissingEntityException() : base() { }
    public MissingEntityException(string message) : base( message) { }
    public MissingEntityException(string message, Exception inner) : base( message, inner) { }
    protected MissingEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
        "MissingEntityException: Requested entity not found.";

}


[Serializable]
public class DoubledEntityException : Exception
{
    public DoubledEntityException() : base() { }
    public DoubledEntityException(string message) : base(  message) { }
    public DoubledEntityException(string message, Exception inner) : base( message, inner) { }
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


[Serializable]
public class XMLFileLoadCreateException : Exception
{
    private string filePath;
    private string v;
    Exception ex;
    public XMLFileLoadCreateException(string msg) : base(msg) { }
    public XMLFileLoadCreateException(string filePath, string v, Exception ex)
    {
        this.filePath = filePath;
        this.v = v;
        this.ex = ex;
    }
}
