module SystemInfo

open System
open Lib
open Xunit

[<Fact>]
let ``Get info`` () =
  let info = SystemInfo.systemInfo ()
  printfn $"%A{info}"
  Assert.False(String.IsNullOrWhiteSpace(info.distroName), "distroName should not be empty")
  Assert.False(String.IsNullOrWhiteSpace(info.kernelName), "kernelName should not be empty")
  Assert.False(String.IsNullOrWhiteSpace(info.user), "user should not be empty")
  Assert.False(String.IsNullOrWhiteSpace(info.hostName), "hostName should not be empty")
  Assert.False(String.IsNullOrWhiteSpace(info.memInfo), "memInfo should not be empty")

[<Fact>]
let ``Render displayInfo does not throw`` () =
  let ex = Record.Exception(fun () -> DisplayInfo.displayInfo ())
  Assert.Null(ex)
