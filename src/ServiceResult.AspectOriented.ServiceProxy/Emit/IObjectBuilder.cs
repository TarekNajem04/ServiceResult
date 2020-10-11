using System;

namespace ServiceResult.AspectOriented.ServiceProxy.Emit
{
    public interface IObjectBuilder
    {
        object CreateObject(string name, BuilderPropertyInfo[] properties = null, Type baseClass = null, Type[] interfaces = null, bool autoGenerateInterfaceproperties = false);

        TInterface CreateObject<TBase, TInterface>(string name, BuilderPropertyInfo[] properties = null) where TBase : class, new();
        TInterface CreateObject<TBase, TInterface>(string name) where TBase : class, new();
        TInterface CreateObject<TInterface>(string name);

        TBase CreateObject<TBase>(string name, BuilderPropertyInfo[] properties = null) where TBase : class, new();
        TBase CreateObject<TBase>(string name, BuilderPropertyInfo[] properties = null, Type[] interfaces = null, bool autoGenerateInterfaceproperties = false) where TBase : class, new();
    }
}
