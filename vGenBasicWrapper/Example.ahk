#SingleInstance force

; Load CLR library that allows us to load C# DLLs
#include CLR.ahk

; Instantiate class from C# DLL
asm := CLR_LoadLibrary("vGenBasicWrapper.dll")
vGen := asm.CreateInstance("vGenBasicWrapper")

vj := vGen.vJoy.Device(1)
vx := vGen.vXbox.Device(1)

JoyPos := 0
JoyDir := -1
JoyBtn := 1

SetTimer, UpdatevJoy, 100
return

UpdatevJoy:
	vj.SetButton(JoyBtn, 0)
	vx.SetButton(JoyBtn, 0)
	
	vj.SetAxis(1, JoyPos)
	vx.SetAxis(1, JoyPos)
	
	JoyBtn++
	if (JoyBtn > 4)
		JoyBtn := 1
	
	vj.SetButton(JoyBtn, 1)
	vx.SetButton(JoyBtn, 1)
	
	vj.SetPov(1, JoyPos)
	vx.SetPov(1, JoyPos)
	
	if (JoyPos == 100 || JoyPos == 0)
		JoyDir *= -1
	JoyPos += (JoyDir * 2)
	return

^Esc::
	ExitApp