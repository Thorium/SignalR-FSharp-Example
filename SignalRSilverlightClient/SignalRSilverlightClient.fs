module SignalRSilverlightClient

#if INTERACTIVE
#else
let aspnetUrl = 
    let hs = System.Windows.Application.Current.Host.Source
    hs.Scheme + "://" + hs.Host + ":" + hs.Port.ToString();
#endif
let owinurl = "http://localhost:8080/"

open Microsoft.AspNet.SignalR.Client.Hubs
open System
open System.Linq
open System.ComponentModel.Composition
open System.Reactive.Subjects
open System.Threading.Tasks
open System.Threading

// SignalR supports two kinds of connections: Hub and PersistentConnection

let MakePersistentConnection url =

    let gotResult = new Subject<string>()    
    let connection = new Microsoft.AspNet.SignalR.Client.Connection(url + "/signalrConn")

    connection.add_Received(fun r -> gotResult.OnNext(r))
    connection.add_Error(fun e -> gotResult.OnError(e))
    //connection.add_Reconnected(fun r -> ignore())
    
    let handleExceptions (task:Task) =
        match task.Exception with
        | null -> "none" |> ignore
        | ex -> gotResult.OnError(ex.InnerExceptions.[0])

    connection.Start()
        .ContinueWith(handleExceptions, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default)
    |> ignore
        
    gotResult :> IObservable<string>


// Hub uses JSON replies.
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let MakeHubConnection url msgToSend =

    let gotResult = new Subject<string>()

    let connection = new HubConnection(url + "/signalrHub")
    let myhub = connection.CreateHubProxy("myhub")
    let invokeHub (task:Task) = 
        let result = myhub.Subscribe("myCustomClientFunction")
        myhub.Invoke("MyCustomServerFunction", msgToSend).Wait()
        result

    connection.add_Received(fun json -> JObject.Parse(json).["A"].First.ToString() |> gotResult.OnNext)
    connection.add_Error(fun e -> gotResult.OnError(e))
    //connection.add_Reconnected(fun r -> ignore())
    
    let handleExceptions (task:Task) =
        match task.Exception with
        | null -> "none" |> ignore
        | ex -> gotResult.OnError(ex.InnerExceptions.[0])

    connection.Start()
        .ContinueWith(handleExceptions, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default)
        .ContinueWith(Func<Task, Subscription>(invokeHub))
    |> ignore
        
    gotResult :> IObservable<string>
