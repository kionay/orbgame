using Godot;
using System;
using System.Collections.Generic;

public partial class game : Node2D
{
	[Signal]
	public delegate void MergeSignalEventHandler(Node2D a, Node2D b);

	RigidBody2D templateOrb;
	Timer dropTimer;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		MergeSignal += HandleMerge;
		dropTimer = GetNode<Timer>("DropTimer");
		base._Ready();
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("mouseclick"))
		{
			if(dropTimer.TimeLeft == 0)
			{
				var spawnedOrb = MakeOrb("red");
				Sprite2D arrow = GetNode<Sprite2D>("Arrow");
				spawnedOrb.Position = new Vector2(arrow.Position.X, arrow.Position.Y + arrow.Texture.GetHeight());
				AddChild(spawnedOrb);
				spawnedOrb.ApplyImpulse(Vector2.Down * 1000, this.Position);
				dropTimer.Start(0.2);
			}
		}
		base._Process(delta);
	}

	private RigidBody2D MakeOrb(string colorTag)
	{
		var templateOrb = GetNode<RigidBody2D>(colorTag);
		RigidBody2D spawnedOrb = templateOrb.Duplicate(6) as RigidBody2D;
		spawnedOrb.Visible = true;
		spawnedOrb.GravityScale = 1;
		spawnedOrb.SetCollisionLayerValue(1, false);
		spawnedOrb.SetCollisionLayerValue(2,  true);
		spawnedOrb.SetCollisionMaskValue(1, false);
		spawnedOrb.SetCollisionMaskValue(2, true);
		spawnedOrb.SetMeta("OrbName", "colorTag");
		return spawnedOrb;
	}

	private List<string> orbColors = new() 
	{
		"red",
		"pink",
		"blue",
		"orange",
		"yellow",
		"light_green",
		"green"
	};

	private void HandleMerge(Node2D a, Node2D b)
	{	
		if(!a.IsQueuedForDeletion() && !b.IsQueuedForDeletion())
		{
			var combiningColor = a.GetMeta("OrbName").AsString();
			if(combiningColor != orbColors[^1])
			{
				var newColor = orbColors[orbColors.IndexOf(combiningColor) + 1];
				var pointInBetween = a.Transform.InterpolateWith(b.Transform, 0.5f);
				var newOrb = MakeOrb(newColor);
				newOrb.Transform = pointInBetween;
				AddChild(newOrb);
				RemoveChild(a);
				RemoveChild(b);
				a.QueueFree();
				b.QueueFree();
			}

		}
	}
}
