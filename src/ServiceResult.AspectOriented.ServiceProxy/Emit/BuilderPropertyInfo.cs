using System;

namespace ServiceResult.AspectOriented.ServiceProxy.Emit
{
    public class BuilderPropertyInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsInterfaceImplementation { get; set; }
    }
}
