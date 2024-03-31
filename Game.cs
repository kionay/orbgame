using Godot;
using Orbgame.Globals;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Game : Node2D
{
	[Signal]
	public delegate void MergeSignalEventHandler(Orb a, Orb b);

	Orb templateOrb;
	Timer dropTimer;
	bool isGameOver = false;
	Random newBallRNG = new();
	Globals globals;

	Dictionary<NodeType, float> spawnableOrbChances = new()
	{
		{ NodeType.red, 0.70f },
		{ NodeType.pink, 0.20f },
		{ NodeType.blue, 0.03f },
	};

	public override void _Ready()
	{
		globals = GetNode<Globals>("/root/Globals");
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		MergeSignal += HandleMerge;
		dropTimer = GetNode<Timer>("DropTimer");
		base._Ready();
	}

	public NodeType GetRandomOrbType()
	{
		var weightSum = spawnableOrbChances.Sum((orbChance) => orbChance.Value);
		var rng = newBallRNG.NextDouble() * weightSum;
		var selection = spawnableOrbChances.FirstOrDefault(orbChance => rng >= (weightSum -= orbChance.Value));
		return selection.Key;
	}
	
	public override void _Process(double delta)
	{
		Arrow arrow = GetNode<Arrow>("Arrow");
		if(dropTimer.TimeLeft == 0)
		{
			if(arrow.heldOrb == null)
			{
				var spawnedOrb = MakeOrb(GetRandomOrbType());
				spawnedOrb.GravityScale = 0;
				arrow.heldOrb = spawnedOrb;
				//spawnedOrb.Position = new Vector2(arrow.Position.X, arrow.Position.Y + arrow.Texture.GetHeight());
				arrow.MoveOrb();
				AddChild(spawnedOrb);
			}
		}
		if (Input.IsActionJustPressed("mouseclick") && !isGameOver)
		{
			if(arrow.heldOrb != null)
			{
				DropOrb(arrow.heldOrb);
				arrow.heldOrb = null;
				dropTimer.Start(0.2);
			}
		}
		if(Input.IsActionJustPressed("newgame") && isGameOver)
		{
			NewGame();
		}
		base._Process(delta);
	}

	private void NewGame()
	{
		// delete all orbs
		var orbs = GetChildren().Where((child) => child is Orb && child.Name != "orb");
		foreach(Orb orb in orbs.Cast<Orb>())
		{
			RemoveChild(orb);
			orb.QueueFree();
		}
		// reset score
		globals.Score = 0;
		GetNode<RichTextLabel>("/root/Game/GameOverGroup/GameOverText").Hide();
		// reset gameover flag
		isGameOver = false;
	}

	private void DropOrb(Orb orb)
	{
		orb.GravityScale = 1;
		orb.ApplyImpulse(Vector2.Down * 1000, this.Position);
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
		var nodeScaleFactor = globals.NodeConfiguration[nodeType].Scale;
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

	private bool IsCollisionGameOver(Orb a, Orb b)
	{
		// the highest orb is the most likely to cause game over
		var orbHeight = a.GetChild<Sprite2D>(1).Texture.GetHeight();
		// Y increases downward, so we want the smallest Y value
		// coordinates start from the center of the orb, 
		// 	so we need to subtract half of the height of the orb to find the top of it
		var topOfOrbA = a.Position.Y - (orbHeight / 2);
		var topOfOrbB = b.Position.Y - (orbHeight / 2);
		var checkHeight = Math.Min(topOfOrbA, topOfOrbB);
		var gameOverLine = GetNode<Line2D>("/root/Game/GameOverMarker");
		var gameOverY = gameOverLine.Points.First().Y;
		if(gameOverY >= checkHeight)
		{
			return true;
		} else {
			return false;
		}
	}

	private void PlayOrbMergeSound(NodeType mergeNodeType)
	{
		var player = GetNode<AudioStreamPlayer>(globals.NodeConfiguration[mergeNodeType].MergeSoundResourcePath);
		player.Play();
	}

	private void HandleMerge(Orb a, Orb b)
	{	
		if(!a.IsQueuedForDeletion() && !b.IsQueuedForDeletion())
		{
			if(IsCollisionGameOver(a, b))
			{
				var orbs = GetChildren().Where((child) => child is Orb && child.Name != "orb");
				foreach(Orb orb in orbs.Cast<Orb>())
				{
					orb.Freeze = true;
				}
				GetNode<RichTextLabel>("/root/Game/GameOverGroup/GameOverText").Show();
				isGameOver = true;
			} 
			else if(!a.NodeType.IsBiggestNodeType())
			{
				var newNodeType = a.NodeType + 1;
				var pointInBetween = a.Transform.InterpolateWith(b.Transform, 0.5f);
				var newOrb = MakeOrb(newNodeType);
				newOrb.Transform = pointInBetween;
				globals.Score += globals.NodeConfiguration[a.NodeType].MergeScore;
				PlayOrbMergeSound(newNodeType);
				AddChild(newOrb);
				RemoveChild(a);
				RemoveChild(b);
				a.QueueFree();
				b.QueueFree();
			}	
		}
	}
}
