# Scal.Interpreting.Commands

A lightweight, deterministic command-line interpreter for .NET 8 with attribute-based validation and type conversion.

Unlike reflection-heavy or attribute-mandatory CLI frameworks, **Scal.Interpreting.Commands** prioritizes deterministic resolution and minimal dependencies.

## Motivation

### Philosophy

- simple deterministic grammar: **verb (noun) (arguments)**
- case-insensitive
- dash-tolerant but dash-agnostic
- accept abbreviations but detect collisions
- bulletproof and predictable behavior

### Technical features

- verb/noun definition by attributes or Pascal-case naming convention
- strongly-typed command instantiation
- validation via DataAnnotations
- TypeConverter support
- contextual help generation
- dependency-free
- DI-agnostic construction
- .Net 8.0 LTS compatible (console or ASP.NET)

## Usage example

```cli
dotnet add package Scal.Interpreting.Commands
```

<details open="true">
    <summary>
    Example of a program accepting as <b>List Image Name=abc</b> command
    </summary>

```c#
[DataContract(Name = "CliArgs")]
[Description("Cli arguments interpreter example")]
public abstract class Program
{
    private static async Task<int> Main(string[] args)
    {
        var interpretation = new CommandLineInterpreter().Interpret<Program>(args);
        if (interpretation.Command is null) {
            interpretation.Feedback(Console.WriteLine);
            return 1;
        }
        interpretation.Feedback(Console.WriteLine, showHelp: false);
        return await interpretation.Command.ExecuteAsync();
    }

    public abstract Task<int> ExecuteAsync();

    [Description("List the images")]
    [DataContract(Namespace = "List", Name = "Image")]
    public class ListImage : Program
    {
        [Description("The image name pattern")]
        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        [Description("The image type Id")]
        [Range(1, 9)]
        public int TypeId { get; set; } = 1;

        public override Task<int> ExecuteAsync()
        {
            Console.WriteLine("Simulate {0} {1}", nameof(ListImage), this.Name);
            return Task.FromResult(0);
        }
    }
}
```
</details>

Mention that:
- the Program itself is an abstract with just an entrypoint and the **ExecuteAsync** contract
- it is derived in a **ListImage** class that is instantiated
- I choose to output the feedback without help in case of success which shows the program title

Executing it with <b>List Image Name=abc</b> or <b>L I N=abc</b> gives:
```cli
CliArgs Cli arguments interpreter example
Simulate ListImage abc
```

## Help

Executing the program without parameter will output this with the accepted abbreviations in parentheses:
```cli
CliArgs Cli arguments interpreter example
*** : Usage: verb (noun) (parameters)
  List     Image            List the images (L I)
    TypeId                  The image type Id (T)
    Name                    The image name pattern (N)
```

## Validation using **System.ComponentModel.DataAnnotations** attributes or your custom attributes

Executing it with <b>List Image Name= Type=10</b> or <b>L I N= T=10</b> gives:
```cli
CliArgs Cli arguments interpreter example
*** TypeId: The field TypeId must be between 1 and 9.
*** Name: The Name field is required.
  List     Image            List the images (L I)
    TypeId                  The image type Id (T)
    Name                    The image name pattern (N)
```

## New ListImport command without attribute

<details open="true">
    <summary>
    When adding a new command <b>List Import</b> to the same program:
    </summary>

```c#
    public class ListImportWithoutParameter : Program
    {
        public override Task<int> ExecuteAsync()
        {
            Console.WriteLine("Simulate {0}", nameof(ListImportWithoutParameter));
            return Task.FromResult(0);
        }
    }
```
</details>

Mention that:
- The class does not require any attribute and VerbNoun is extracted using the first two words of the class name Pascal-casing
- Acronyms such as XML or HTTP will be split into individual letters unless explicitly configured using attributes,
e.g. a **ListXMLFile** will be interpreted as **List X** by convention.
In such a case, use a **DataContract** attribute to clarify your intent.
- The abbreviations now become **l ima** for **List Image** and **l imp** for **List Import**
as those are the minimum required to prevent ambiguity.

> :warning: Please note that abbreviations **should never** be used is scripts or documentation for many reasons
(clarity, newbie-friendly), including the fact that they may change by adding new commands.

## Verb-only command

<details open="true">
    <summary>
    You may a define a verb-only command by creating a one word class like <b>Cleanup</b>,
    or by specifying a <b>Namespace</b> in a <b>DataContract</b> without <b>Name</b>:
    </summary>

```c#
    public class Cleanup : Program
    {
        public override Task<int> ExecuteAsync()
        {
            Console.WriteLine("Simulate {0}", nameof(Cleanup));
            return Task.FromResult(0);
        }
    }
```
</details>

## Type converter and custom validation

<details open="true">
    <summary>
    Custom type converter and custom validation may be used:
    </summary>

```c#
    public class SomeCommand : Program
    {
        [TypeConverter(typeof(YourTypeConverter))]
        [YourCustomValidation]
        [Required]
        public YourType Reference { get; set; }

        public override Task<int> ExecuteAsync()
        {
            Console.WriteLine("Simulate {0} {1}", nameof(SomeCommand), this.Reference);
            return Task.FromResult(0);
        }
    }
```
</details>

## Factory constructor

You may provide a factory delegate to integrate with your preferred DI framework:

```c#
var interpreter = new CommandLineInterpreter(
    factory: type => MyContainer.Resolve(type));
```

If no factory delegate is provided, Activator is used:

```c#
... = Activator.CreateInstance(type);
```

## Examples

To view examples, see the tests models: by convention, by annotation and with type converter.

## Thanks

Thanks to Dan (aka ChatGPT) for they advices and making doubts disappear.
