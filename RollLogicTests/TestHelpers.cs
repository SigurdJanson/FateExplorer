using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RollLogicTests
{
    

    internal static class TestHelpers
    {
        public const string Path2wwwrootData = "..\\..\\..\\..\\dev\\wwwroot\\data";


        /// <summary>
        /// Compares two objects on the topmost level of properties without recursive
        /// entry into complex objects. Be careful with reference types!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="to"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        /// <remarks>
        /// Formerly known as PublicInstancePropertiesEqual, 
        /// see <see href="https://stackoverflow.com/a/844855/13241545"/>
        /// </remarks>
        public static bool IsTier1Equal<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new(ignore);
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            Console.WriteLine($"Mismatch at property {pi.Name}");
                            return false;
                        }
                    }
                }
                return true;
            }
            return self == to;
        }



        public static bool IsDeeplyEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new(ignore);
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (TypeExtensions.IsSimpleType(type.GetProperty(pi.Name).GetType()))
                            if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                            {
                                Console.WriteLine($"Mismatch at property {pi.Name}");
                                return false;
                            }
                        else
                            if (!IsDeeplyEqual(selfValue, toValue, ignore))
                            {
                                return false;
                            }
                                
                    }
                }
                return true;
            }
            return self == to;
        }

    }

    public static class TypeExtensions
    {
        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// </summary>
        /// <see href="http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive"/>
        public static bool IsSimpleType(this Type type)
        {
            return
               type.IsValueType ||
               type.IsPrimitive ||
               new[]
               {
               typeof(String),
               typeof(Decimal),
               typeof(DateTime),
               typeof(DateTimeOffset),
               typeof(TimeSpan),
               typeof(Guid)
               }.Contains(type) ||
               (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            return member.MemberType switch
            {
                MemberTypes.Event => ((EventInfo)member).EventHandlerType,
                MemberTypes.Field => ((FieldInfo)member).FieldType,
                MemberTypes.Method => ((MethodInfo)member).ReturnType,
                MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                _ => throw new ArgumentException
                (
                "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                ),
            };
        }
    }
}
