﻿namespace MembershipSystem.Composite.Composite
{
    public interface IComponent
    {
        int Id { get; set; }
        string Name { get; set; }

        int Count();
        string Display();
    }
}
