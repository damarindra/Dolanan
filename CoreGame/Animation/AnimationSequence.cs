using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CoreGame.Engine;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Sigil;

namespace CoreGame.Animation
{
	//==================================================================================
	// ============================= END ANIMATION =====================================
	//==================================================================================

	#region ANIMATION
	
	// TODO : Complete the animation system
	// TODO : Changing Animation value need a delegate to refresh the other value. For now, it can only be set on constructor
	public class AnimationSequence : BaseObject
	{
		/// <summary>
		/// Total time animation in miliseconds
		/// </summary>
		public float AnimationLength { get; private set; }

		public bool IsPlaying { get; private set; } = true;
		
		public bool IsReverse
		{
			get => _animationData.IsReverse;
		}

		public bool IsLoop
		{
			get => _animationData.IsLoop;
		}

		AnimationData _animationData;


		List<ValueTrack<int>> _intTracks = new List<ValueTrack<int>>();
		List<ValueTrack<float>> _floatTracks = new List<ValueTrack<float>>();
		List<ValueTrack<bool>> _boolTracks = new List<ValueTrack<bool>>();
		List<ValueTrack<string>> _stringTracks = new List<ValueTrack<string>>();
		List<MethodTrack> _methodTracks = new List<MethodTrack>();

		/// <summary>
		/// Create new Animation
		/// </summary>
		/// <param name="animationLength">animation time in miliseconds</param>
		public AnimationSequence(float animationLength, bool isReverse = false, bool isLoop = true)
		{
			AnimationLength = animationLength;
			_animationData = new AnimationData(this);
			_animationData.IsLoop = isLoop;
			_animationData.IsReverse = isReverse;
		}

		public ValueTrack<T> CreateNewValueTrack<T>(string trackName, object trackedObj, string trackedField)
		{
			ValueTrack<T> newTrack = new ValueTrack<T>(this, trackedObj, trackedField);
			if (typeof(T) == typeof(int))
				_intTracks.Add(newTrack as ValueTrack<int>);
			else if (typeof(T) == typeof(float))
				_floatTracks.Add(newTrack as ValueTrack<float>);
			else if (typeof(T) == typeof(bool))
				_boolTracks.Add(newTrack as ValueTrack<bool>);
			else if (typeof(T) == typeof(string))
				_stringTracks.Add(newTrack as ValueTrack<string>);

			return newTrack;
		}

		public void UpdateAnimation(GameTime gameTime)
		{
			if (!IsPlaying)
				return;

			float positionBeforeUpdate = _animationData.Position;
			_animationData.UpdateAnimationData(gameTime);
			
			
			foreach (var valueTrack in _intTracks)
			{
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, _animationData.Position, _animationData, out var key))
				{
					valueTrack.SetValueToTrackedObject(key.Value);
					Log.Print(_animationData.Position.ToString());
				}
			}
			foreach (var valueTrack in _stringTracks)
			{
				if(valueTrack.TryGetNewKey(positionBeforeUpdate, _animationData.Position, _animationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _floatTracks)
			{
				if(valueTrack.TryGetNewKey(positionBeforeUpdate, _animationData.Position, _animationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _boolTracks)
			{
				if(valueTrack.TryGetNewKey(positionBeforeUpdate, _animationData.Position, _animationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			}
			foreach (var valueTrack in _methodTracks)
			{
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, _animationData.Position, _animationData, out var key))
					valueTrack.CallMethod(key.Value);
			}
		}

		public void Stop()
		{
			IsPlaying = false;
		}

		public void Restart()
		{
			_animationData.Position = 0;
			IsPlaying = true;
		}

		public void Resume()
		{
			IsPlaying = true;
		}

		// TODO : Seek doesn't work because we need to update all of the track _nextKeyIndex
		public void Seek(float timeInMilisec)
		{
			if (timeInMilisec < 0)
				timeInMilisec = 0;
			else if (timeInMilisec > AnimationLength)
				timeInMilisec = AnimationLength;

			_animationData.Position = timeInMilisec;
			Resume();
		}
	}

	public class AnimationData
	{
		public float Position { get; set; }
		public bool IsLoop { get; set; }
		public bool IsReverse { get; set; }
		
		/// <summary>
		/// Get this value after calling UpdateAnimationData (it is already called, don't call it again)
		/// So, this will give true value if in this frame UpdateAnimationData,
		/// the Position before update is less than AnimationLength and less than zero
		/// OR otherwise
		/// For IsLoop only
		/// </summary>
		public bool IsThisFrameLoopBack { get; private set; }

		public AnimationSequence AnimationSequence
		{
			get => _animationSequence;
		}

		private AnimationSequence _animationSequence;

		public AnimationData(AnimationSequence animationSequence)
		{
			_animationSequence = animationSequence;
		}

		public void UpdateAnimationData(GameTime gameTime)
		{
			IsThisFrameLoopBack = false;
			Position += gameTime.ElapsedGameTime.Milliseconds  * (IsReverse ? -1 : 1);

			if (!IsLoop)
			{
				if (Position < 0)
					Position = 0;
				else if (Position > _animationSequence.AnimationLength)
					Position = _animationSequence.AnimationLength;
			}
			else
			{
				if (Position < 0)
				{
					Position = _animationSequence.AnimationLength - Position;
					IsThisFrameLoopBack = true;
				}
				else if (Position > _animationSequence.AnimationLength)
				{
					Position = Position - _animationSequence.AnimationLength;
					IsThisFrameLoopBack = true;
				}
			}
			
		}
	}
	#endregion
	//==================================================================================
	// ============================= END ANIMATION =====================================
	//==================================================================================
 
	//==================================================================================
	// ================================= KEY =====================================
	//==================================================================================
	#region KEY
	// TODO: Key -> Changing TimeMilisec need auto sort List<Key> inside the Track. 
	// TODO: Make key IEquatable. It will add ability to check if a key already recorded
	public class Key
	{
		public float TimeMilisec { get; private set; }
		/// <summary>
		/// Create new key
		/// </summary>
		/// <param name="timeMilisec">Time in miliseconds</param>
		public Key(float timeMilisec)
		{
			TimeMilisec = timeMilisec;
		}
	}

	public class Key<T> : Key
	{
		public T Value;

		/// <summary>
		/// Create new key
		/// </summary>
		/// <param name="timeMilisec">Time in miliseconds</param>
		/// <param name="val"></param>
		public Key(float timeMilisec, T val) : base(timeMilisec)
		{
			Value = val;
		}
	}

	[Obsolete("Actually, object is just base class, if we store upper type, then it return to the base. SO it is useless")]
	public class KeyValue : Key<object>
	{
		public KeyValue(float timeMilisec, object val) : base(timeMilisec, val)
		{
		}
	}
	#endregion
	//==================================================================================
	// ============================= END KEY =====================================
	//==================================================================================

	//==================================================================================
	// ================================= TRACK =====================================
	//==================================================================================
	#region TRACK
	public enum TrackType
	{
		Value, Bezier, Method, Audio
	}

	public enum ReflectionInfoType
	{
		None, Field, Property
	}

	public class Track : BaseObject
	{
		public TrackType Type;
		/// <summary>
		/// Object currently being recorded
		/// </summary>
		protected object RecordObject;
		protected AnimationSequence Owner;

		public Track(AnimationSequence owner, object recordObject)
		{
			Owner = owner;
			RecordObject = recordObject;
		}
	}

	/// <summary>
	/// Value Track on Property (get set) value is a lot faster than Field (regular variable). Please consider it!
	/// Changing Track value in runtime will Resulting bugg, please don't do it
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Track<T> : Track
	{
		private List<Key<T>> Keys = new List<Key<T>>();
		private int _nextKeyIndex = -1;
		private int _lastValidKeyIndex = -1;

		public void PrintALl()
		{
			foreach (var key in Keys)
			{
				Log.Print(key.TimeMilisec.ToString());
			}
		}
		
		public Track(AnimationSequence owner,object recordObject) : base(owner, recordObject)
		{
		}

		/// <summary>
		/// Adding a Keyframe
		/// </summary>
		/// <param name="key"></param>
		/// <typeparam name="T"></typeparam>
		public virtual void AddKey(Key<T> key)
		{
			if (key.TimeMilisec < 0)
			{
				Log.PrintError("Time not valid : " + key.TimeMilisec);
				return;
			}
			Keys.Add(key);
			Keys = Keys.OrderBy(a => a.TimeMilisec).ToList();
			UpdateLastValidKeysIndex(Owner.AnimationLength);
		}

		/// <summary>
		/// Updating the track. If time has passing a key, return the key
		/// </summary>
		/// <param name="positionBeforeUpdate"></param>
		/// <param name="currentPosition"></param>
		/// <param name="animationData"></param>
		/// <param name="key">key result</param>
		public virtual bool TryGetNewKey(float positionBeforeUpdate, float currentPosition, AnimationData animationData,
			out Key<T> key)
		{
			key = null;
			if (Keys.Count == 0)
				return false;

			if (_nextKeyIndex == -1)
			{
				//initialize for the first time
				key = Keys[0];
				_nextKeyIndex = 0;
				UpdateNextIndex(0, animationData);
				Log.Print("Starto");
				return true;
			}
			
			Key<T> nextKey = Keys[_nextKeyIndex];

			float min = Math.Min(positionBeforeUpdate, currentPosition);
			float max = Math.Max(positionBeforeUpdate, currentPosition);
			bool isFoundNextKey = min < nextKey.TimeMilisec && nextKey.TimeMilisec <= max;

			if (!animationData.IsLoop)
			{
				if (isFoundNextKey)
				{
					//yes we found it, next key is trapped inside min and max
					key = nextKey;

					int lastIndex = _nextKeyIndex;
					UpdateNextIndex(max - min, animationData);
					if (lastIndex == _nextKeyIndex)
						Owner.Stop();
				}
			}
			else
			{
				// When looped back from the edge of animation frame
				if (animationData.IsThisFrameLoopBack && (min >= nextKey.TimeMilisec || max < nextKey.TimeMilisec))
				{
					key = nextKey;
					UpdateNextIndex(max - min, animationData);
					Log.Print("Reversed to : " + _nextKeyIndex);
				}
				// just regular next animation, forward or backward
				else if (isFoundNextKey)
				{
					//yes we found it, next key is trapped inside min and max
					key = nextKey;

					UpdateNextIndex(max - min, animationData);
					Log.Print("AFter getting Key, next index is : " + _nextKeyIndex);
				}
			}

			return key != null;
		}

		/// <summary>
		/// Update the next index
		/// Perform delta checking, so we can get accurate next key
		/// This is stupid to be honest. Who the fuck doing animation frame LESS THAN 16 MILISECONDS
		/// </summary>
		/// <param name="deltaAnimationTime"></param>
		/// <param name="animationData"></param>
		/// <returns></returns>
		private int UpdateNextIndex(float deltaAnimationTime, AnimationData animationData)
		{
			int dir = animationData.IsReverse ? -1 : 1;
			int maybeNextIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);
			float currentKeyDeltaTime = MathF.Abs(Keys[maybeNextIndex].TimeMilisec - Keys[_nextKeyIndex].TimeMilisec);
			
			while (currentKeyDeltaTime < deltaAnimationTime)
			{
				maybeNextIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);
				if (Owner.IsLoop)
				{
					if (maybeNextIndex == _lastValidKeyIndex && dir == -1)
					{
						currentKeyDeltaTime += Keys[0].TimeMilisec + Keys[maybeNextIndex].TimeMilisec;
					}else if (maybeNextIndex == 0 && dir == 1)
					{
						currentKeyDeltaTime += Keys[0].TimeMilisec +
						                       Keys[_lastValidKeyIndex].TimeMilisec;
					}
					else
					{
						currentKeyDeltaTime += Keys[maybeNextIndex].TimeMilisec;
					}
				}
				else
				{
					if (maybeNextIndex == 0 || maybeNextIndex == _lastValidKeyIndex)
						break;
				}
			}
			
			Log.Print(_nextKeyIndex + " - " + maybeNextIndex);
			return _nextKeyIndex = maybeNextIndex;
		}

		private int TurnToValidKeysIndex(int i)
		{
			return Owner.IsLoop ? MathEx.PosMod(i, _lastValidKeyIndex) : i < 0 ? 0 : i > _lastValidKeyIndex ? _lastValidKeyIndex : i;
		}

		private int UpdateLastValidKeysIndex(float animationLength)
		{
			if (Keys.Count == 0)
				return _lastValidKeyIndex = -1;

			if (Keys.Count == 1)
				return _lastValidKeyIndex = 0;

			for (int i = 1; i < Keys.Count; i++)
			{
				if (Keys[i].TimeMilisec > animationLength)
					return _lastValidKeyIndex = i - 1;
			}

			return _lastValidKeyIndex = Keys.Count - 1;
		}
	}

	public class ValueTrack<T> : Track<T>
	{
		protected string VarName;
		protected readonly ReflectionInfoType InfoType = ReflectionInfoType.None;
		protected readonly FieldInfo FieldInfo;
		private readonly PropertyInfo _propertyInfo;
		protected Action<object, T> Setter;
		
		public ValueTrack(AnimationSequence owner,object recordObject, string varName) : base(owner,recordObject)
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
			if(InfoType == ReflectionInfoType.Property)
				Setter(RecordObject, param);
			else if(InfoType == ReflectionInfoType.Field)
				FieldInfo.SetValue(RecordObject, param);
		}
	}
	
	public class MethodTrack : Track<object>
	{
		private MethodInfo _methodInfo;
		
		public MethodTrack(AnimationSequence owner,object recordObject, string methodName) : base(owner, recordObject)
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

	//==================================================================================
	// ============================= END TRACK =====================================
	//==================================================================================
}