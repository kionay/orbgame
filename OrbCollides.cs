using Godot;
using System;

public partial class OrbCollides : RigidBody2D
{
	public override void _Ready()
	{
		BodyEntered += ForwardCollision;
		base._Ready();
	}

	private void ForwardCollision(Node body)
	{
		CallDeferred("CollidedEventHandler", body as Node2D);
	}

	public void CollidedEventHandler(Node2D body)
	{
		if(body.GetMetaList().Contains("OrbName"))
		{
			if(this.GetMeta("OrbName").AsString() == body.GetMeta("OrbName").AsString())
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
}



