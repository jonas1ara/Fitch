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
[<InlineData("Windows 10")>]
[<InlineData("Windows 11")>]
[<InlineData("windows")>]
let ``Embedded logo resource exists for supported distros`` (distro: string) =
  let normalized = DisplayInfo.normalizeLogoName distro
  let resourceName = $"Lib.Logos.{normalized}.png"
  use stream = typeof<Lib.Types.Info>.Assembly.GetManifestResourceStream(resourceName)
  Assert.NotNull(stream)
  let logo = DisplayInfo.loadLogo distro
  Assert.NotNull(logo)

[<Fact>]
let ``Fallback logo loads for unknown distro`` () =
  let logo = DisplayInfo.loadLogo "unknown-distro-fallback"
  Assert.NotNull(logo)

