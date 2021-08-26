using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mockingjay.Common.Json
{
    internal partial class ConventionBasedSerializer<TSvo>
    {
        private static readonly Type[] NodeTypes = new[] { typeof(string), typeof(double), typeof(long), typeof(bool) };

        private Type SvoType { get; } = TypeHelper.GetNotNullableType(typeof(TSvo));

        private void Initialize()
        {
            var factories = SvoType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(IsFactory)
                ;

            foreach (var factory in factories)
            {
                var parameterType = factory.GetParameters()[0].ParameterType;

                if (parameterType == typeof(string))
                {
                    fromJsonString = CompileDeserialize<string>(factory);
                }
                else if (parameterType == typeof(double))
                {
                    fromJsonDouble = CompileDeserialize<double>(factory);
                }
                else if (parameterType == typeof(long))
                {
                    fromJsonLong = CompileDeserialize<long>(factory);
                }
                else if (parameterType == typeof(bool))
                {
                    fromJsonBool = CompileDeserialize<bool>(factory);
                }
                else
                {
                    // do nothing
                }
            }

            if (fromJsonString is null)
            {
                return;
            }

            if (fromJsonDouble is null)
            {
                fromJsonDouble = (num) => fromJsonString(num.ToString(CultureInfo.InvariantCulture));
            }

            if (fromJsonLong is null)
            {
                fromJsonLong = (num) => fromJsonString(num.ToString(CultureInfo.InvariantCulture));
            }

            if (fromJsonBool is null)
            {
                fromJsonBool = (b) => fromJsonString(b ? "true" : "false");
            }

            var toJson = SvoType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(IsToJson)
                ;
            toJsonObject = CompileSerialize(toJson);
        }

        private bool IsFactory(MethodInfo method)
        {
            return method.ReturnType == SvoType
                   && method.Name == nameof(ConventionBasedSerializer<object>.FromJson)
                   && method.GetParameters().Length == 1
                   && NodeTypes.Contains(method.GetParameters()[0].ParameterType)
                ;
        }

        private static bool IsToJson(MethodInfo method)
        {
            return method.Name == nameof(ConventionBasedSerializer<object>.ToJson)
                   && method.GetParameters().Length == 0
                   && method.ReturnType != null;
        }

        private Func<TSvo, object> CompileSerialize(MethodInfo method)
        {
            var toJson = method ?? typeof(object).GetMethod(nameof(ToString));
            var svo = Expression.Parameter(typeof(TSvo), "svo");
            var par = SvoType != typeof(TSvo)
                ? Expression.Convert(svo, SvoType)
                : (Expression)svo;

            Expression body = Expression.Call(par, toJson);

            // if nullable, add a convert.
            if (toJson.ReturnType != typeof(object))
            {
                body = Expression.Convert(body, typeof(object));
            }

            var expression = Expression.Lambda<Func<TSvo, object>>(body, svo);

            return expression.Compile();
        }

        private static Func<TNode, TSvo> CompileDeserialize<TNode>(MethodInfo method)
        {
            var node = Expression.Parameter(typeof(TNode), "node");
            Expression body = Expression.Call(method, node);

            // If nullable, add a convert.
            if (method.ReturnType != typeof(TSvo))
            {
                body = Expression.Convert(body, typeof(TSvo));
            }

            var expression = Expression.Lambda<Func<TNode, TSvo>>(body, node);
            return expression.Compile();
        }
    }
}
