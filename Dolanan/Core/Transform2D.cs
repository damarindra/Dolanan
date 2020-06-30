﻿using System.Collections.Generic;
using Dolanan.Components;
 using Dolanan.Editor.Attribute;
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
		[VisibleProperty] private Vector2 _localScale = Vector2.One;
		[VisibleProperty] private Vector2 _location = Vector2.Zero;

		private Matrix _matrix = Matrix.Identity;
		private Transform2D _parent;
		[VisibleProperty] private float _rotation;
		internal List<Transform2D> Childs = new List<Transform2D>();
		public TransformParentChange OnParentChange;

		public Transform2D(Actor owner) : base(owner)
		{
		}

		/// <summary>
		///     Never set parent! Set parent from Actor instead! C# doens't have friend feature, so this is suck!
		/// </summary>
		public Transform2D Parent
		{
			get => _parent;
			internal set
			{
				if (_parent != null)
				{
					_parent.Childs.Remove(this);
					_parent = null;
				}

				_parent = value;
				if (_parent != null)
				{
					_parent.Childs.Add(Transform);
					if (_parent.Owner.Layer != Owner.Layer)
						Owner.SetLayer(_parent.Owner.Layer);
				}

				OnParentChange?.Invoke(value);
			}
		}

		/// <summary>
		///     Local Location
		/// </summary>
		public virtual Vector2 Location
		{
			get => _location;
			set
			{
				_location = value;
				UpdateTransform();
			}
		}

		/// <summary>
		///     Global Location
		/// </summary>
		public virtual Vector2 GlobalLocation
		{
			get => _matrix.Translation.ToVector2();
			set => Location = value - ParentGlobalLocation;
		}


		public virtual float Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				UpdateTransform();
			}
		}

		public virtual float GlobalRotation
		{
			get => ParentGlobalRotation + Rotation;
			set => Rotation = value - ParentGlobalRotation;
		}


		public virtual Vector2 LocalScale
		{
			get => _localScale;
			set
			{
				_localScale = value;
				UpdateTransform();
			}
		}

		public virtual Vector2 GlobalScale => ParentScale * LocalScale;

		/// <summary>
		///     Getting Parent Global Position, Careful, if parent null, return Vector2.Zero.
		/// </summary>
		protected Vector2 ParentGlobalLocation => Parent?.GlobalLocation ?? Vector2.Zero;

		protected float ParentGlobalRotation => Parent?.GlobalRotation ?? 0;

		protected Vector2 ParentScale => Parent?.GlobalScale ?? Vector2.One;

		public Matrix Matrix => _matrix;

		public Vector2 Right => _matrix.Right.ToVector2();

		public Vector2 Left => -Right;

		// This actually weird, maybe because matrix using 3D space, so Down is up
		public Vector2 Down => _matrix.Up.ToVector2();
		public Vector2 Up => -Down;

		public Transform2D[] GetChilds => Childs.ToArray();

		public override void Start()
		{
		}

		public void UpdateTransform()
		{
			var matrix = Matrix.CreateScale(new Vector3(LocalScale, 1)) *
			             Matrix.CreateRotationZ(Rotation) *
			             Matrix.CreateTranslation(new Vector3(Location, 0));
			if (Parent != null)
				matrix = matrix * GetParentMatrix();

			_matrix = matrix;

			foreach (var actor in Childs)
				actor.UpdateTransform();
			//Log.PrintWarning(GlobalPosition.ToString());
		}

		public Matrix GetParentMatrix()
		{
			return Matrix.CreateScale(new Vector3(ParentScale, 1)) *
			       Matrix.CreateRotationZ(ParentGlobalRotation) *
			       Matrix.CreateTranslation(new Vector3(ParentGlobalLocation, 0));
		}
	}
}