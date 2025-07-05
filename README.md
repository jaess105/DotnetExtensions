# Extensions

This is a simple dotnet solution providing some Extensions functions I regularly use.

The main usage of this repo is adding one or multiple projects to another solution and then using the project(s)
in another project.
Currently there are no plans publishing this to NuGet.

## Adding to Solution

To add one of the packages to a different solution and then its package, do the following:

Go to the solution you want to add this to and type:

```sh
dotnet sln add <path-to-repo>/<project>/<project>.csproj
```

Then in the project where you want to add it type

```sh
dotnet add reference <path-to-repo>/<project>/<project>.csproj
```

## License

This repo is licensed under the [MIT](LICENSE) license.

