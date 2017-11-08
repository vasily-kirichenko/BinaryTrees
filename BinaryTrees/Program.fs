open System
open System.Linq
open System.Diagnostics

[<Struct>]
type Tree = { Next: Next }
and Next = { 
    Left: Tree 
    Right: Tree }

let rec check (tree: Tree) =
    if isNull (box tree.Next) then
        1
    else 
        1 + check(tree.Next.Right) + check(tree.Next.Left)

let rec bottomUpTree (depth: int) =
    if depth > 0 then
        { Next = 
            { Left = bottomUpTree (depth - 1)
              Right = bottomUpTree (depth - 1) }}
    else 
        { Next = Unchecked.defaultof<_> } 

let inner (depth: int) (iterations: int) =
    if depth > 18 then
        [ 1..iterations ]
            .AsParallel()
            .Select(fun _ -> check (bottomUpTree depth))
            .Sum()
    else
        let mutable sum = 0
        for _ in 1..iterations do
            sum <- sum + check (bottomUpTree depth)
        sum
    |> sprintf "%d\t trees of depth %d\t check: %d" iterations depth
    
[<EntryPoint>]
let main args =
    let sw = Stopwatch.StartNew()
    let minDepth = 4
    let maxDepth =
        let n = match args with [|x|] -> int x | _ -> 10
        if minDepth + 2 > n then minDepth + 2 else n
    (
        let depth = maxDepth + 1
        let c = check (bottomUpTree depth)
        printfn "stretch tree of depth %d\t check: %d" depth c
    )
    
    let longLivedTree = bottomUpTree maxDepth
    
    let messages =
        [| for halfDepth in minDepth/2..maxDepth/2 do
             yield async {
                let depth = halfDepth * 2
                let iterations = 1 <<< (maxDepth - depth + minDepth)
                let result = inner depth iterations
                return result
             } 
        |]
        |> Async.Parallel
        |> Async.RunSynchronously
            
    for message in messages do
        printfn "%s" message
    
    printfn "Elapsed %O" sw.Elapsed
    exit 0