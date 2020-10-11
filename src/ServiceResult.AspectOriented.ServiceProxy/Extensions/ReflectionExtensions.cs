using System.Linq;
using System.Reflection;

namespace ServiceResult.AspectOriented.ServiceProxy.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool IsProperty(this MethodInfo methodInfo) =>
                methodInfo?.DeclaringType != null &&
                methodInfo.IsSpecialName &&
                (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_")) &&
                methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetMethod == methodInfo || prop.SetMethod == methodInfo);

        public static bool IsSetAccessor(this MethodInfo methodInfo) =>
                methodInfo?.DeclaringType != null &&
                methodInfo.IsSpecialName &&
                methodInfo.Name.StartsWith("set_") &&
                methodInfo.DeclaringType.GetProperties().Any(prop => prop.SetMethod == methodInfo);

        public static bool IsGetAccessor(this MethodInfo methodInfo) =>
                methodInfo?.DeclaringType != null &&
                methodInfo.IsSpecialName &&
                methodInfo.Name.StartsWith("get_") && methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetMethod == methodInfo);

        public static MethodKind GetKind(this MethodInfo methodInfo) => methodInfo.IsProperty()
            ? methodInfo.IsSetAccessor()
                ? MethodKind.SetAccessor
                : methodInfo.IsGetAccessor()
                    ? MethodKind.GetAccessor
                    : MethodKind.Method
            : MethodKind.Method;
    }
}
