using Godot;
using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;

public class Testing : Control
{
    public override void _Process(float delta)
    {
        base._Process(delta);
		if (Input.IsActionJustPressed("fullscreen")){
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}
