using Godot;
using Orbgame.Globals;
using System;

public partial class Orb : RigidBody2D
{
	[Export]
	private NodeType NodeType;

	public override void _Ready()
	{
		this.NodeType = GetMeta("OrbName")
			.AsString()
			.ToNodeType();
		BodyEntered += ForwardCollision;
		base._Ready();
	}

	private void ForwardCollision(Node body)
	{
		CallDeferred("CollidedEventHandler", body as Node2D);
	}

	public void CollidedEventHandler(Node2D body)
	{
		if((body as Orb).NodeType == this.NodeType)
		{
			// if(this.IsConnected("body_entered", ForwardCollision))
			// {
			// 	this.Disconnect("body_entered", ForwardCollision);
			// 	BodyEntered -= ForwardCollision;
			// }

			GetParent()?.EmitSignal(game.SignalName.MergeSignal, this, body);
		}
	}
}



