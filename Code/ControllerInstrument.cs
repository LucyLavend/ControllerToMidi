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
    int offset = 0;
    [Export]
    int rootNote = 65;

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
    int CCValue = 0;

    public override void _Ready()
    {
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
        string controllerNumber = "C" + (ID + 1);

        //########################## Controller ########################

        if (currentControllerType == controllerType.Controller)
        {
            ListenForInput(controllerNumber + "B0", rootNote + offset);
            ListenForInput(controllerNumber + "B1", rootNote + offset + 2);
            ListenForInput(controllerNumber + "B2", rootNote + offset + 3);
            ListenForInput(controllerNumber + "B3", rootNote + offset + 5);
            ListenForInput(controllerNumber + "B4", rootNote + offset + 7);
            ListenForInput(controllerNumber + "B5", rootNote + offset + 8);

            //var horizontal = Input.GetActionRawStrength("right") - Input.GetActionRawStrength("left");
            //ListenForAnalog("C1LeftStickUp", 80);
        }

        //############################ Guitars ########################

        if (currentControllerType == controllerType.Guitar && midiChannel == 2)
        {
            ListenForInput(controllerNumber + "B0", rootNote + offset);
            ListenForInput(controllerNumber + "B1", rootNote + offset + 2);
            ListenForInput(controllerNumber + "B3", rootNote + offset + 3);
            ListenForInput(controllerNumber + "B2", rootNote + offset + 5);
            ListenForInput(controllerNumber + "B4", rootNote + offset + 7);
            ListenForInput(controllerNumber + "B12", rootNote + offset - 5);
            ListenForInput(controllerNumber + "B13", rootNote + offset + 10);
        }
        if (currentControllerType == controllerType.Guitar && midiChannel == 3)
        {
            offset = -12;
            ListenForInput(controllerNumber + "B0", rootNote + offset);
            ListenForInput(controllerNumber + "B1", rootNote + offset + 2);
            ListenForInput(controllerNumber + "B3", rootNote + offset + 3);
            ListenForInput(controllerNumber + "B2", rootNote + offset + 5);
            ListenForInput(controllerNumber + "B4", rootNote + offset + 7);
            ListenForInput(controllerNumber + "B12", rootNote + offset - 5);
            ListenForInput(controllerNumber + "B13", rootNote + offset + 10);
        }

        //######################## DJPad ########################

        if (currentControllerType == controllerType.DJPad && midiChannel == 4)
        {
            ListenForInput(controllerNumber + "B3", rootNote + offset);
            ListenForAnalog(Input.GetActionStrength(controllerNumber + "A3P") - Input.GetActionStrength(controllerNumber + "A3M"), 80, -1.0, 1.0, 45, 127);
            ListenForAnalog(Input.GetActionStrength(controllerNumber + "A2P") - Input.GetActionStrength(controllerNumber + "A2M"), 81, -1.0, 1.0, 0, 127);
        }


        if (currentControllerType == controllerType.DJPad && midiChannel == 5)
        {
            float sliderValue = Input.GetActionStrength(controllerNumber + "A3P") - Input.GetActionStrength(controllerNumber + "A3M";

            if (Input.IsActionJustPressed(controllerNumber + "B3"))
            {

                if (sliderValue >= 0.33)
                {
                    ListenForInput(controllerNumber + "B3", rootNote + offset);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 3);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 7);
                }
                else if (sliderValue >= -0.33)
                {
                    offset = 2;
                    ListenForInput(controllerNumber + "B3", rootNote + offset);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 3);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 7);
                }
                else if (sliderValue >= -1.0)
                {
                    offset = 7;

                    ListenForInput(controllerNumber + "B3", rootNote + offset);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 3);
                    ListenForInput(controllerNumber + "B3", rootNote + offset + 7);
                }
            }


            ListenForAnalog(Input.GetActionStrength(controllerNumber + "A2P") - Input.GetActionStrength(controllerNumber + "A2M"), 82, -1.0, 1.0, 0, 127);
        }

        offset = 0;


        if (Input.IsActionJustPressed("fullscreen"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
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

    private void ListenForAnalog(float actionStrength, int CC, double minActionStrength = 0.0, double maxActionStrength = 1.0, float minValue = 0, float maxValue = 127)
    {
        float oldCCValue = CCValue;
        CCValue = (int)MapValue(actionStrength, (float)minActionStrength, (float)maxActionStrength, minValue, maxValue);
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
