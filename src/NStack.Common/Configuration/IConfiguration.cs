﻿#region header
// <copyright file="IConfiguration.cs" company="mikegrabski.com">
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
using System.ComponentModel;

using NStack.Fluent;

namespace NStack.Configuration
{
    /// <summary>
    /// A contract for types that build an application's configuration.
    /// </summary>
    public interface IConfiguration : IFluent
    {
        /// <summary>
        /// Fluently configures an aspect of an application.
        /// </summary>
        /// <typeparam name="T">The aspect type.</typeparam>
        /// <param name="config">The optional delegate that will be invoked to configure details of the aspect.</param>
        /// <returns>The current configuration.</returns>
        IConfiguration Aspect<T>(Action<T> config = null)
            where T : IConfigurationAspect;

        /// <summary>
        /// Gets the <see cref="IContainerAdapter"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerAdapter ContainerAdapter { get; }
    }
}