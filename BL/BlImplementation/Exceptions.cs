using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

using System.Runtime.Serialization;

[Serializable]
public class InvalidDataException : Exception
{
    public InvalidDataException() : base() { }
    public InvalidDataException(string message) : base(message) { }
    public InvalidDataException(string message, Exception inner) : base(message, inner) { }
    protected InvalidDataException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
    "InvalidDataException: Invalid data. \n";
}

[Serializable]
public class InvalidDateException : Exception
{
    public InvalidDateException() : base() { }
    public InvalidDateException(string message) : base(message) { }
    public InvalidDateException(string message, Exception inner) : base(message, inner) { }
    protected InvalidDateException(SerializationInfo info, StreamingContext context) : base(info, context) { } // special constructor for our custom exception

    override public string ToString() =>
    "InvalidDateException: Incorrect date. \n";
}