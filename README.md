# Z PL/SQL Analyzer for PL/SQL Developer

[![Build Status](https://dev.azure.com/felipebz/z-plsql-analyzer/_apis/build/status/zpa-plsql-developer-plugin?branchName=master)](https://dev.azure.com/felipebz/z-plsql-analyzer/_build/latest?definitionId=11&branchName=master)

The [Z PL/SQL Analyzer](https://github.com/felipebz/zpa) (or simply ZPA) is a code analyzer for PL/SQL and Oracle SQL code. This repository contains the ZPA plugin for [Allround Automations PL/SQL Developer](https://www.allroundautomations.com/products/pl-sql-developer/).

## Installation

Note that this is work in progress and it has a lot of rough edges.

* Open the [build pipeline](https://dev.azure.com/felipebz/z-plsql-analyzer/_build/latest?definitionId=11&branchName=master) and download the plugin from the "Artifacts" button. You must the download the artifact corresponding to your PL/SQL Developer installation.

![Artifact download menu with the options 32-bit and 64-bit](docs/artifacts.png)

* Extract the file to <PLSQL Developer folder>\PlugIns.

* Run PL/SQL Developer and confirm that the plugin was recognized (menu Tools > Configure Plug-Ins).

![Plug-In Manager listing the Z PL/SQL Analyzer](docs/plugin-manager.png)

## Usage

Open a program file and execute an analysis using the option "Analyze with ZPA" in the Tools menu.

![Example of the result window](docs/example.png)
