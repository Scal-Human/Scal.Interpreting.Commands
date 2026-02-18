using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Scal.Interpreting.Commands.Tests.ByAnnotation;

internal abstract class Program
{
}

[DataContract(Namespace = "List", Name = "Image")]
internal class CmdListImageByType : Program
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;
    [Range(1, 9)]
    public int TypeId { get; set; }
}

[DataContract(Namespace = "List", Name = "Image")]
internal class CmdListImageByNamespace : Program
{
    public string Name { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
}

[DataContract(Namespace = "List", Name = "Import")]
internal class CmdListImport : Program
{
}

[DataContract(Namespace = "Cleanup")]
internal class CmdCleanup : Program
{
}
