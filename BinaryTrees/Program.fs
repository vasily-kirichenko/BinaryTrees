open System
open System.Threading
open System.Linq
open System.Diagnostics
open System.Threading.Tasks

[<Sealed; AllowNullLiteral>]
type TreeNode(left: TreeNode, right: TreeNode) =
    member this.itemCheck() = if isNull left then 1 else 1 + left.itemCheck() + right.itemCheck()

let empty = TreeNode(null, null)

let MIN_DEPTH = 4

let rec bottomUpTree(depth: int) : TreeNode =
    if depth > 0 then
        TreeNode(bottomUpTree(depth - 1), bottomUpTree(depth - 1))
    else
        empty

[<EntryPoint>]
let main args =
    let sw = Stopwatch.StartNew()
    let n = match args with [|n|] -> int n | _ -> 0
    let maxDepth = if n < MIN_DEPTH + 2 then MIN_DEPTH + 2 else n
    let stretchDepth = maxDepth + 1

    printfn "stretch tree of depth %d\t check: %d" stretchDepth (bottomUpTree(stretchDepth).itemCheck())

    let longLivedTree = bottomUpTree maxDepth

    let results =
        [| for depth in MIN_DEPTH..2..maxDepth do
             yield async {
                let iterations = 1 <<< (maxDepth - depth + MIN_DEPTH)
                let check = Array.init iterations (fun _ -> bottomUpTree(depth).itemCheck()) |> Array.sum
                return sprintf "%d\t trees of depth %d\t check: %d" iterations depth check
             } |]
        |> Async.Parallel
        |> Async.RunSynchronously

    results |> Array.iter (printfn "%s")

    printfn "long lived tree of depth %d\t check: %d" maxDepth (longLivedTree.itemCheck())
    printfn "Elapsed %O" sw.Elapsed
    Console.ReadLine() |> ignore
    exit 0