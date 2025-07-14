using Godot;
using System;

public partial class Level1 : Node2D
{

    DirectionalLight2D sunLight;
    PointLight2D flashlight;
    Label labelDay;
    AnimationPlayer animPlayer;

	public enum DayState
    {
        MORNING,
        DAY,
        EVENING,
        NIGHT
    }

    public DayState dayState = DayState.MORNING;

    public int dayCount = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        sunLight = GetNode<DirectionalLight2D>("SunLight");
        flashlight = GetNode<PointLight2D>("PointLight2D");
        labelDay = GetNode<Label>("/root/Level/CanvasLayer/LabelDayText");
        animPlayer = GetNode<AnimationPlayer>("/root/Level/CanvasLayer/AnimationPlayer");

        dayCount = 1;
        SetDayText();
        DayTextFade();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    private void MorningStart() 
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(sunLight, "energy", 0.4, 20.0f);
        Tween tween2 = GetTree().CreateTween();
        tween2.TweenProperty(flashlight, "energy", 0.0, 10.0f);
    }

    private void DayStart() 
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(sunLight, "energy", 0.2, 20.0f);
    }

    private void EnevingStart()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(sunLight, "energy", 0.6, 20.0f);
        Tween tween2 = GetTree().CreateTween();
        tween2.TweenProperty(flashlight, "energy", 1.5, 10.0f);
    }

    private void NightStart() 
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(sunLight, "energy", 0.95, 20.0f);
    }

    public void _on_day_night_timeout() 
    {
        dayState += 1;
        dayState = (DayState)((int)dayState % 4);


        switch (dayState)
        {
            case DayState.MORNING:
                dayCount++;
                SetDayText();
                MorningStart();
                DayTextFade();
                break;
            case DayState.DAY:
                DayStart();
                break;
            case DayState.EVENING:
                EnevingStart();
                break;
            case DayState.NIGHT:
                NightStart();
                break;
        }
    }

    private async void DayTextFade() 
    {
        animPlayer.Play("dayTextFadeIn");
        await ToSignal(GetTree().CreateTimer(2), "timeout");
        animPlayer.Play("dayTextFadeOut");
    }

    private void SetDayText() 
    {
        labelDay.Text = "Day " + dayCount;
    }
}
