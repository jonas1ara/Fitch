module DisplayInfoTest

open Xunit
open Lib

[<Theory>]
[<InlineData("linux")>]
[<InlineData("ubuntu")>]
[<InlineData("debian")>]
[<InlineData("fedora")>]
[<InlineData("arch")>]
[<InlineData("nixos")>]
[<InlineData("alpine")>]
[<InlineData("linuxmint")>]
[<InlineData("windows")>]
[<InlineData("unknown-distro-fallback")>]
let ``Load logo test`` (distro: string) =
  let logo = DisplayInfo.loadLogo distro
  Assert.NotNull(logo)

