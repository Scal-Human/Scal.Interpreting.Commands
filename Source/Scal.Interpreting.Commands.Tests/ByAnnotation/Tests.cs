using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Scal.Interpreting.Commands.Tests.ByAnnotation;

public class Tests(ITestOutputHelper output)
{
    private ITestOutputHelper _output = output;

    public static IEnumerable<object[]> GetTestData() => [
        [new TestData("Name field is required",             null,   "List", "Image",    "Type=1")],
        [new TestData("Name field is required",             null,   "List", "Image",    "Type=1",   "N" )],
        [new TestData("Name field is required",             null,   "List", "Image",    "Type=A",   "N" )],
        [new TestData(null, typeof(CmdListImageByType),             "List", "Image",    "Type=1",   "Name=abc")],
        [new TestData(null, typeof(CmdListImageByType),             "L",    "Ima",      "T=1",      "N=abc" )],
        [new TestData("TypeId must be between 1 and 9",     null,   "List", "Image",    "Type=0",   "N=abc" )],
        [new TestData(null, typeof(CmdListImageByNamespace),        "List", "Image",    "--Namespace=abc")],
        [new TestData(null, typeof(CmdListImageByNamespace),        "List", "Ima",      "--Names=abc")],
        [new TestData("Ambiguous command",                  null,   "List", "Ima",      "Name=abc")],
        [new TestData("abbreviation is ambiguous",          null,   "List", "Ima",      "N=abc")],
        [new TestData(null, typeof(CmdListImport),                  "List", "Import")],
        [new TestData(null, typeof(CmdListImport),                  "List", "Imp")],
        [new TestData("Ambiguous command",                  null,   "L", "I")],
        [new TestData(null, typeof(CmdCleanup),                     "Cleanup")],
    ];


    [Theory]
    [MemberData(nameof(GetTestData))]
    public void ShouldInterpretOrReject(TestData testData)
    {
        var interpreter     = new CommandLineInterpreter();
        var interpretation  = interpreter.Interpret<Program>(testData.Args);
        this._output.WriteLine(string.Join(' ', testData.Args));
        interpretation.Feedback(this._output.WriteLine);
        if (testData.ExpectedType is null) {
            Assert.Null(interpretation.Command);
            Assert.NotEmpty(interpretation.Results);
        } else {
            Assert.IsType(testData.ExpectedType, interpretation.Command);
            Assert.Empty(interpretation.Results);
        }
        List<string> feedback = [];
        interpretation.Feedback(feedback.Add);
        Assert.NotEmpty(feedback);
        if (testData.Error is not null) {
            Assert.Contains(feedback, msg => msg.Contains(testData.Error, StringComparison.OrdinalIgnoreCase));
        }
    }

}
