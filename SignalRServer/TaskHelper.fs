﻿module TaskHelper

open System.Threading.Tasks
// Source code from: http://theburningmonk.com/2012/10/f-helper-functions-to-convert-between-asyncunit-and-task/

[<AutoOpen>]
module Async =
    let inline awaitPlainTask (task: Task) = 
        // rethrow exception from preceding task if it fauled
        let continuation (t : Task) : unit =
            match t.IsFaulted with
            | true -> raise t.Exception
            | arg -> ()
        task.ContinueWith continuation |> Async.AwaitTask
 
    let inline startAsPlainTask (work : Async<unit>) = Task.Factory.StartNew(fun () -> work |> Async.RunSynchronously)