using Godot;
using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;

public class Testing : Control
{
	// string port = "controller port";
	// Label debugText;
	// int rootNote = 65;
	// int CCValue = 0;

	// public override void _Ready()
	// {
	//     GD.Print(Godot.Input.GetJoyName(0));
	//     GD.Print(Godot.Input.GetJoyGuid(0));

	//     debugText = GetNode("VBoxContainer/DebugText") as Label;
	// }

	// private void OnEventSent(object sender, MidiEventSentEventArgs e)
	// {
	//     var midiDevice = (MidiDevice)sender;
	//     GD.Print($"Event sent to '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
	//     debugText.Text = $"Event sent to '{midiDevice.Name}': {e.Event}";
	// }

	// public override void _Process(float delta)
	// {
	//     // ListenForInput("C1A", rootNote);
	//     // ListenForInput("C1X", rootNote + 2);
	//     // ListenForInput("C1Y", rootNote + 3);
	//     // ListenForInput("C1B", rootNote + 5);
	//     // ListenForInput("C1BR", rootNote + 7);
	//     // ListenForInput("C1BL", rootNote + 8);

	//     // ListenForAnalog("C1LeftStickUp", 80);

	//     ListenForInput("C1A", rootNote);
	//     ListenForInput("C1B", rootNote + 2);
	//     ListenForInput("C1Y", rootNote + 3);
	//     ListenForInput("C1X", rootNote + 5);
	//     ListenForInput("C1BL", rootNote + 7);
	//     ListenForInput("C1BL", rootNote + 8);

	//     ListenForAnalog("Tilt", 80);

	//     //GD.Print(Input.GetActionStrength("Whammy"));
	// }

	// public void ListenForInput(string buttonName, int note)
	// {
	//     if (Input.IsActionJustPressed(buttonName))
	//     {
	//         playNote(note, 120);
	//     }
	//     if (Input.IsActionJustReleased(buttonName))
	//     {
	//         stopNote(note);
	//     }
	// }

	// public void ListenForAnalog(string actionName, int CC, int minValue = 0, int maxValue = 127)
	// {
	//     float oldCCValue = CCValue;
	//     CCValue = (int)MapValue(Input.GetActionStrength(actionName), 0f, 1f, 0f, 127f);
	//     // CCValue = (int)MapValue(Input.GetActionStrength(actionName), 0f, 1f, 64f, 96f);
	//     if (CCValue != oldCCValue)
	//     {
	//         CCEvent(CC, CCValue);
	//     }

	// }

	// public void playNote(int midiNote, int velocity)
	// {
	//     using (var outputDevice = OutputDevice.GetByName(port))
	//     {
	//         outputDevice.EventSent += OnEventSent;
	//         NoteOnEvent NOnEvent = new NoteOnEvent((SevenBitNumber)midiNote, (SevenBitNumber)velocity);
	//         NOnEvent.Channel = (FourBitNumber)1;
	//         outputDevice.SendEvent(NOnEvent);
	//     }
	// }

	// public void stopNote(int midiNote, int velocity = 0)
	// {
	//     using (var outputDevice = OutputDevice.GetByName(port))
	//     {
	//         outputDevice.EventSent += OnEventSent;
	//         NoteOffEvent NOffEvent = new NoteOffEvent((SevenBitNumber)midiNote, (SevenBitNumber)velocity);
	//         NOffEvent.Channel = (FourBitNumber)1;
	//         outputDevice.SendEvent(NOffEvent);
	//     }
	// }

	// public void CCEvent(int CC, int value = 0)
	// {
	//     using (var outputDevice = OutputDevice.GetByName(port))
	//     {
	//         outputDevice.EventSent += OnEventSent;

	//         outputDevice.SendEvent(new ControlChangeEvent((SevenBitNumber)CC, (SevenBitNumber)value));
	//     }
	// }

	// public float MapValue(float x, float x1, float x2, float y1, float y2)
	// {
	//     var m = (y2 - y1) / (x2 - x1);
	//     var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

	//     return m * x + c;
	// }
}
