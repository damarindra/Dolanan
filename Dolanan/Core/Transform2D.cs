using Dolanan.Components;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public delegate void TransformParentChange(Transform2D parent);

	/// <summary>
	///     A component that control a Transformation.
	/// </summary>
	public class Transform2D : Component
	{
		private Vector2 _localScale = Vector2.One;

		private Matrix _matrix = Matrix.Identity;
		private Transform2D _parent;
		private Vector2 _position = Vector2.Zero;
		private float _rotation;
		public TransformParentChange OnTransformParentChange;

		public Transform2D(Actor owner) : base(owner)
		{
		}

		/// <summary>
		///     Never set parent! Set parent from Actor instead!
		/// </summary>
		public Transform2D Parent
		{
			get => _parent;
			set
			{
				_parent = value;
				OnTransformParentChange?.Invoke(value);
			}
		}

		// Local
		public Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				UpdateTransform();
				// Log.Print(_matrix.M41.ToString());
				// Log.Print(_matrix.M42.ToString());
				// Log.Print(_matrix.M43.ToString());
				// Log.Print(_matrix.M44.ToString());
			}
		}

		public Vector2 GlobalPosition
		{
			get => _matrix.Translation.ToVector2();
			set => Position = value - ParentGlobalPosition;
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

		public float GlobalRotation
		{
			get => ParentGlobalRotation + Rotation;
			set => Rotation = value - ParentGlobalRotation;
		}


		public Vector2 LocalScale
		{
			get => _localScale;
			set
			{
				_localScale = value;
				UpdateTransform();
			}
		}

		/// <summary>
		///     Getting Parent Global Position, Careful, if parent null, return Vector2.Zero.
		/// </summary>
		private Vector2 ParentGlobalPosition => Parent?.GlobalPosition ?? Vector2.Zero;

		private float ParentGlobalRotation => Parent?.GlobalRotation ?? 0;
		public Vector2 GlobalScale => ParentScale * LocalScale;

		private Vector2 ParentScale => Parent?.GlobalScale ?? Vector2.One;

		public Matrix Matrix => _matrix;

		public Vector2 Right => _matrix.Right.ToVector2();

		public Vector2 Left => -Right;

		// This actually weird, maybe because matrix using 3D space, so Down is up
		public Vector2 Down => _matrix.Up.ToVector2();
		public Vector2 Up => -Down;

		public override void Start()
		{
		}

		public void UpdateTransform()
		{
			var matrix = Matrix.CreateScale(new Vector3(LocalScale, 1)) *
			             Matrix.CreateRotationZ(Rotation) *
			             Matrix.CreateTranslation(new Vector3(Position, 0));
			if (Parent != null)
				matrix *= GetParentMatrix();

			_matrix = matrix;

			foreach (var actor in Owner.GetChilds)
				actor.Transform.UpdateTransform();
			//Log.PrintWarning(GlobalPosition.ToString());
		}

		public Matrix GetParentMatrix()
		{
			return Matrix.CreateScale(new Vector3(ParentScale, 1)) *
			       Matrix.CreateRotationZ(ParentGlobalRotation - Rotation) *
			       Matrix.CreateTranslation(new Vector3(ParentGlobalPosition, 0));
		}
	}
}