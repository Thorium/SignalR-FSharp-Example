module SignalRSilverlightClient

open Microsoft.AspNet.SignalR.Client.Hubs
open System
open System.ComponentModel.Composition
open System.Reactive.Subjects
open System.Threading.Tasks
open System.Threading

let MakeConnection() =
    //let url = "http://localhost:8080/"

    let url =
        let hs = System.Windows.Application.Current.Host.Source
        hs.Scheme + "://" + hs.Host + ":" + hs.Port.ToString();

    let gotResult = new Subject<string>()
    
    //SignalR supports two kinds of connections: Hub and PersistentConnection
    //While server supports both, we use PersistentConnection the client currently uses PersistentConnection
    let connection = new Microsoft.AspNet.SignalR.Client.Connection(url + "/signalrConn")
    //let connection = new HubConnection(url + "/signalrHub")
    //let myhub = connection.CreateHubProxy("myhub")

    connection.add_Received(fun r -> gotResult.OnNext(r))
    connection.add_Error(fun e -> gotResult.OnError(e))
    //connection.add_Reconnected(fun r -> ignore())
    
    let handleExceptions (task:Task) =
        match task.Exception with
        | null -> "none" |> ignore
        | ex -> gotResult.OnError(ex.InnerExceptions.[0])

    connection.Start().ContinueWith(handleExceptions, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default) |> ignore
    
    //myhub.Invoke("MyCustomFunction").Wait()

    gotResult :> IObservable<string>