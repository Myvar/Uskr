using System;
using System.Xml.Schema;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static unsafe class KObject
    {
        [Plug("System.Void System.Object::.ctor()")]
        public static void Ctor(void* obj)
        {
            
        }
        
        [Plug("System.String System.Object::ToString()")]
        public static string ToString(void* obj)
        {
            return "System.Object";
        }
    }
}