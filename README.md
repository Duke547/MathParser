# Math Parser

A simple tool for parsing mathematical expressions.

## How to Use
```C#
Parser.Parse("3 * 2 + 1")          // returns 7m
Parser.Parse("3 * (2 + 1)")        // returns 9m
Parser.Parse("1 + 2 + (-(-3))")    // returns 6m
```

Invalid inputs will throw a `TokenException`.

```C#
Parser.Parse("1 + 1 +")    // throws TokenException "Unexpected token '+'"
Parser.Parse("(1 + 1")     // throws MissingTokenException "Missing ')'"
```
## Supported Operations
|Operation|Type|Symbols|
|:-------:|:-----:|:-----:|
|Negate   |Unary  |-      |
|Add      |Binary |+      |
|Subtract |Binary |-      |
|Multiply |Binary |*, ×, ∙|
|Divide   |Binary |/, ÷   |
|Remainder|Binary |%      |
