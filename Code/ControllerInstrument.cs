using Godot;
using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;

public class ControllerInstrument : Control
{
    [Export]
    int ID = 0;
    [Export]
    Color BGColor = Colors.LightCoral;

    public enum controllerType
    {
        Controller,
        Guitar,
        DJPad,
        Undifined
    }

    controllerType currentControllerType = controllerType.Undifined;

    int midiChannel;
    string port = "controller port";
    Label debugText;
    int rootNote = 65;
    int CCValue = 0;

    public override void _Ready()
    {
        // Panel BGPanel = GetNode("MarginContainer/Panel") as Panel;
        // BGPanel.GetStylebox("panel").BGColor = BGColor;
        GD.Print(Godot.Input.GetJoyName(0));
        GD.Print(Godot.Input.GetJoyGuid(0));

        GetNode<Label>("MarginContainer/Panel/MarginContainer/HSplitContainer/GridContainer/ControllerNumber").Text = "#" + (ID + 1);

        debugText = GetTree().Root.GetNode("Main/VBoxContainer/DebugText") as Label;
    }

    private void OnEventSent(object sender, MidiEventSentEventArgs e)
    {
        var midiDevice = (MidiDevice)sender;
        // GD.Print($"Event sent to '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
        debugText.Text = $"Event sent to '{midiDevice.Name}': {e.Event}";
    }

    public override void _Process(float delta)
    {
        // ListenForInput("C1A", rootNote);
        // ListenForInput("C1X", rootNote + 2);
        // ListenForInput("C1Y", rootNote + 3);
        // ListenForInput("C1B", rootNote + 5);
        // ListenForInput("C1BR", rootNote + 7);
        // ListenForInput("C1BL", rootNote + 8);

        // ListenForAnalog("C1LeftStickUp", 80);
        if (currentControllerType == controllerType.Controller)
        {
            ListenForInput("C1B1", rootNote);
            ListenForInput("C1B2", rootNote + 2);
            ListenForInput("C1B3", rootNote + 3);
            ListenForInput("C1B4", rootNote + 5);
            ListenForInput("C1B5", rootNote + 7);
            ListenForInput("C1B6", rootNote + 8);
        }
        GD.Print(currentControllerType);

        //GD.Print(Input.GetActionStrength("Whammy"));
    }

    private void ListenForInput(string buttonName, int note)
    {
        if (Input.IsActionJustPressed(buttonName))
        {
            playNote(note, 120);
        }
        if (Input.IsActionJustReleased(buttonName))
        {
            stopNote(note);
        }
    }

    private void ListenForAnalog(string actionName, int CC, int minValue = 0, int maxValue = 127)
    {
        float oldCCValue = CCValue;
        CCValue = (int)MapValue(Input.GetActionStrength(actionName), 0f, 1f, 0f, 127f);
        // CCValue = (int)MapValue(Input.GetActionStrength(actionName), 0f, 1f, 64f, 96f);
        if (CCValue != oldCCValue)
        {
            CCEvent(CC, CCValue);
        }

    }

    private void playNote(int midiNote, int velocity)
    {
        using (var outputDevice = OutputDevice.GetByName(port))
        {
            outputDevice.EventSent += OnEventSent;
            NoteOnEvent NOnEvent = new NoteOnEvent((SevenBitNumber)midiNote, (SevenBitNumber)velocity);
            NOnEvent.Channel = (FourBitNumber)midiChannel;
            outputDevice.SendEvent(NOnEvent);
        }
    }

    private void stopNote(int midiNote, int velocity = 0)
    {
        using (var outputDevice = OutputDevice.GetByName(port))
        {
            outputDevice.EventSent += OnEventSent;
            NoteOffEvent NOffEvent = new NoteOffEvent((SevenBitNumber)midiNote, (SevenBitNumber)velocity);
            NOffEvent.Channel = (FourBitNumber)midiChannel;
            outputDevice.SendEvent(NOffEvent);
        }
    }

    private void CCEvent(int CC, int value = 0)
    {
        using (var outputDevice = OutputDevice.GetByName(port))
        {
            outputDevice.EventSent += OnEventSent;

            outputDevice.SendEvent(new ControlChangeEvent((SevenBitNumber)CC, (SevenBitNumber)value));
        }
    }

    public void ChangeMidiChannel(int channel)
    {
        midiChannel = channel;
        GetNode<Label>("MarginContainer/Panel/MarginContainer/HSplitContainer/GridContainer/ChannelNumber").Text = "Ch: " + (midiChannel + 1);
    }

    public void ChangeControllerType(int type)
    {
        currentControllerType = (controllerType)type;
    }

    private float MapValue(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

        return m * x + c;
    }
}
