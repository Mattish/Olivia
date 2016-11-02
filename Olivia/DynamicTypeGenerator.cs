using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Olivia
{
    public static class DynamicTypeGenerator
    {
        public static Type GetNewType(Type type)
        {
            AssemblyName asmName = new AssemblyName();
            asmName.Name = "OliviaAssem";
            AssemblyBuilder asmBuild = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder modBuild = asmBuild.DefineDynamicModule("OliviaMod", "OliviaAssem.dll");
            var random = new Random();
            var newTypeName = $"{random.Next()}{type.Name}";
            var tb = modBuild.DefineType(newTypeName, TypeAttributes.Public, type);
            var newType = tb.CreateType();
            //Logger.AppendLine($"Created dynamic type:{newTypeName}");
            return newType;
        }

        public static T GetNewType<T>(params object[] @params)
        {
            var newType = GetNewType(typeof(T));
            var constructorInfo = newType.GetConstructors()[0];
            var battleController = constructorInfo.Invoke(null, @params);
            return (T)battleController;

        }
    }
}