using System;
using System.Reflection;
using System.Threading.Tasks;
using ServiceResult.AspectOriented.ServiceProxy.Extensions;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    public class GenericDecorator<T> : DispatchProxy
    {
        protected T Decorated { get; private set; }

        /// <summary>
        /// Whenever any method on the generated proxy type is called,
        /// this method is invoked before the target method to dispatch control.
        /// </summary>
        /// <remarks>Use this interceptor method to initialize your plan before inject any behavior into the proxy.</remarks>
        /// <param name="targetMethod">The method the caller invoked.</param>
        /// <param name="args">The arguments the caller passed to the method.</param>
        /// <param name="methodKind">The kind of methode.</param>
        protected virtual void BeforeInvoke(MethodInfo targetMethod, object[] args, MethodKind methodKind) { }

        /// <summary>
        /// Whenever any method on the generated proxy type is called,
        /// this method is invoked after the target method to dispatch control.
        /// </summary>
        /// <remarks>Use this interceptor method to inject behavior into the proxy.</remarks>
        /// <param name="targetMethod">The method the caller invoked.</param>
        /// <param name="args">The arguments the caller passed to the method.</param>
        /// <param name="methodKind">The kind of methode.</param>
        /// <param name="result">The object returned from the target method.</param>
        /// <returns>The object to return to the caller, or null for void methods.</returns>
        protected virtual object AfterInvoke(MethodInfo targetMethod, object[] args, MethodKind methodKind, object result) => result;

        /// <summary>Executed when an exception occurred.</summary>
        /// <param name="exception">The exception that occured.</param>
        /// <param name="methodInfo">The function that executed and issued this exception.</param>
        /// <param name="handled">TReturn true when handled the exception, otherwise false</param>
        /// <returns>The object to return to the caller, or null for void methods.</returns>
        protected virtual object OnException(Exception exception, MethodInfo methodInfo, out bool handled)
        {
            handled = false;
            return null;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var getAwaiterMethod = targetMethod.ReturnType.GetMethod(nameof(Task.GetAwaiter));

            try
            {
                object result = null;
                var methodKind = targetMethod.GetKind();

                BeforeInvoke(targetMethod, args, methodKind);

                // Check if method is async
                if (getAwaiterMethod != null)
                {
                    if (targetMethod.ReturnType.IsGenericType)
                    {
                        dynamic awaitable = targetMethod.Invoke(Decorated, args);

                        result = awaitable.GetAwaiter().GetResult();
                        //Task.Run(() => { }).GetAwaiter();
                        result = AfterInvoke(targetMethod, args, methodKind, result);
                        result = CreateTask(targetMethod, result);
                    }
                    else
                    {
                        dynamic awaitable = targetMethod.Invoke(Decorated, args);

                        awaitable.GetAwaiter().GetResult();
                        result = Task.CompletedTask;
                    }
                }
                else
                {
                    if (targetMethod.ReturnType == typeof(void))
                    {
                        targetMethod.Invoke(Decorated, args);
                    }
                    else
                    {
                        result = targetMethod.Invoke(Decorated, args);
                        result = AfterInvoke(targetMethod, args, methodKind, result);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // Check if ex is TargetInvocationException or AggregateException.
                ex = ex.InnerException ?? ex;

                var result = OnException(ex, targetMethod, out var handled);

                result = getAwaiterMethod is null ? result : CreateTask(targetMethod, result);
                return handled ? result : throw ex;
            }
        }

        protected object CreateTask(MethodInfo targetMethod, object result) => CreateTask(GetMethodReturnType(targetMethod), result);

        /// <summary>
        /// Return the type from the method, void if the method was Task, T if the method was Task<T>.
        /// </summary>
        protected Type GetMethodReturnType(MethodInfo targetMethod)
        {
            //if (targetMethod.ReturnType == typeof(void)) { return null; }

            if (typeof(Task).IsAssignableFrom(targetMethod.ReturnType))
            {
                return targetMethod.ReturnType.IsGenericType
                    ? targetMethod.ReturnType.GetGenericArguments()[0]
                    : typeof(void);
            }

            return targetMethod.ReturnType;
        }
        protected object CreateTask(Type genericType, object result)
        {
            var fromResult = typeof(Task).GetMethod(nameof(Task.FromResult), BindingFlags.Public | BindingFlags.Static);

            return fromResult.MakeGenericMethod(genericType).Invoke(null, new object[] { result });
            //return Task.FromResult((dynamic)result);
        }

        protected virtual void SetParameters(T original) => Decorated = original ?? throw new ArgumentNullException(nameof(original));
    }
}
