# Peter

Peter is a set of tools that may help you creating and testing your ASP.NET Core Minimal API's.
It provides:

- A test server ready to use for integration testing of your API, supporting secured endpoints
- A type Result<T> useful to return both results and errors, from commands and queries, being able to map them to http results
- A solution for minimal api modules organization
- Extension methods to help you create thin action methods with Mediatr (not yet available)
- API url discovery tools (not yet available)

# Releases

* CI Build Status [![Build Status](https://github.com/StarskyCorp/Peter/actions/workflows/ci.yaml/badge.svg?ref=main)


# Where does the name "Peter" come from?

There is a spanish expression *"No lo usa ni Peter"* than means "nobody use it!".

As this package is pretty new, we supose nobody is using it yet. We hope it will change soon.

# Getting started

Peter provides several components, let's see how to start with some of them. For further information go to the component documentation

## Testing minimal Api's

...

# Contributting

Pending

# Acknowledgements

Peter is built using the someopen source projects like:

- XUnit
- Fluent Assertions
- MediatR

In addition, some of the Peter "tools" are inspired by awesome open source projects like:

- [Acheve Test Host by the xabaril team](https://github.com/Xabaril/Acheve.TestHost)
- [Carter, created by some NancyFx mantainers](https://github.com/CarterCommunity/Carter)
- [Result, created by Steve "Ardalis" Smith](https://github.com/ardalis/Result)
