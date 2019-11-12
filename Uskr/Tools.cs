using System;
using System.Linq;
using System.Text;

namespace Uskr
{
    public static class Tools
    {
        public static string PlugGenerator(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Uskr.Attributes;");

            sb.AppendLine("namespace Uskr.Runtime");
            sb.AppendLine("{");
            sb.AppendLine("[RK]");
            sb.AppendLine("public unsafe static class k" + t.Name);
            sb.AppendLine("{");

            foreach (var meth in t.GetMethods())
            {
                var args = "";
                if (meth.GetParameters().Length > 0)
                {
                    foreach (var parameter in meth.GetParameters())
                    {
                        args += parameter.ParameterType.FullName + ",";
                    }
                }

                sb.AppendLine($"[Plug(\"{meth.ReturnType.FullName} {t.FullName}::{meth.Name}({args.TrimEnd(',')})\")]");
                sb.Append($"private static {meth.ReturnType.Name.ToLower()} {meth.Name}(");
                if (!meth.IsStatic) sb.Append($"void* obj");

                if (meth.GetParameters().Length > 0)
                {
                    for (var i = 0; i < meth.GetParameters().Length - 1; i++)
                    {
                        var parameter = meth.GetParameters()[i];
                        sb.Append($"{parameter.ParameterType.Name.ToLower()} {parameter.Name},");
                    }

                    sb.Append($"{meth.GetParameters().Last().ParameterType.Name.ToLower()} {meth.GetParameters().Last().Name}");
                }

                sb.AppendLine($")");
                sb.AppendLine("{");
                sb.AppendLine("}");
            }


            sb.AppendLine("}");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}