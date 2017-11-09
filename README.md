# BinaryTrees

This is [Binary Trees benchmark](http://benchmarksgame.alioth.debian.org/u64q/program.php?test=binarytrees&lang=fsharpcore&id=3)
modified so it's closer to the Java version.

It allocates ~300M objects

![image](https://user-images.githubusercontent.com/873919/32607347-ba8c01b8-c569-11e7-99f0-174e157bd91f.png)

And it spends almost all the time allocating and GC-ing

![image](https://user-images.githubusercontent.com/873919/32607550-6b61529a-c56a-11e7-9491-e07dc628827b.png)

Output on Core i7-6700 Windows 10 machine, .NET Core 2.0
```
stretch tree of depth 22         check: 8388607
2097152  trees of depth 4        check: 65011712
524288   trees of depth 6        check: 66584576
131072   trees of depth 8        check: 66977792
32768    trees of depth 10       check: 67076096
8192     trees of depth 12       check: 67100672
2048     trees of depth 14       check: 67106816
512      trees of depth 16       check: 67108352
128      trees of depth 18       check: 67108736
32       trees of depth 20       check: 67108832
long lived tree of depth 21      check: 4194303
Elapsed 00:00:12.5967918
```
