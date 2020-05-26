using Microsoft.Xna.Framework;

namespace CoreGame.Engine
{
	public class Transform2D
	{
		public Matrix Matrix;
		
		public Transform2D Parent { get; set; }
		
		// Local
		public Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				UpdateTransform();
			}
		}
		private Vector2 _position = Vector2.Zero;

		public Vector2 GlobalPosition
		{
			get => ParentGlobalPosition + Position;
			set
			{
				Position = value - ParentGlobalPosition;
				UpdateTransform();
			}
		}

		private Vector2 ParentGlobalPosition
		{
			get => Parent?.GlobalPosition ?? Vector2.Zero;
		}

		public float Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				UpdateTransform();
			}
		}
		private float _rotation = 0f;
		public float GlobalRotation
		{
			get => ParentGlobalRotation + Rotation;
			set
			{
				Rotation = value - ParentGlobalRotation;
				UpdateTransform();
			}
		}

		private float ParentGlobalRotation
		{
			get => Parent?.GlobalRotation ?? 0;
		}

		public Vector2 Scale
		{
			get => _scale;
			set
			{
				_scale = value;
				UpdateTransform();
			}
		}

		private Vector2 _scale = Vector2.One;
		private Vector2 GlobalScale
		{
			get => Parent?.GlobalScale * Scale ?? Scale;
		}

		public void UpdateTransform()
		{
			Matrix = Matrix.CreateScale(new Vector3(GlobalScale, 0)) *
			          Matrix.CreateRotationZ(GlobalRotation) *
			          Matrix.CreateTranslation(new Vector3(GlobalPosition, 0));
		}

		public Vector2 Right
		{
			get => new Vector2(Matrix.Right.X, Matrix.Right.Y);
		}
		public Vector2 Left
		{
			get => new Vector2(Matrix.Left.X, Matrix.Left.Y);
		}
		public Vector2 Up
		{
			get => new Vector2(Matrix.Up.X, Matrix.Up.Y);
		}
		public Vector2 Down
		{
			get => new Vector2(Matrix.Down.X, Matrix.Down.Y);
		}
	}
}