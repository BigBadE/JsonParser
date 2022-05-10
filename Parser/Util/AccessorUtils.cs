using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Parser.Util
{
    public static class AccessorUtils
    {
        private static readonly Dictionary<string, Delegate> _generated = new Dictionary<string, Delegate>();
            
        public static Delegate GenerateSetter(FieldInfo fieldInfo)
        {
            string name = fieldInfo.Name + "_get";
            if (_generated.ContainsKey(name))
            {
                return (Action<object>) _generated[name];
            }

            DynamicMethod dynamicMethod = new DynamicMethod(name, null, Type.EmptyTypes, true);
            
            ILGenerator generator = dynamicMethod.GetILGenerator();
            
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Emit(OpCodes.Ret);
            
            return dynamicMethod.CreateDelegate(typeof(Action<int>));
        }
    }
}