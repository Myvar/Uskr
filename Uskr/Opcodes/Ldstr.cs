using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldstr)]
    public class Ldstr : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            var bytes = "";

            foreach (var c in instruction.Operand.ToString())
            {
                bytes += ((byte) c) + ",";
            }

            bytes = bytes.Trim().TrimEnd(',');

            var key = "_" + Utils.MD5(instruction.Operand.ToString());
            var bits = BitConverter.GetBytes(instruction.Operand.ToString().Length);
            if (!context.GlobalsExtra.ContainsKey(key))
                context.GlobalsExtra.Add(key,
                    $"{bits[0]},{bits[1]},{bits[2]},{bits[3]}" + "," +
                    bytes);
            context.Asm($"push _{Utils.MD5(instruction.Operand.ToString())}");
        }
    }
}