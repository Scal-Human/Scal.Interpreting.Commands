using System.ComponentModel.DataAnnotations;

namespace Scal.Interpreting.Commands.Tests.ByConvention;

internal abstract class Program
{
}

internal class ListImageByType : Program
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Range(1, 9)]
    public int TypeId { get; set; }
}

internal class ListImageByNamespace : Program
{
    public string Name { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
}

internal class ListImport : Program
{
}

internal class Cleanup : Program
{
}
