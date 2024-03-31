using Godot;

public partial class Arrow : Sprite2D
{
	public StaticBody2D heldOrb;
	public override void _Ready()
	{
		heldOrb = GetNode<StaticBody2D>("/root/Game/HeldOrb");
		base._Ready();
	}
	
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetViewport().GetMousePosition();
		if(mousePosition.X < 370)
		{
			mousePosition.X = 370f;
		} 
		else if(mousePosition.X > 880)
		{
			mousePosition.X = 880f;
		}
		GlobalPosition = new Vector2(mousePosition.X,80);
		MoveOrb();
		base._Process(delta);
	}

	public void MoveOrb()
	{
		var scaledHeight = Texture.GetHeight() * Scale.Y;
		var bottomOfArrow = GlobalPosition.Y + scaledHeight;
		heldOrb.Position = new Vector2(GlobalPosition.X, bottomOfArrow);
	}
}
