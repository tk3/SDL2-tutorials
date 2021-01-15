namespace FSharp.SDL2

open System.Runtime.InteropServices

module SDL =
    [<Literal>]
    let DLL_NAME = "SDL2.dll"

    let SDL_INIT_VIDEO = 20u

    // SDL.h
    [<DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_Init(uint32 flags)

