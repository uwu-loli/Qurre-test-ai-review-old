using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using Qurre.Internal.Attributes;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities;

[PublicAPI]
public static class EntityManager
{
    private static readonly Dictionary<Object, IEntity> EntityMappings = new();
    private static readonly Dictionary<Type, Func<Object, IEntity>> EntityFactories = new();

    static EntityManager()
    {
        RegisterFactoriesFromAttributes();
    }

    private static void RegisterFactoriesFromAttributes()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<EntityWrapBindForFactory>();
            if (attribute != null) RegisterFactory(attribute.BaseType, type);
        }
    }

    private static void RegisterFactory(Type baseType, Type factoryType)
    {
        EntityFactories[baseType] = baseInstance =>
        {
            if (!baseType.IsAssignableFrom(baseInstance.GetType()))
                throw new InvalidOperationException($"baseInstance is not of type {baseType}");

#if DEBUG
            //Log.Debug($"Factory$$Create<{factoryType.Name}>({baseType.Name})");
#endif

            return (
                Activator.CreateInstance(
                    factoryType,
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    [baseInstance],
                    null
                ) as IEntity
            )!;
        };
    }

    public static TEntity? Get<TEntity>(Object baseObject)
        where TEntity : class, IEntity
    {
        if (baseObject == null)
            return null;

        if (EntityMappings.TryGetValue(baseObject, out var entity) && entity is TEntity typedEntity)
            return typedEntity;

        Func<Object, IEntity>? entityFactory = null;
        var baseType = baseObject.GetType();

#if DEBUG
        //Log.Debug($"EntitiesManager$$Get.BaseType = {baseType.Name}");
#endif

        while (baseType is not null)
        {
            if (EntityFactories.TryGetValue(baseType, out entityFactory))
                break;

            baseType = baseType.BaseType;
        }

        if (entityFactory is null)
            throw new InvalidOperationException($"No factory registered for type {baseType}");

#if DEBUG
        //Log.Info($"EntitiesManager$$Create<{typeof(TEntity).Name}>({baseObject.GetType().Name})");
#endif

        typedEntity = (TEntity)entityFactory(baseObject);
        EntityMappings[baseObject] = typedEntity;
        typedEntity.Destroyed += () => EntityMappings.Remove(baseObject);
        return typedEntity;
    }

    public static bool TryGet<TEntity>(Object baseObject, [NotNullWhen(true)] out TEntity? result)
        where TEntity : class, IEntity
    {
        result = Get<TEntity>(baseObject);
        return result is not null;
    }

    public static IReadOnlyCollection<TEntity> GetAll<TEntity>()
        where TEntity : class, IEntity
    {
        var result = new List<TEntity>();

        foreach (var entity in EntityMappings.Values)
            if (entity is TEntity typedEntity)
                result.Add(typedEntity);

        return result;
    }

    public static TEntity GetOrException<TEntity>(Object baseObject)
        where TEntity : class, IEntity
    {
        return Get<TEntity>(baseObject) ?? throw new ObjectDestroyedException();
    }
}