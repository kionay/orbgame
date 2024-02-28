using Godot;
using Orbgame.Globals;
using System;

public partial class Orb : RigidBody2D
{
	[Export]
	public NodeType NodeType;

	public override void _Ready()
	{
		BodyEntered += ForwardCollision;
		base._Ready();
	}

	private void ForwardCollision(Node body)
	{
		CallDeferred("CollidedEventHandler", body as Orb);
	}

	public void CollidedEventHandler(Orb body)
	{
		if(body.NodeType == this.NodeType)
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



