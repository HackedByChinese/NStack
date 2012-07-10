﻿#region header

// <copyright file="DefaultNamingConvention.cs" company="mikegrabski.com">
//    Copyright 2012 Mike Grabski
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

using NHibernate.Mapping.ByCode;

using NStack.Extensions;

namespace NStack.Data
{
    /// <summary>
    /// The default implementation of <see cref="INamingConvention"/>.
    /// </summary>
    public class DefaultNamingConvention : INamingConvention
    {
        private const string KeyColumnFormat = "{0}_{1}";

        private const string ForeignKeyNameFormat = "fk_{0}_{1}_{2}";

        private const string IndexNameFormat = "ix_{0}_{1}";

        /// <summary>
        /// Returns the name of the table for the model.
        /// </summary>
        /// <param name="modelInspector">The model inspector.</param>
        /// <param name="type">The model type.</param>
        /// <returns>The name of the model's table.</returns>
        public string Table(IModelInspector modelInspector, Type type)
        {
            Requires.That(type, "type").IsNotNull();

            return type.Name.Pluralize().Underscore();
        }

        /// <summary>
        /// Returns the name of the table column for an entity's property.
        /// </summary>
        /// <param name="inspector">The model inspector.</param>
        /// <param name="member">The entity property.</param>
        /// <returns>The name of the table column.</returns>
        public string Column(IModelInspector inspector, PropertyPath member)
        {
            Requires.That(member, "member").IsNotNull();
            Requires.That(member.LocalMember, "member.LocalMember").IsNotNull();

            var localName = member.LocalMember.Name.Underscore();

            var type = member.LocalMember.GetPropertyOrFieldType();

            if (inspector.IsEntity(type)) // is a foreign key
            {
                var id = inspector.FindPersistentId(type);

                if (id != null) return KeyColumnFormat.Formatted(localName, Column(inspector, id));
            }

            return localName;
        }



        /// <summary>
        /// Returns the name of the foreign key in a relationship.
        /// </summary>
        /// <param name="inspector">The model inspector.</param>
        /// <param name="member">The entity property.</param>
        /// <returns>The name of the foreign key.</returns>
        public string ForeignKey(IModelInspector inspector, PropertyPath member)
        {
            Requires.That(member, "member").IsNotNull();
            Requires.That(member.LocalMember, "member.LocalMember").IsNotNull();

            return ForeignKeyNameFormat.Formatted(Table(inspector, member.LocalMember.GetPropertyOrFieldType()),
                                              Table(inspector, member.LocalMember.DeclaringType),
                                              Column(inspector, member));
        }

        /// <summary>
        /// Returns the name of the index for the column.
        /// </summary>
        /// <param name="inspector">The model inspector.</param>
        /// <param name="member">The entity property.</param>
        /// <returns>The name of the index.</returns>
        public string Index(IModelInspector inspector, PropertyPath member)
        {
            Requires.That(member, "member").IsNotNull();
            Requires.That(member.LocalMember, "member.LocalMember").IsNotNull();

            return IndexNameFormat.Formatted(Table(inspector, member.LocalMember.DeclaringType),
                                             Column(inspector, member));
        }

        /// <summary>
        /// Returns the name of a key column.
        /// </summary>
        /// <param name="inspector">The model inspector.</param>
        /// <param name="member"></param>
        /// <returns></returns>
        public string KeyColumn(IModelInspector inspector, PropertyPath member)
        {
            Requires.That(member, "member").IsNotNull();
            Requires.That(member, "member.LocalMember").IsNotNull();

            var type = member.LocalMember.DeclaringType;
            var localName = type.Name.Underscore();
            var id = inspector.FindPersistentId(type);

            return KeyColumnFormat.Formatted(localName, id.LocalMember.Name.Underscore());
        }
    }
}