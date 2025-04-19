using System;
using JetBrains.Annotations;

namespace Qurre.API.Attributes;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse]
public class PluginEnable : Attribute;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse]
public class PluginDisable : Attribute;