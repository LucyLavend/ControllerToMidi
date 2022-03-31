extends Panel

onready var parent = get_parent().get_parent()

var inactive_style : StyleBoxFlat
var active_style : StyleBoxFlat

func _ready():
	inactive_style = get('custom_styles/panel').duplicate()
	inactive_style.set_bg_color(parent.BGColor)
	
	active_style = inactive_style.duplicate()
	active_style.set_bg_color(parent.BGColor - Color(.3, .3, .3, 0.0))
	
	set('custom_styles/panel', inactive_style)

func _input(event):
	if event is InputEventJoypadButton:
		print(event.device)
		if event.device == parent.ID:
			if event.is_pressed():
				set('custom_styles/panel', active_style)
				print(parent.ID)
			else:
				set('custom_styles/panel', inactive_style)
