using Godot;

public partial class Arrow : Sprite2D
{
	public Orb heldOrb = null;
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
		if(heldOrb != null)
		{
			MoveOrb();
		}
		base._Process(delta);
	}

	public void MoveOrb()
	{
		var scaledHeight = Texture.GetHeight() * Scale.Y;
		var scaledWidth = Texture.GetWidth() * Scale.X;
		var bottomOfArrow = GlobalPosition.Y + scaledHeight;
		var middleOfArrowWidth = GlobalPosition.X + (scaledWidth / 2);
		var orbSprite = heldOrb.GetChild<Sprite2D>(1);
		var orbWidth = orbSprite.Texture.GetWidth() *  orbSprite.Scale.X;
		var leftOfOrbDestination = middleOfArrowWidth - (orbWidth / 2);
		heldOrb.Position = new Vector2(leftOfOrbDestination, bottomOfArrow);
	}
}
