using System;
using JetBrains.Annotations;

namespace Qurre.API.Exceptions;

[PublicAPI]
public class ObjectDestroyedException() : InvalidOperationException("Object was destroyed")
{
    
}
