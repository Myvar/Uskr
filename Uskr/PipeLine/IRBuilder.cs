using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using dnlib.DotNet;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;
using MethodAttributes = dnlib.DotNet.MethodAttributes;

namespace Uskr.PipeLine
{
    public class IRBuilder : IPipeLine
    {
        public static IRAssembly Build(ModuleDefMD module, UskrContext context, bool rk = false)
        {
            Logger.Log($"Building IR for module: {module.Name}");

            var re = new IRAssembly();

            if (!rk) //dont imbed resource in the kernel assembly
            {
                foreach (var resource in module.Resources)
                {
                    if (resource.ResourceType == ResourceType.Embedded
                        && resource is EmbeddedResource er)
                    {
                        re.EmbeddedResources.Add(new IREmbedded()
                        {
                            Namespace = resource.Name,
                            Data = er.CreateReader().ToArray()
                        });
                    }
                }
            }

            foreach (var type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                if (type.CustomAttributes.Any(x =>
                    x.AttributeType.Name.ToString() == typeof(UskrIgnoreAttribute).Name))
                    continue;


                if (rk)
                {
                    if (type.CustomAttributes.All(x => x.AttributeType.Name.ToString() != typeof(RKAttribute).Name))
                    {
                        /*Logger.Debug(
                            $"Found Type {type.Name} but it does not have RK Attribute, and is there for being skipped");*/
                        continue;
                    }
                    else
                    {
                    }
                }

                Logger.Log($"Found Valid Type: {type.Name}");


                if (type.IsClass)
                {
                    foreach (var method in type.Methods)
                    {
                        if (!method.HasBody) continue;

                        if (rk)
                        {
                            if (method.IsConstructor && !method.IsStaticConstructor)
                            {
                                Logger.Warn($"RK METH IS CTOR {method.FullName}");
                                continue;
                            }

                            if (!method.IsStatic)
                            {
                                Logger.Error($"RK METH NOT STATIC {method.FullName}");
                                Environment.Exit(0);
                            }
                        }

                        if (method.CustomAttributes.Any(x =>
                            x.AttributeType.Name.ToString() == typeof(UskrIgnoreAttribute).Name))
                            continue;


                        if (method.IsVirtual && !context.VirtualTypes.Contains(type))
                        {
                            context.VirtualTypes.Add(type);
                        }


                        var irMeth = new IRMethod();
                        irMeth.IsVirtual = (method.Attributes &
                                            MethodAttributes.VtableLayoutMask) == MethodAttributes.VtableLayoutMask;

                        irMeth.Namespace = method.FullName;
                        irMeth.IsFunc = method.HasReturnType;
                        irMeth.IsStatic = method.IsStatic;
                        irMeth.BaseType = type.Name;
                        if (method.CustomAttributes.Any(x =>
                            x.AttributeType.Name.ToString() == typeof(PlugAttribute).Name))
                        {
                            var att = method.CustomAttributes
                                .First(x => x.AttributeType.Name.ToString() == typeof(PlugAttribute).Name);
                            irMeth.Namespace = att.ConstructorArguments.First().Value.ToString();
                        }
                        
                        if (method.CustomAttributes.Any(x =>
                            x.AttributeType.Name.ToString() == typeof(CCallAttribute).Name))
                        {
                          
                            irMeth.CCall = true;
                        }

                        irMeth.Identifier = method.Name;
                        irMeth.ParamsCount = method.Parameters.Count;
                        irMeth.Body = method.Body;

                        re.Methods.Add(irMeth);
                    }

                    foreach (FieldDef member in type.Fields)
                    {
                        var irMem = new IRMember();
                        irMem.Namespace = member.FullName;
                        irMem.Identifier = member.Name;
                        irMem.Static = member.IsStatic;
                        irMem.IsField = true;

                        irMem.Size = member.GetFieldSize();

                        irMem.InitValue = member.ResolveFieldDef().InitialValue;

                        re.Members.Add(irMem);
                    }

                    foreach (var member in type.Properties)
                    {
                        var irMem = new IRMember();
                        irMem.Namespace = member.FullName;
                        irMem.Identifier = member.Name;
                        // Prop cant be static i think @Confirm
                        irMem.IsField = false;


                        re.Members.Add(irMem);
                    }
                }
            }

            return re;
        }


        public void Run(ref UskrContext context)
        {
            context.RuntimeKernel = IRBuilder.Build(ModuleDefMD.Load(typeof(UskrEngine).Module), context, true);
            context.UserSpace = IRBuilder.Build(ModuleDefMD.Load(context.UserAssembly.Modules.First()), context, false);
        }
    }
}