#SingleInstance force

asm := CLR_LoadLibrary("vGenBasicWrapper.dll")
vGen := asm.CreateInstance("vGenBasicWrapper")

vj := vGen.vJoy.Device(1)

vj.SetAxis(1, 100)				; Set axis 1 to 100%
Sleep, 1000
vj.SetAxis(1, 0)				; Set axis 1 to 0%
