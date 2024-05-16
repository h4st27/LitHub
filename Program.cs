using MyApp.Services;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        Film filmBarbie = new(1, "Barbie", true, 9, 'A');
        Film filmOppenheimer = new(2, "Oppenheimer", false, 9, 'A');

        Type filmType = typeof(Film);
        TypeInfo filmTypeInfo = filmType.GetTypeInfo();

        Console.WriteLine($"Type Name: {filmType.Name}");

        Console.WriteLine("\nMembers:");
        foreach (MemberInfo member in filmTypeInfo.GetMembers())
        {
            Console.WriteLine($"Name: {member.Name}({member.ReflectedType}), {member.MemberType} ");
        }

        Console.WriteLine("\nFields:");
        foreach (FieldInfo fieldInfo in filmTypeInfo.DeclaredFields)
        {
            Console.WriteLine($"Name: {fieldInfo.Name}({fieldInfo.Attributes}), FieldType: {fieldInfo.FieldType}");
        }
        Console.WriteLine("\nMethods:");
        foreach (MethodInfo methodInfo in filmTypeInfo.DeclaredMethods)
        {
            Console.WriteLine($"Name: {methodInfo.Name}({methodInfo.Attributes}), MethodType: {methodInfo.ReturnType}");
        }

       Console.WriteLine("\nReflection:");
       MethodInfo reflectionMethod = filmType.GetMethod("WriteInfo");
        if (reflectionMethod != null ) {
            reflectionMethod.Invoke(filmBarbie, [filmOppenheimer]);
        }
    }
}