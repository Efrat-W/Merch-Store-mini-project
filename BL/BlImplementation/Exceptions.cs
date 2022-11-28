using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

using System.Runtime.Serialization;

[Serializable]
//exception class for invalid data
public class InvalidArgumentException : Exception
{
    public InvalidArgumentException() : base() { }
    public InvalidArgumentException(string message) : base("InvalidArgumentException: " + message) { }
    public InvalidArgumentException(Exception inner) : base("InvalidArgumentException caused by ", inner) { }
    public InvalidArgumentException(string message, Exception inner) : base("InvalidArgumentException " + message + " caused by ", inner) { }
    protected InvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
    "InvalidArgumentException: Invalid data argument. ";
}

//exception class for data that does not exists
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() : base() { }
    //public EntityNotFoundException(string message) : base("EntityNotFoundException: " + message) { }
    public EntityNotFoundException(string message) : base(message) { }
    public EntityNotFoundException(string message, Exception inner) : base("EntityNotFoundException " + message + " caused by ", inner) { }
    public EntityNotFoundException(Exception inner) : base("EntityNotFoundException caused by ", inner) { }
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
    "EntityNotFoundException: The requested entity doesn't exist.\n";
}

[Serializable]
//exception class for invalid dates
public class InvalidDateException : Exception
{
    public InvalidDateException() : base() { }
    public InvalidDateException(string message) : base("InvalidDateException: " + message) { }
    public InvalidDateException(string message, Exception inner) : base(message, inner) { }
    protected InvalidDateException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
    "InvalidDateException: Invalid date. \n";
}
