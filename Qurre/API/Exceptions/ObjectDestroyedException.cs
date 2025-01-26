using System;
using JetBrains.Annotations;

namespace Qurre.API.Exceptions;

[PublicAPI]
public class ObjectDestroyedException(string objectName = "") : InvalidOperationException($"Object{(objectName != "" ? $" \"{objectName}\")" : "")} was destroyed")
{
    
}
