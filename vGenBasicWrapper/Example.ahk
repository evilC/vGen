#SingleInstance force

asm := CLR_LoadLibrary("vGenBasicWrapper.dll")
vGen := asm.CreateInstance("vGenBasicWrapper")

vj := vGen.vJoy.Device(1)
vx := vGen.vXbox.Device(1)

JoyPos := 0
JoyDir := -1

SetTimer, UpdatevJoy, 10
return

UpdatevJoy:
	vj.SetAxis(1, JoyPos)
	vx.SetAxis(1, JoyPos)
	
	if (JoyPos == 100 || JoyPos == 0)
		JoyDir *= -1
	JoyPos += JoyDir
	return

^Esc::
	ExitApp