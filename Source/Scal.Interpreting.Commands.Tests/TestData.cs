using System;

namespace Scal.Interpreting.Commands.Tests;

public record TestData(string? Error, Type? ExpectedType, params string[] Args);
