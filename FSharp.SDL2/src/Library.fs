namespace FSharp.SDL2

open System
open System.Text
open System.Runtime.InteropServices

module SDL =
    [<Literal>]
    let dllName = "SDL2.dll"

    let INIT_VIDEO = 20u

    let WINDOWPOS_CENTERED_MASK = 0x2FFF0000
    let WINDOWPOS_CENTERED_DISPLAY x = WINDOWPOS_CENTERED_MASK ||| x
    let WINDOWPOS_CENTERED = WINDOWPOS_CENTERED_DISPLAY(0)
    let WINDOWPOS_ISCENTERED x = (x &&& 0xFFFF0000) = WINDOWPOS_CENTERED_MASK


    ////////////////////////////////////////////////////////////////////////////
    // SDL.h
    [<DllImport(dllName, EntryPoint = "SDL_Init", CallingConvention = CallingConvention.Cdecl)>]
    extern int Init(uint32 flags)

    [<DllImport(dllName, EntryPoint = "SDL_Quit", CallingConvention = CallingConvention.Cdecl)>]
    extern void Quit()


    ////////////////////////////////////////////////////////////////////////////
    // SDL_error.h
    [<DllImport(dllName, EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)>]
    extern string GetError()


    ////////////////////////////////////////////////////////////////////////////
    // SDL_pixels.h

    [<StructLayout(LayoutKind.Sequential)>]
    type Color =
        struct
            val r: uint8
            val g: uint8
            val b: uint8
            val a: uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type Palette =
        struct
            val ncolors: int
            val colors: IntPtr    // SDL_Color*
            val version: uint32
            val refcount: int
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal PixelFormat =
        struct
            val format: uint32
            val palette: IntPtr    // SDL_Palette*
            val BitsPerPixel: uint8
            val BytesPerPixel: uint8
            val padding: uint16    // Uint8 padding[2]
            val Rmask: uint32
            val Gmask: uint32
            val Bmask: uint32
            val Amask: uint32
            val Rloss: uint8
            val Gloss: uint8
            val Bloss: uint8
            val Aloss: uint8
            val Rshift: uint8
            val Gshift: uint8
            val Bshift: uint8
            val Ashift: uint8
            val refcount: int
            val next: IntPtr    // SDL_PixelFormat*
        end

    [<DllImport(dllName, EntryPoint = "SDL_MapRGB", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern uint32 SDL_MapRGB(SDL_PixelFormat* format, uint8 r, uint8 g, uint8 b)
    extern uint32 MapRGB(IntPtr format, uint8 r, uint8 g, uint8 b)


    ////////////////////////////////////////////////////////////////////////////
    // SDL_rect.h
    [<StructLayout(LayoutKind.Sequential)>]
    type Rect =
        struct
            val x: int
            val y: int
            val w: int
            val h: int
        end


    ////////////////////////////////////////////////////////////////////////////
    // SDL_surface.h
    [<StructLayout(LayoutKind.Sequential)>]
    type Surface =
        struct
            val flags: uint32
            val format: IntPtr    // SDL_PixelFormat*
            val w: int
            val h: int
            val pitch: int
            val pixels: IntPtr    // void*
            val userdata: IntPtr    // void*
            val locked: int
            val list_blitmap: IntPtr    // void*
            val clip_rect: Rect
            val map: IntPtr    // struct SDL_BlitMap *
            val refcount: int
        end

    [<DllImport(dllName, EntryPoint = "SDL_FillRect", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern int SDL_FillRect(SDL_Surface * dst, SDL_Rect * rect, uint32 color)
    extern int FillRect(IntPtr dst, IntPtr rect, uint32 color)

    [<DllImport(dllName, EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)>]
    // extern DECLSPEC int SDLCALL SDL_UpperBlit (SDL_Surface * src, const SDL_Rect * srcrect, SDL_Surface * dst, SDL_Rect * dstrect);
    extern int UpperBlit(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)

    [<DllImport(dllName, EntryPoint = "SDL_LoadBMP_RW", CallingConvention = CallingConvention.Cdecl)>]
    // extern DECLSPEC SDL_Surface *SDLCALL SDL_LoadBMP_RW(SDL_RWops * src, int freesrc);
    extern IntPtr LoadBMP_RW(IntPtr src, int freesrc)

    [<DllImport(dllName, EntryPoint = "SDL_FreeSurface", CallingConvention = CallingConvention.Cdecl)>]
    // extern DECLSPEC void SDLCALL SDL_FreeSurface(SDL_Surface * surface);
    extern void FreeSurface(IntPtr surface)


    ////////////////////////////////////////////////////////////////////////////
    // SDL_rwops.h

    [<DllImport(dllName, EntryPoint = "SDL_RWFromFile", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern DECLSPEC SDL_RWops *SDLCALL SDL_RWFromFile(const char *file, const char *mode);
    extern IntPtr RWFromFile(byte[] file, byte[] mode)


    ////////////////////////////////////////////////////////////////////////////
    // SDL_surface.h

    // #define SDL_LoadBMP(file)   SDL_LoadBMP_RW(SDL_RWFromFile(file, "rb"), 1
    let LoadBMP(file: byte[]): IntPtr =
        let utf16bytes = Encoding.Unicode.GetBytes("rb")
        let utf8bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16bytes)
        let file = RWFromFile(file, utf8bytes)
        LoadBMP_RW(file, 1)


    ////////////////////////////////////////////////////////////////////////////
    // SDL_timer.h

    [<DllImport(dllName, EntryPoint = "SDL_Delay", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    extern void Delay(uint32 ms)


    ////////////////////////////////////////////////////////////////////////////
    // SDL_video.h

    type WindowFlags =
        | WINDOW_FULLSCREEN = 0x00000001
        | WINDOW_OPENGL = 0x00000002
        | WINDOW_SHOWN = 0x00000004
        | WINDOW_HIDDEN = 0x00000008
        | WINDOW_BORDERLESS = 0x00000010
        | WINDOW_RESIZABLE = 0x00000020
        | WINDOW_MINIMIZED = 0x00000040
        | WINDOW_MAXIMIZED = 0x00000080
        | WINDOW_INPUT_GRABBED = 0x00000100
        | WINDOW_INPUT_FOCUS = 0x00000200
        | WINDOW_MOUSE_FOCUS = 0x00000400
        | WINDOW_FULLSCREEN_DESKTOP = 0x00001001  // SDL_WINDOW_FULLSCREEN ||| 0x00001000
        | WINDOW_FOREIGN = 0x00000800
        | WINDOW_ALLOW_HIGHDPI = 0x00002000
        | WINDOW_MOUSE_CAPTURE = 0x00004000
        | WINDOW_ALWAYS_ON_TOP = 0x00008000
        | WINDOW_SKIP_TASKBAR = 0x00010000
        | WINDOW_UTILITY = 0x00020000
        | WINDOW_TOOLTIP = 0x00040000
        | WINDOW_POPUP_MENU = 0x00080000
        | WINDOW_VULKAN = 0x10000000
        | WINDOW_METAL = 0x20000000


    [<DllImport(dllName, EntryPoint = "SDL_CreateWindow", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern IntPtr CreateWindow(string title, int x, int y, int w, int h, WindowFlags flags)
    extern IntPtr CreateWindow(byte[] title, int x, int y, int w, int h, WindowFlags flags)

    [<DllImport(dllName, EntryPoint = "SDL_GetWindowSurface", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern DECLSPEC SDL_Surface * SDLCALL SDL_GetWindowSurface(SDL_Window * window);
    extern IntPtr GetWindowSurface(IntPtr window)

    [<DllImport(dllName, EntryPoint = "SDL_UpdateWindowSurface", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)>]
    // extern int SDL_UpdateWindowSurface(SDL_Window * window)
    extern int UpdateWindowSurface(IntPtr window)

    [<DllImport(dllName, EntryPoint = "SDL_DestroyWindow", CallingConvention = CallingConvention.Cdecl)>]
    extern void DestroyWindow(IntPtr window)

