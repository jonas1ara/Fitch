module DisplayInfoTest

open Xunit
open Lib

[<Fact>]
let ``Load logo test`` () =
  // Test que se pueda cargar un logo PNG
  let logo = DisplayInfo.loadLogo "linux"
  Assert.NotNull(logo)

