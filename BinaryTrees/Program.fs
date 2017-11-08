open System
open System.Diagnostics

type Next = { Left: Tree; Right: Tree }
and [<Struct>] Tree(next: Next) =
    member t.Check() =
        match box next with 
        | null -> 1
        | _ -> 1 + next.Left.Check() + next.Right.Check()

let rec make depth =
    if depth > 0 then Tree({Left = make (depth-1); Right=make (depth-1)})
    else Tree(Unchecked.defaultof<_>)

let inline check (tree: Tree) = tree.Check()

let rec loopDepths maxDepth minDepth d =
    if d <= maxDepth then
        let niter = 1 <<< (maxDepth - d + minDepth)
        let mutable c = 0
        for i = 1 to niter do
            c <- c + check (make d)
        printfn "%d\t trees of depth %d\t check: %d" niter d c
        loopDepths maxDepth minDepth (d + 2)

[<EntryPoint>]
let main args =
    let sw = Stopwatch.StartNew()
    let minDepth = 4
    let maxDepth =
        let n = if args.Length > 0 then int args.[0] else 10
        max (minDepth + 2) n
    let stretchDepth = maxDepth + 1

    let c = check (make stretchDepth)
    printfn "stretch tree of depth %d\t check: %d" stretchDepth c
    let longLivedTree = make maxDepth
    loopDepths maxDepth minDepth minDepth
    printfn "long lived tree of depth %d\t check: %d" maxDepth (check longLivedTree)
    printfn "Elapsed %O" sw.Elapsed
    exit 0