open FSharp.SDL2

[<EntryPoint>]
let main argv =
    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore
    0
