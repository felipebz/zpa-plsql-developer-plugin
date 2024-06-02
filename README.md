# Z PL/SQL Analyzer for PL/SQL Developer

[![Build](https://github.com/felipebz/zpa-plsql-developer-plugin/actions/workflows/build.yml/badge.svg)](https://github.com/felipebz/zpa-plsql-developer-plugin/actions/workflows/build.yml)

The [Z PL/SQL Analyzer](https://github.com/felipebz/zpa) (or simply ZPA) is a code analyzer for PL/SQL and Oracle SQL code. This repository contains the ZPA plugin for [Allround Automations PL/SQL Developer](https://www.allroundautomations.com/products/pl-sql-developer/).

## Requisites

- .NET Framework 4.6
- Java 11

## Installation

Note that this is work in progress and it has a lot of rough edges.

* Download the plugin from the [release](https://github.com/felipebz/zpa-plsql-developer-plugin/releases/tag/early-access).

* Extract the files to `<PLSQL Developer folder>\PlugIns`.

* Run PL/SQL Developer and confirm that the plugin was recognized (menu Tools > Configure Plug-Ins).

![Plug-In Manager listing the Z PL/SQL Analyzer](docs/plugin-manager.png)

## Usage

Open a program file and execute an analysis using the option "Analyze with ZPA" in the Tools menu.

![Example of the result window](docs/example.png)
