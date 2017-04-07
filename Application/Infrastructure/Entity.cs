// <copyright file="Entity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System;

    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}
