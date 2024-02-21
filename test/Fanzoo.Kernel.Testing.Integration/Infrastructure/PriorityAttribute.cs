using System;

namespace Fanzoo.Kernel.Testing
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PriorityAttribute(int priority) : Attribute
    {
        public int Priority { get; init; } = priority;
    }
}
