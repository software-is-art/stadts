# StaDTs

StaDTs stands for static algebraic data types.

Heavily inspired by [Dunet](https://github.com/domn1995/dunet) StaDTs aims to eliminate some of the runtime cost of relying on inheritance to form union types.

StaDTs achieves this by using a Roslyn source generator + C# 11 static abstract interfaces.

The end result is the ability to abstract over types without using inheritance. 

Leaning on static abstracts also allows StaDTs to provide unions over structs.
