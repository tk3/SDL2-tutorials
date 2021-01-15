open FSharp.SDL2

[<EntryPoint>]
let main argv =
    SDL.SDL_Init(0u) |> ignore
    0
