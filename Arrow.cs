using Godot;

public partial class Arrow : Sprite2D
{
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
		base._Process(delta);
	}
}
