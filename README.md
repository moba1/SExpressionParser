# SExpressionParser  [![Build Status](https://travis-ci.org/moba1/SExpressionParser.svg?branch=master)](https://travis-ci.org/moba1/SExpressionParser)
C# S Expression Parser Library

## Usage

 1. build this solution
 2. add reference to project

## Example

```cs
var parser = new SExpressionParser.Parser();

Dump.Dumps(parser.Parse("(\"Hello\" (\"World\"))");
```

