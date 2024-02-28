using Godot;
using Orbgame.Globals;
using System;
using System.Collections.Generic;

public partial class game : Node2D
{
	[Signal]
	public delegate void MergeSignalEventHandler(Orb a, Orb b);

	Orb templateOrb;
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
				var spawnedOrb = MakeOrb(NodeType.red);
				Sprite2D arrow = GetNode<Sprite2D>("Arrow");
				spawnedOrb.Position = new Vector2(arrow.Position.X, arrow.Position.Y + arrow.Texture.GetHeight());
				AddChild(spawnedOrb);
				spawnedOrb.ApplyImpulse(Vector2.Down * 1000, this.Position);
				dropTimer.Start(0.2);
			}
		}
		base._Process(delta);
	}

	private Orb MakeOrb(NodeType nodeType)
	{
		var templateOrb = GetNode<RigidBody2D>("orb");
		Orb spawnedOrb = templateOrb.Duplicate(6) as Orb;
		spawnedOrb.NodeType = nodeType;
		spawnedOrb.Visible = true;
		spawnedOrb.GravityScale = 1;
		spawnedOrb.SetCollisionLayerValue(1, false);
		spawnedOrb.SetCollisionLayerValue(2,  true);
		spawnedOrb.SetCollisionMaskValue(1, false);
		spawnedOrb.SetCollisionMaskValue(2, true);
		var nodeScaleFactor = Globals.NodeScales[nodeType];
		var nodeScaleVector = new Vector2(nodeScaleFactor, nodeScaleFactor);
		spawnedOrb.GetChild<CollisionShape2D>(0).Scale = nodeScaleVector;
		var nodeSprite = spawnedOrb.GetChild<Sprite2D>(1);
		nodeSprite.Texture = nodeType.GetTexture();
		nodeSprite.Scale = nodeScaleVector;
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

	private void HandleMerge(Orb a, Orb b)
	{	
		if(!a.IsQueuedForDeletion() && !b.IsQueuedForDeletion())
		{
			if(!a.NodeType.IsBiggestNodeType())
			{
				var newNodeType = a.NodeType + 1;
				var pointInBetween = a.Transform.InterpolateWith(b.Transform, 0.5f);
				var newOrb = MakeOrb(newNodeType);
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
