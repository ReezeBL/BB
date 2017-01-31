using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlessBuddy.Core.Engine;

namespace BlessBuddy.Core
{
    public static class ObjectReverser
    {
        public static void ReverseClass(UStruct objClass, StringBuilder sb = null)
        {
            if (!objClass.IsValid && sb != null)
            {
                File.WriteAllText("ReverseResults.txt", sb.ToString());
                Console.WriteLine("Reversing done!");
                return;
            }
            if (!objClass.IsValid)
                return;

            if (sb == null)
                sb = new StringBuilder();

            var objSuperClass = new UStruct(objClass.SuperField);
            sb.AppendLine(objSuperClass.IsValid ? $"{objClass.Name} : {objSuperClass.Name} (0x{objSuperClass.PropertySize:X})" : $"{objClass.Name}");

            List<UFunction> functions = new List<UFunction>();
            List<UProperty> properties = new List<UProperty>();

            var property = objClass.Children;
            while (property.IsValid)
            {
                if(property.Class.Name == "Function")
                    functions.Add(new UFunction(property));
                else
                    properties.Add(new UProperty(property));
                
                property = property.Next;
            }
            foreach (var uFunction in functions)
            {
                sb.AppendLine(GenerateFunctionStab(uFunction));
            }
            sb.AppendLine();
            foreach (var uProperty in properties)
            {
                sb.AppendLine($"0x{uProperty.PropertyOffset,-8:X}\t{GetPropertyType(uProperty),-25}\t{uProperty.Name}");
            }

            sb.AppendLine();
            sb.AppendLine();

            ReverseClass(objSuperClass, sb);
        }

        public static string GetPropertyType(UProperty property)
        {
            switch (property.Class.NameId)
            {
                case 1:
                    return "byte";
                case 2:
                    return "int";
                case 3:
                    return "bool";
                case 4:
                    return "float";
                case 5:
                {
                        var structProperty = new UStructProperty(property);
                        return structProperty.PropertyClass.Name + "*";
                }
                case 8:
                {
                        var structProperty = new UClassProperty(property);
                        return structProperty.MetaObject.Name + "*";
                }
                case 9:
                    {
                        var structProperty = new UStructProperty(property);
                        return $"Array<{GetPropertyType(new UStructProperty(structProperty.PropertyClass))}>";
                    }
                case 10:
                {
                    var structProperty = new UStructProperty(property);
                    return structProperty.PropertyClass.Name;
                }
                case 13:
                    return "string";
                case 14:
                    {
                        var structProperty = new UClassProperty(property);
                        return $"Map<{GetPropertyType(new UProperty(structProperty.PropertyClass))}, {new UProperty(structProperty.MetaObject)}>";
                    }
                default:
                    return property.Class.Name;
            }
        }

        public static string GenerateFunctionStab(UFunction function)
        {
            StringBuilder sb = new StringBuilder($"0x{function.FunctionPtr.ToInt64(),-8:X}\t");
            List<UProperty> funcParams = new List<UProperty>();

            var param = function.Children;
            while (param.IsValid)
            {
                funcParams.Add(new UProperty(param));
                param = param.Next;
            }

            bool hasReturn = (function.FunctionFlags & 0x400) != 0;
            if (hasReturn && funcParams.Any())
                sb.Append(GetPropertyType(funcParams.Last()) + " ");
            else
                sb.Append("void ");
            sb.Append(function.FunctionName + "(");

            int paramsCount = hasReturn ? funcParams.Count - 1 : funcParams.Count;

            for (int i = 0; i < paramsCount - 1; i++)
            {
                sb.Append($"{GetPropertyType(funcParams[i])} {funcParams[i].Name}");
                sb.Append(i == paramsCount - 1? "" : ", ");
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
