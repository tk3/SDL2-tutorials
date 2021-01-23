open FSharp.SDL2
open System
open System.Text

let screenWidth = 640
let screenHeight = 480

let mutable windowPtr: IntPtr = IntPtr.Zero
let mutable screenSurfacePtr: IntPtr = IntPtr.Zero
let mutable helloworldPtr: IntPtr = IntPtr.Zero

let Init(): bool =
    if SDL.Init(SDL.INIT_VIDEO) < 0 then
        printfn "SDL could not initialize! SDL_Error: %s\n" (SDL.GetError())
        false
    else
        let title = "SDL_Tutorial"
        let utf16bytes = Encoding.Unicode.GetBytes(title)
        let utf8bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16bytes)

        windowPtr <- SDL.CreateWindow(utf8bytes, SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED, screenWidth, screenHeight, SDL.WindowFlags.WINDOW_SHOWN)
        if windowPtr = IntPtr.Zero then
            printfn "Window could not be created! SDL_Error: %s\n" (SDL.GetError())
            false
        else
            screenSurfacePtr <- (SDL.GetWindowSurface(windowPtr))
            printfn "Init() succeed."
            true


let LoadMedia(): bool =
    let imageFilename = "assets/images/hello_world.bmp"
    let utf16bytes = Encoding.Unicode.GetBytes(imageFilename)
    let utf8bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16bytes)

    helloworldPtr <- SDL.LoadBMP(utf8bytes)
    if helloworldPtr = IntPtr.Zero then
        printfn "Unable to load image %s! SDL Error: %s\n" imageFilename (SDL.GetError())
        false
    else
        printfn "LoadMedia() succeed."
        true

let Close(): unit =
    SDL.FreeSurface(helloworldPtr)
    SDL.DestroyWindow(windowPtr)
    SDL.Quit()
    ()

[<EntryPoint>]
let main argv =
    Init() |> ignore

    LoadMedia() |> ignore

    SDL.UpperBlit(helloworldPtr, IntPtr.Zero, screenSurfacePtr, IntPtr.Zero) |> ignore

    SDL.UpdateWindowSurface(windowPtr) |> ignore

    SDL.Delay(2000u)

    Close() |> ignore

    0

