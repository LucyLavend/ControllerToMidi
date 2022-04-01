extends Control

onready var controller_button = get_node("../MarginContainer/Panel/MarginContainer/HSplitContainer/GridContainer/Controller")
onready var guitar_button = get_node("../MarginContainer/Panel/MarginContainer/HSplitContainer/GridContainer/Guitar")
onready var DJPad_button = get_node("../MarginContainer/Panel/MarginContainer/HSplitContainer/GridContainer/DJPad")

var type_buttons = []

var disabled_style : StyleBoxFlat
var enabled_style : StyleBoxFlat

func _ready():
	disabled_style = controller_button.get('custom_styles/hover')
	enabled_style = controller_button.get('custom_styles/hover').duplicate()
	enabled_style.set_bg_color(Color.gray)
	
	type_buttons.append(controller_button)
	type_buttons.append(guitar_button)
	type_buttons.append(DJPad_button)

func reset_colors():
	for i in type_buttons:
		i.set("custom_styles/focus", disabled_style)
		i.set("custom_styles/hover", disabled_style)
		i.set("custom_styles/normal", disabled_style)

func highlight_button(var id):
	reset_colors()
	
	type_buttons[id].set("custom_styles/focus", enabled_style)
	type_buttons[id].set("custom_styles/hover", enabled_style)
	type_buttons[id].set("custom_styles/normal", enabled_style)
