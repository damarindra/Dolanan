using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Sigil;

namespace CoreGame.Engine
{
	// TODO : Complete the animation system
	// 1. Add all track inside the AnimationSequence
	// 2. Find a way to implement the animation timing, Ofcourse using update dumbass
	// 3. Is Animation a Component??
	public class Animation : BaseObject
	{
		/// <summary>
		/// Total time animation in miliseconds
		/// </summary>
		public float AnimationLength { get; set; } = 5000;

		public bool Enable { get; set; } = true;

		private float _currentAnimationTime = 0;

		List<ValueTrack<int>> _intTracks = new List<ValueTrack<int>>();
		List<ValueTrack<float>> _floatTracks = new List<ValueTrack<float>>();
		List<ValueTrack<string>> _stringTracks = new List<ValueTrack<string>>();
		List<ValueTrack<bool>> _boolTracks = new List<ValueTrack<bool>>();
		List<MethodTrack> _methodTracks = new List<MethodTrack>();

		public Animation()
		{
		}

		public void UpdateAnimation(GameTime gameTime)
		{
			if (!Enable)
				return;

			foreach (var valueTrack in _intTracks)
			{
				TKey<int> key;
				if(valueTrack.TryGetNewKey(_currentAnimationTime, out key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _stringTracks)
			{
				TKey<string> key;
				if(valueTrack.TryGetNewKey(_currentAnimationTime, out key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _floatTracks)
			{
				TKey<float> key;
				if(valueTrack.TryGetNewKey(_currentAnimationTime, out key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _boolTracks)
			{
				TKey<bool> key;
				if(valueTrack.TryGetNewKey(_currentAnimationTime, out key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _methodTracks)
			{
				TKey<object> key;
				if (valueTrack.TryGetNewKey(_currentAnimationTime, out key))
					valueTrack.CallMethod(key.Value);
			}
			
			
			_currentAnimationTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds % AnimationLength;
		}
	}

	#region KEY
	public class Key
	{
		public float Time;
		public Key(float time)
		{
			Time = time;
		}
	}

	public class TKey<T> : Key
	{
		public T Value;

		public TKey(float time, T val) : base(time)
		{
			Value = val;
		}
	}

	[Obsolete("Actually, object is just base class, if we store upper type, then it return to the base. SO it is useless")]
	public class KeyValue : TKey<object>
	{
		public KeyValue(float time, object val) : base(time, val)
		{
		}
	}
	#endregion

	public enum TrackType
	{
		Value, Bezier, Method, Audio
	}

	public enum ReflectionInfoType
	{
		None, Field, Property
	}

	#region TRACK
	public class Track
	{
		public TrackType Type;
		/// <summary>
		/// Object currently being recorded
		/// </summary>
		protected object RecordObject;

		public Track(object recordObject)
		{
			RecordObject = recordObject;
		}
	}

	/// <summary>
	/// Value Track on Property (get set) value is a lot faster than Field (regular variable). Please consider it!
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Track<T> : Track
	{
		private List<TKey<T>> Keys = new List<TKey<T>>();
		public int KeysIndexPassed { get; private set; }

		public Track(object recordObject) : base(recordObject)
		{
		}

		/// <summary>
		/// Adding a Keyframe
		/// </summary>
		/// <param name="key"></param>
		/// <typeparam name="T"></typeparam>
		public virtual void AddKey(TKey<T> key)
		{
			Keys.Add(key);
		}

		
		/// <summary>
		/// Updating the track. If time has passing a key, return the key
		/// </summary>
		/// <param name="currentAnimationTime">total animation in miliseconds</param>
		/// <param name="key">key result</param>
		public virtual bool TryGetNewKey(float currentAnimationTime, out TKey<T> key)
		{
			key = null;
			TKey<T> nextKey = Keys[KeysIndexPassed + 1 % Keys.Count];
			if (currentAnimationTime > nextKey.Time)
			{
				KeysIndexPassed += 1 % Keys.Count;
				key = nextKey;
				return true;
			}

			return false;
		}
	}

	public class ValueTrack<T> : Track<T>
	{
		protected string VarName;
		protected readonly ReflectionInfoType InfoType = ReflectionInfoType.None;
		protected readonly FieldInfo FieldInfo;
		private readonly PropertyInfo _propertyInfo;
		protected Action<object, T> Setter;
		
		public ValueTrack(object recordObject, string varName) : base(recordObject)
		{
			VarName = varName;
			Type = TrackType.Value;

			Type t = recordObject.GetType();
			while (t != null && InfoType == ReflectionInfoType.None)
			{
				_propertyInfo = t.GetProperty(varName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (_propertyInfo != null)
				{
					InfoType = ReflectionInfoType.Property;
					Emit<Action<object, T>> setterEmit = Emit<Action<object, T>>
						.NewDynamicMethod()
						.LoadArgument(0)
						.CastClass(RecordObject.GetType())
						.LoadArgument(1)
						.Call(_propertyInfo.GetSetMethod(nonPublic: true))
						.Return();
					Setter = setterEmit.CreateDelegate();
					
					break;
				}
				
				FieldInfo = t.GetField(varName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (FieldInfo != null)
				{
					InfoType = ReflectionInfoType.Field;
					break;
				}

				t = t.BaseType;
			}
			
			if(FieldInfo == null && _propertyInfo == null)
				Log.PrintError("Variable name : " + varName + "not found at " + recordObject.GetType());
		}
		public void SetValueToTrackedObject(T param)
		{
			Setter(RecordObject, param);
		}
	}
	
	public class MethodTrack : Track<object>
	{
		private MethodInfo _methodInfo;
		
		public MethodTrack(object recordObject, string methodName) : base(recordObject)
		{
			Type t = recordObject.GetType();
			while (t != null)
			{
				_methodInfo = t.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (_methodInfo != null)
				{
					break;
				}

				t = t.BaseType;
			}
			
			if(_methodInfo == null)
				Log.PrintError("Method name : " + methodName + "not found at " + recordObject.GetType());
			
		}

		/// <summary>
		/// Call method
		/// </summary>
		/// <param name="p">parameter from the original method</param>
		/// <returns></returns>
		public object CallMethod(object p)
		{
			if(_methodInfo != null)
				return _methodInfo.Invoke(RecordObject, new [] {p});
			return null;
		}
	}
	#endregion
		
	// TODO : BezierTrack, copy with godot implementation
	// public class KeyBezier : TKey<float>
	// {
	// 	public Vector2 In_Handle = Vector2.Zero;
	// 	public Vector2 Out_Handle = Vector2.Zero;
	// }
	// public class BezierTrack : Track
	// {
	// 	public BezierTrack(BaseObject trackObject, string varName) : base(trackObject)
	// 	{
	// 	}
	// }

}