using YamlDotNet.Serialization;

namespace Uskr.Core
{
    public class BuildCfg
    {
        [YamlMember(Alias = "gcc-root")] public string GccRoot { get; set; }
        [YamlMember(Alias = "qemu-root")] public string QemuRoot { get; set; }
        [YamlMember(Alias = "nasm-exe")] public string NasmExe { get; set; }
    }
}