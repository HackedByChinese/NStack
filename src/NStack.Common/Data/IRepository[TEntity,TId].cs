﻿#region header

// <copyright file="IRepository[TEntity,TId].cs" company="mikegrabski.com">
//    Copyright 2013 Mike Grabski
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

#endregion

using System;
using System.Linq;

namespace NStack.Data
{
    /// <summary>
    ///     A contract implemented by classes that act as a repository of objects.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entity the repository contains. </typeparam>
    /// <typeparam name="TId"> The type of the identifier property on the entity, used to identify a particular instance. </typeparam>
    public interface IRepository<TEntity, in TId> : IQueryable<TEntity>
    {
        /// <summary>
        ///     Gets the <see cref="IUnitOfWork" /> responsible for managing unit of work transactions.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        ///     Attaches the specified entity to the repository.
        /// </summary>
        /// <param name="entity"> The entity to attach. </param>
        void Add(TEntity entity);

        /// <summary>
        ///     Returns the entity with the specified <paramref name="id" /> from the repository.
        /// </summary>
        /// <param name="id"> The ID of the desired entity. </param>
        /// <returns>
        ///     The entity with the specified <paramref name="id" /> ; otherwise, null.
        /// </returns>
        TEntity Get(TId id);

        /// <summary>
        ///     Returns an instance of the entity with the specified <paramref name="id" /> , which can be used in operations where the object does not have to be fully loaded from the underlying store.
        /// </summary>
        /// <param name="id"> The ID of the desired entity. </param>
        /// <returns> The entity with the specified ID. </returns>
        /// <remarks>
        ///     During certain operations, it is not always useful or efficient to completely load an entity from the underlying store. For example, if you wish to remove an entity from the repository or use it to establish a relationship, you do not necessarily need the entire entity loaded from the underlying store. Instead a deferred or proxied instance of the entity, where at least the ID property is set, can be used to efficiently complete the operation. Preferrably, repositories should return an object that when any other member on the returned entity than Id, GetHashCode, or Equals is invoked, it should then load the object from the underlying store before continuing. DeferredGet differs from
        ///     <see
        ///         cref="Get" />
        ///     , in that <see cref="Get" /> should always return the entity after retrieving it from the underlying store. Finally, if this method is supported by the implementation, it should always return an entity with the specified ID set, even if an entity with that ID does not exist in the repository. The repository should wait to check for the existence of the specified entity when changes are actually made to the repository (such as when an item is deleted).
        /// </remarks>
        /// <exception cref="NotSupportedException">Deferred loading is not supported by the implementation.</exception>
        TEntity DeferredGet(TId id);

        /// <summary>
        ///     Removes the specified entity from the repository.
        /// </summary>
        /// <param name="entity"> The entity to remove. </param>
        void Remove(TEntity entity);

        /// <summary>
        ///     Returns the total number of entities in the repository.
        /// </summary>
        /// <returns> The total number of entities in the repository. </returns>
        int Count();

        /// <summary>
        ///     Ensures the repository watches for changes to the entity so changes can be persisted.
        /// </summary>
        /// <param name="entity"> The entity/ </param>
        void Attach(TEntity entity);

        /// <summary>
        ///     Prevents the repository from watching for changes to the entity, so the entity can be modified without changes being persisted.
        /// </summary>
        /// <param name="entity"> </param>
        void Detach(TEntity entity);

        /// <summary>
        ///     Refreshes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Refresh(TEntity entity);
    }
}