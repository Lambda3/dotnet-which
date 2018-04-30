# Dotnet Which - The .NET tools finder

A tool that helps you find your .net Tools.

Any tool that is called `dotnet-*` can be invoked by `dotnet` like so: `dotnet foo` (assuming the tool is called `dotnet-foo`).

This tool helps you find those tools, listing them.

To use this you have to have at least .NET Core 2.1, which has Global tools.
.NET global tools are a new feature in .NET 2.1, which is currently in preview.

## Synopsis

```bash
dotnet which
```

Sample output:

````
Found the following tools:
.net
sdk
which
````


## Install

Install .NET Core CLI at least 2.1 from [microsoft.com](https://www.microsoft.com/net/download/all),
then run:

```bash
dotnet tool install -g dotnet-which
```

### Parameters

This tool accepts parameters. You call it like so:

```bash
dotnet which [options]
```

Run `dotnet which` with `--help`  to see possible options. Here we document a few:

*  `--full-name` - Show full command name
*  `--path` - Show command path
*  `--quiet` - Only prints the command names
*  `--verbose` - Verbose install and run

## Testing install during development

Just cd to `src/dotnet-which` and run `.\pack.ps1` or `dotnet pack -C Release -o ../nupkg`.

Then cd to `src/nupkg` and run `dotnet install tool -g dotnet-which`.

## Maintainers/Core team

* [Giovanni Bassi](http://blog.lambda3.com.br/L3/giovannibassi/), aka Giggio, [Lambda3](http://www.lambda3.com.br), [@giovannibassi](https://twitter.com/giovannibassi)

Contributors can be found at the [contributors](https://github.com/lambda3/dotnet-which/graphs/contributors) page on Github.

## Contact

Use Twitter.

## License

This software is open source, licensed under the Apache License, Version 2.0.
See [LICENSE.txt](https://github.com/lambda3/dotnet-which/blob/master/LICENSE.txt) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project, and agree to
mention clearly if do not agree to these terms. Your work will be licensed with the project at Apache V2, along the rest of the code.
