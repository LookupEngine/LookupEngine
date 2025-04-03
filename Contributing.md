## Table of contents

<!-- TOC -->
  * [Fork, Clone, Branch and Create your PR](#fork-clone-branch-and-create-your-pr)
  * [Rules](#rules)
  * [Building](#building)
    * [Prerequisites](#prerequisites)
    * [Compiling Source Code](#compiling-source-code)
  * [Solution structure](#solution-structure)
  * [Publishing Releases](#publishing-releases)
    * [Updating the Changelog](#updating-the-changelog)
    * [Creating a new Release from the JetBrains Rider](#creating-a-new-release-from-the-jetbrains-rider)
    * [Creating a new Release from the Terminal](#creating-a-new-release-from-the-terminal)
<!-- TOC -->

## Fork, Clone, Branch and Create your PR

1. Fork the repo if you haven't already.
2. Clone your fork locally.
3. Create & push a feature branch.
4. Create a [Draft Pull Request (PR)](https://github.blog/2019-02-14-introducing-draft-pull-requests/).
5. Work on your changes.

## Rules

- Follow the pattern of what you already see in the code.
- When adding new classes/methods/changing existing code: 
  - Run the debugger and make sure everything works.
  - Add appropriate XML documentation comments.
  - Follow C# coding conventions.
  - Write unit tests for new functionality.
- The naming should be descriptive and direct, giving a clear idea of the functionality.
- Keep commits atomic and write meaningful commit messages.
- Update documentation when changing public APIs.
- Follow semantic versioning guidelines for releases.
- Run tests locally before submitting PR.
- Address code review feedback promptly.

## Building

### Prerequisites

- Windows 10 or newer.
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48).
- [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet).
- [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/).
- [Git](https://git-scm.com/downloads).

### Compiling Source Code

We recommend JetBrains Rider as preferred IDE, since it has outstanding .NET support. If you don't have Rider installed, you can download it
from [here](https://www.jetbrains.com/rider/).

1. Open IDE.
2. Open the solution file `LookupEngine.sln`.
3. In the `Solutions Configuration` drop-down menu, select `Debug` configuration.
4. After the solution loads, you can build it by clicking on `Build -> Build Solution`.
5. Use the `Debug` button to start debugging.

## Solution structure

| Folder  | Description                                                                      |
|---------|----------------------------------------------------------------------------------|
| build   | Nuke build system. Used to automate project builds, releases and CI/CD pipelines |
| source  | Project source code folder containing core library projects:                     |
|         | - LookupEngine: Main library implementation                                      |
|         | - LookupEngine.Abstractions: Public interfaces and models                        |
| tests   | Test projects:                                                                   |
|         | - LookupEngine.Tests.Unit: Unit tests                                            |
|         | - LookupEngine.Tests.Performance: Performance benchmarks                         |
| .github | GitHub specific files (workflows, templates, etc)                                |
| .run    | Run configurations for JetBrains Rider                                           |
| .nuke   | Build system configuration                                                       |

## Publishing Releases

Releases are managed by creating new [Git tags](https://git-scm.com/book/en/v2/Git-Basics-Tagging).
A tag in Git used to capture a snapshot of the project at a particular point in time, with the ability to roll back to a previous version.

Tags must follow the format `version` or `version-stage.n.date` for pre-releases, where:

- **version** specifies the version of the release:
    - `1.0.0`
    - `2.3.0`
- **stage** specifies the release stage:
    - `alpha` - represents early iterations that may be unstable or incomplete.
    - `beta` - represents a feature-complete version but may still contain some bugs.
- **n** prerelease increment (optional):
    - `1` - first alpha prerelease
    - `2` - second alpha prerelease
- **date** specifies the date of the pre-release (optional):
    - `250101`
    - `20250101`

For example:

| Stage   | Version                |
|---------|------------------------|
| Alpha   | 1.0.0-alpha            |
| Alpha   | 1.0.0-alpha.1.20250101 |
| Beta    | 1.0.0-beta.2.20250101  |
| Release | 1.0.0                  |

### Updating the Changelog

For releases, changelog for the release version is required.

To update the changelog:

1. Navigate to the solution root.
2. Open the file **Changelog.md**.
3. Add a section for your version. The version separator is the `#` symbol.
4. Specify the release number e.g. `# 1.0.0` or `# 25.01.01 v1.0.0`, the format does not matter, the main thing is that it contains the version.
5. In the lines below, write a changelog for your version, style to your taste. For example, you will find changelog for version 1.0.0, do the same.
6. Make a commit.

### Creating a new Release from the JetBrains Rider

Publishing a release begins with the creation of a new tag:

1. Open JetBrains Rider.
2. Navigate to the **Git** tab.
3. Click **New Tag...** and create a new tag.

   ![image](https://github.com/user-attachments/assets/19c11322-9f95-45e5-8fe6-defa36af59c4)

4. Navigate to the **Git** panel.
5. Expand the **Tags** section.
6. Right-click on the newly created tag and select **Push to origin**.

   ![image](https://github.com/user-attachments/assets/b2349264-dd76-4c21-b596-93110f1f16cb)

   This process will trigger the Release workflow and create a new Release on GitHub.

### Creating a new Release from the Terminal

Alternatively, you can create and push tags using the terminal:

1. Navigate to the repository root and open the terminal.
2. Use the following command to create a new tag:
   ```shell
   git tag 'version'
   ```

   Replace `version` with the desired version, e.g., `1.0.0`.
3. Push the newly created tag to the remote repository using the following command:
   ```shell
   git push origin 'version'
   ```

> [!NOTE]  
> The tag will reference your current commit, so verify you're on the correct branch and have fetched latest changes from remote first.