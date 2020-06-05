using System;
using Dolanan.Components;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public delegate void TransformParentChange(Transform2D parent);
	
	/// <summary>
	/// A component that control a Transformation.
	/// </summary>
	public class Transform2D : ICloneable
	{
		public static Transform2D Identity => new Transform2D();
		
		public TransformParentChange OnTransformParentChange;
		
		public Matrix Matrix { get; private set; }
		
		private Transform2D _parent;
		private Vector2 _position = Vector2.Zero;
		private float _rotation = 0f;
		private Vector2 _scale = Vector2.One;

		public Transform2D()
		{
			Matrix = Matrix.Identity;
		}

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
			}
		}

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

		private Vector2 GlobalScale
		{
			get => Parent?.GlobalScale * Scale ?? Scale;
		}

		private void UpdateTransform()
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

		public object Clone()
		{
			Transform2D clone = Transform2D.Identity;
			clone.Parent = Parent;
			clone.Position = Position;
			clone.Rotation = Rotation;
			clone.Scale = Scale;
			clone.UpdateTransform();

			return clone;
		}
	}
}