//Host path
#r "System.Windows.dll"

//Reactive Extensions:
#r "System.ComponentModel.Composition.dll"
#r "../packages/Rx-Interfaces.2.1.30214.0/lib/Net45/System.Reactive.Interfaces.dll"
#r "../packages/Rx-Core.2.1.30214.0/lib/Net45/System.Reactive.Core.dll"
#r "../packages/Rx-Linq.2.1.30214.0/lib/Net45/System.Reactive.Linq.dll"

//SignalR Client:
#r "../packages/Newtonsoft.Json.5.0.8/lib/net45/Newtonsoft.Json.dll"
#r "../packages/Microsoft.AspNet.SignalR.Client.2.0.0/lib/net45/Microsoft.AspNet.SignalR.Client.dll"

#load "SignalRSilverlightClient.fs"
open SignalRSilverlightClient


let client1 : Connection =     
    MakePersistentConnection owinurl

client1.ResultFeed |> Observable.subscribe System.Console.WriteLine

let client2 : Connection =     
    MakeHubConnection owinurl // [|box("hello there!")|]

client2 |> Observable.subscribe System.Console.WriteLine
