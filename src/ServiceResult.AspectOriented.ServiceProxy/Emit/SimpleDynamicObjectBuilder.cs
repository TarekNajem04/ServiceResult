using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ServiceResult.AspectOriented.ServiceProxy.Emit
{
    /// <summary>
    /// This is a special builder for our scenario, but we can do more on System.Reflection.Emit.
    /// For more details see <a href="https://livebook.manning.com/book/metaprogramming-in-dot-net/about-this-book/">Metaprogramming in .NET Book</a>.
    /// </summary>
    public class SimpleDynamicObjectBuilder : IObjectBuilder
    {
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly ModuleBuilder _moduleBuilder;

        public SimpleDynamicObjectBuilder(string assemblyName)
        {
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule("MainModule");
        }

        public SimpleDynamicObjectBuilder() : this(Guid.NewGuid().ToString()) { }

        public TInterface CreateObject<TInterface>(string name)
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface) { return default; }

            return CreateObject(name, null, null, new Type[] { interfaceType }, true) is TInterface @interface ? @interface : default;
        }

        public TInterface CreateObject<TBase, TInterface>(string name) where TBase : class, new() => CreateObject<TBase, TInterface>(name, null);

        public TInterface CreateObject<TBase, TInterface>(string name, BuilderPropertyInfo[] properties = null) where TBase : class, new()
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface) { return default; }

            return CreateObject(name, properties, typeof(TBase), new Type[] { interfaceType }, true) is TInterface @interface ? @interface : default;
        }

        public TBase CreateObject<TBase>(string name, BuilderPropertyInfo[] properties = null) where TBase : class, new() => CreateObject<TBase>(name, properties);
        public TBase CreateObject<TBase>(string name, BuilderPropertyInfo[] properties = null, Type[] interfaces = null, bool autoGenerateInterfaceproperties = false) where TBase : class, new() =>
            CreateObject(name, properties, typeof(TBase), interfaces, autoGenerateInterfaceproperties) as TBase;

        public object CreateObject(string name, BuilderPropertyInfo[] properties = null, Type baseClass = null, Type[] interfaces = null, bool autoGenerateInterfaceproperties = false)
        {
            // To avoid creating the class again.
            var definedType = Array.Find(_moduleBuilder.GetTypes(), x => x.Name == name);

            if (definedType != null)
            {
                return Activator.CreateInstance(definedType);
            }

            var dynamicClass = DefineType(name, baseClass, interfaces);

            CreateDefaultConstructor(dynamicClass);

            if (properties?.Length > 0)
            {
                foreach (var property in properties)
                {
                    CreateProperty(dynamicClass, property);
                }
            }

            if (interfaces?.Length > 0 && autoGenerateInterfaceproperties)
            {
                foreach (var property in interfaces
                                            .SelectMany(x => x.GetProperties())
                                            .Select(x => new BuilderPropertyInfo()
                                            {
                                                Name = x.Name,
                                                Type = x.PropertyType,
                                                IsInterfaceImplementation = true
                                            })
                                            .ToArray())
                {
                    CreateProperty(dynamicClass, property);
                }
            }

            return Activator.CreateInstance(dynamicClass.CreateType());
        }

        private TypeBuilder DefineType(string name, Type baseClass = null, Type[] interfaces = null) => _moduleBuilder.DefineType(name,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                baseClass == typeof(void) ? null : baseClass,
                interfaces);

        private ConstructorBuilder CreateDefaultConstructor(TypeBuilder typeBuilder) =>
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

        private PropertyBuilder CreateProperty(TypeBuilder typeBuilder, BuilderPropertyInfo propertyInfo)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyInfo.Name, propertyInfo.Type, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, propertyInfo.Type, null);
            var methodAttributes =
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig |
                (propertyInfo.IsInterfaceImplementation ? MethodAttributes.Virtual : 0);

            // get => _privateField;
            var getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyInfo.Name, methodAttributes, propertyInfo.Type, Type.EmptyTypes);
            var getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            // set => _privateField = value;
            var setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyInfo.Name, methodAttributes, null, new[] { propertyInfo.Type });
            var setIl = setPropMthdBldr.GetILGenerator();

            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);
            setIl.Emit(OpCodes.Ret);

            // Set get and set method to property
            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

            return propertyBuilder;
        }
    }
}
