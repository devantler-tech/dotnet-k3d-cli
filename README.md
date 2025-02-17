# ③ .NET K3d CLI

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler/dotnet-k3d-cli/actions/workflows/test.yaml/badge.svg)](https://github.com/devantler/dotnet-k3d-cli/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler/dotnet-k3d-cli/graph/badge.svg?token=RhQPb4fE7z)](https://codecov.io/gh/devantler/dotnet-k3d-cli)

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->
```
.
├── .github
│   ├── scripts
│   └── workflows
├── Devantler.K3dCLI
│   └── runtimes
│       ├── linux-arm64
│       │   └── native
│       ├── linux-x64
│       │   └── native
│       ├── osx-arm64
│       │   └── native
│       ├── osx-x64
│       │   └── native
│       └── win-x64
│           └── native
└── Devantler.K3dCLI.Tests
    └── K3dTests

18 directories
```
<!-- readme-tree end -->

</details>

A simple .NET library that embeds the K3d CLI.

## 🚀 Getting Started

To get started, you can install the package from NuGet.

```bash
dotnet add package Devantler.K3dCLI
```

## 📝 Usage

You can execute the K3d CLI commands using the `K3d` class.

```csharp
using Devantler.K3dCLI;

var (exitCode, output) = await K3d.RunAsync(["arg1", "arg2"]);
```
