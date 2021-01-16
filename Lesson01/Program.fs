open FSharp.SDL2
open System
open System.Runtime.InteropServices
open System.Text

[<EntryPoint>]
let main argv =
    if SDL.Init(SDL.INIT_VIDEO) < 0 then
        printfn"SDL could not initialize! SDL_Error: %s\n" (SDL.GetError())
        Environment.Exit 1

    let title = "SDL_Tutorial"
    let utf16bytes = Encoding.Unicode.GetBytes(title)
    let utf8bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16bytes)

    let screenWidth = 640
    let screenHeight = 480
    let window = SDL.CreateWindow(utf8bytes, SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED, screenWidth, screenHeight, SDL.WindowFlags.WINDOW_SHOWN)

    let screenSurfacePtr = SDL.GetWindowSurface(window)

    let screenSurface: SDL.Surface = Marshal.PtrToStructure(screenSurfacePtr, typeof<SDL.Surface>) :?> SDL.Surface

    let mapRgb = SDL.MapRGB(screenSurface.format, 0xffuy, 0xffuy, 0xffuy)
    SDL.FillRect(screenSurfacePtr, IntPtr.Zero, mapRgb) |> ignore

    SDL.UpdateWindowSurface(screenSurfacePtr) |> ignore

    SDL.Delay(2000u)

    SDL.DestroyWindow(window)

    SDL.Quit()

    0

