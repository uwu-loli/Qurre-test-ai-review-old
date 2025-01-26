using System;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.Internal.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[MeansImplicitUse(ImplicitUseTargetFlags.Itself)]
internal class EntityWrapBindForFactory(Type baseType) : Attribute
{
    public Type BaseType { get; } = baseType;
}