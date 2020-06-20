﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolanan.Engine;
using Dolanan.Tools;
using Sigil;

namespace Dolanan.Animation
{
	//==================================================================================
	// ============================= START ANIMATION =====================================
	//==================================================================================

	#region ANIMATION

	// TODO : Complete the animation system
	// TODO : Changing Animation value need a delegate to refresh the other value. For now, it can only be set on constructor
	public class AnimationSequence
	{
		private readonly List<ValueTrack<bool>> _boolTracks = new List<ValueTrack<bool>>();
		private readonly List<ValueTrack<float>> _floatTracks = new List<ValueTrack<float>>();

		private readonly List<ValueTrack<int>> _intTracks = new List<ValueTrack<int>>();
		private readonly List<MethodTrack> _methodTracks = new List<MethodTrack>();
		private readonly List<ValueTrack<string>> _stringTracks = new List<ValueTrack<string>>();
		public string Name = "";

		/// <summary>
		///     Create new Animation
		/// </summary>
		/// <param name="name"></param>
		/// <param name="animationLength">animation time in miliseconds</param>
		/// <param name="isReverse"></param>
		/// <param name="isLoop"></param>
		public AnimationSequence(string name, float animationLength, bool isReverse = false, bool isLoop = true)
		{
			Name = name;
			AnimationLength = animationLength;
			AnimationData = new AnimationData(this);
			AnimationData.IsLoop = isLoop;
			AnimationData.IsReverse = isReverse;
		}

		/// <summary>
		///     Total time animation in miliseconds
		/// </summary>
		public float AnimationLength { get; }

		public bool IsPlaying { get; private set; } = true;

		public bool IsReverse => AnimationData.IsReverse;

		public bool IsLoop => AnimationData.IsLoop;

		public AnimationData AnimationData { get; }

		public ValueTrack<T> CreateNewValueTrack<T>(string trackName, object trackedObj, string trackedField)
		{
			var newTrack = new ValueTrack<T>(this, trackedObj, trackedField);

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

		public void UpdateAnimation(float totalElapsedMiliseconds)
		{
			if (!IsPlaying)
				return;

			var positionBeforeUpdate = AnimationData.Position;
			AnimationData.UpdateAnimationData(totalElapsedMiliseconds);

			foreach (var valueTrack in _intTracks)
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, AnimationData.Position, AnimationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			foreach (var valueTrack in _stringTracks)
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, AnimationData.Position, AnimationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			foreach (var valueTrack in _floatTracks)
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, AnimationData.Position, AnimationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			foreach (var valueTrack in _boolTracks)
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, AnimationData.Position, AnimationData, out var key))
					valueTrack.SetValueToTrackedObject(key.Value);
			foreach (var valueTrack in _methodTracks)
				if (valueTrack.TryGetNewKey(positionBeforeUpdate, AnimationData.Position, AnimationData, out var key))
					valueTrack.CallMethod(key.Value);
		}

		public void Pause()
		{
			IsPlaying = false;
		}

		public void Stop()
		{
			Seek(0);
			IsPlaying = false;
		}

		public void Restart()
		{
			AnimationData.Position = 0;
			UpdateTrackPosition(0);
			IsPlaying = true;
		}

		public void Resume()
		{
			IsPlaying = true;
		}

		public void Seek(float timeInMilisec)
		{
			if (timeInMilisec < 0)
				timeInMilisec = 0;
			else if (timeInMilisec > AnimationLength)
				timeInMilisec = AnimationLength;

			AnimationData.Position = timeInMilisec;
			UpdateTrackPosition(timeInMilisec);
			Resume();
		}

		private void UpdateTrackPosition(float positionMilisec)
		{
			foreach (var valueTrack in _boolTracks)
				valueTrack.SeekTrack(positionMilisec);
			foreach (var valueTrack in _floatTracks)
				valueTrack.SeekTrack(positionMilisec);
			foreach (var valueTrack in _intTracks)
				valueTrack.SeekTrack(positionMilisec);
			foreach (var valueTrack in _methodTracks)
				valueTrack.SeekTrack(positionMilisec);
			foreach (var valueTrack in _stringTracks)
				valueTrack.SeekTrack(positionMilisec);
		}
	}

	/// <summary>
	///     DONE
	/// </summary>
	public class AnimationData
	{
		public AnimationData(AnimationSequence animationSequence)
		{
			AnimationSequence = animationSequence;
		}

		public float Position { get; set; }
		public bool IsLoop { get; set; }
		public float Speed { get; set; } = 1f;

		public bool IsReverse
		{
			get => Speed < 0;
			set
			{
				if (value) Speed = MathF.Abs(Speed) * -1;
				else Speed = MathF.Abs(Speed);
			}
		}

		/// <summary>
		///     Get this value after calling UpdateAnimationData (it is already called, don't call it again)
		///     So, this will give true value if in this frame UpdateAnimationData,
		///     the Position before update is less than AnimationLength and less than zero
		///     OR otherwise
		///     For IsLoop only
		/// </summary>
		public bool IsThisFrameLoopBack { get; private set; }

		public AnimationSequence AnimationSequence { get; }

		/// <summary>
		///     Updating animation position on timelime. It is timer
		///     DONE
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateAnimationData(float totalElapsedMiliseconds)
		{
			IsThisFrameLoopBack = false;
			Position += totalElapsedMiliseconds * Speed;

			if (!IsLoop)
			{
				if (Position < 0)
					Position = 0;
				else if (Position > AnimationSequence.AnimationLength)
					Position = AnimationSequence.AnimationLength;
			}
			else
			{
				if (Position < 0)
				{
					Position = AnimationSequence.AnimationLength - Position;
					IsThisFrameLoopBack = true;
				}
				else if (Position > AnimationSequence.AnimationLength)
				{
					Position = Position - AnimationSequence.AnimationLength;
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
		/// <summary>
		///     Create new key
		/// </summary>
		/// <param name="timeMilisec">Time in miliseconds</param>
		public Key(float timeMilisec)
		{
			TimeMilisec = timeMilisec;
		}

		public float TimeMilisec { get; }
	}

	public class Key<T> : Key
	{
		public T Value;

		/// <summary>
		///     Create new key
		/// </summary>
		/// <param name="timeMilisec">Time in miliseconds</param>
		/// <param name="val"></param>
		public Key(float timeMilisec, T val) : base(timeMilisec)
		{
			Value = val;
		}
	}

	[Obsolete(
		"Actually, object is just base class, if we store upper type, then it return to the base. SO it is useless")]
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
		Value,
		Bezier,
		Method,
		Audio
	}

	public enum ReflectionInfoType
	{
		None,
		Field,
		Property
	}

	public class Track
	{
		protected AnimationSequence Owner;

		/// <summary>
		///     Object currently being recorded
		/// </summary>
		protected object RecordObject;

		public TrackType Type;

		public Track(AnimationSequence owner, object recordObject)
		{
			Owner = owner;
			RecordObject = recordObject;
		}
	}

	/// <summary>
	///     Value Track on Property (get set) value is a lot faster than Field (regular variable). Please consider it!
	///     Changing Track value in runtime will Resulting bugg, please don't do it
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Track<T> : Track
	{
		/// <summary>
		///     Valid key index that inside AnimationTime
		/// </summary>
		private int _highestValidIndexKey = -1;

		private int _nextKeyIndex = -1;
		private List<Key<T>> Keys = new List<Key<T>>();

		public Track(AnimationSequence owner, object recordObject) : base(owner, recordObject)
		{
		}

		/// <summary>
		///     Adding a Keyframe
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

		public void SeekTrack(float position)
		{
			if (Keys.Count < 1)
				return;
			if (!Owner.AnimationData.IsReverse)
			{
				for (var i = 0; i < Keys.Count; i++)
					if (position < Keys[i].TimeMilisec)
					{
						_nextKeyIndex = i;
						return;
					}

				// if not found, the key index must be the opposite of playback mode 
				_nextKeyIndex = 0;
			}
			else
			{
				for (var i = Keys.Count - 1; i >= 0; i--)
					if (position > Keys[i].TimeMilisec)
					{
						_nextKeyIndex = i;
						return;
					}

				// if not found, the key index must be the opposite of playback mode 
				_nextKeyIndex = Keys.Count - 1;
			}
		}

		/// <summary>
		///     Updating the track. If time has passing a key, return the key
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

			var dir = animationData.IsReverse ? -1 : 1;
			if (_nextKeyIndex == -1)
			{
				//initialize for the first time
				key = Keys[0];
				_nextKeyIndex = TurnToValidKeysIndex(dir);
				//UpdateNextIndex(0, animationData);
				return true;
			}

			var nextKey = Keys[_nextKeyIndex];

			var min = Math.Min(positionBeforeUpdate, currentPosition);
			var max = Math.Max(positionBeforeUpdate, currentPosition);
			var isFoundNextKey = min < nextKey.TimeMilisec && nextKey.TimeMilisec <= max;

			if (!animationData.IsLoop)
			{
				if (isFoundNextKey)
				{
					//yes we found it, next key is trapped inside min and max
					key = nextKey;

					var lastIndex = _nextKeyIndex;
					_nextKeyIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);
					//UpdateNextIndex(max - min, animationData);
					if (lastIndex == _nextKeyIndex && dir == 1 || _nextKeyIndex == 0 && dir == -1)
						Owner.Pause();
				}
			}
			else
			{
				// Console.WriteLine(min + " - " + max + " | " + nextKey.TimeMilisec);
				// When looped back from the edge of animation frame
				if (animationData.IsThisFrameLoopBack && (min >= nextKey.TimeMilisec || max < nextKey.TimeMilisec))
				{
					key = nextKey;
					_nextKeyIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);
					//UpdateNextIndex(max - min, animationData);
				}
				// just regular next animation, forward or backward
				else if (isFoundNextKey)
				{
					//yes we found it, next key is trapped inside min and max
					key = nextKey;
					_nextKeyIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);

					//UpdateNextIndex(max - min, animationData);
				}
			}

			return key != null;
		}

		/// <summary>
		///     Update the next index
		///     Perform delta checking, so we can get accurate next key
		///     This is stupid to be honest. Who the fuck doing animation frame LESS THAN 16 MILISECONDS...
		/// </summary>
		/// <param name="deltaAnimationTime"></param>
		/// <param name="animationData"></param>
		/// <returns></returns>
		[Obsolete]
		private int UpdateNextIndex(float deltaAnimationTime, AnimationData animationData)
		{
			var dir = animationData.IsReverse ? -1 : 1;
			var maybeNextIndex = _nextKeyIndex + dir;
			maybeNextIndex = TurnToValidKeysIndex(maybeNextIndex);

			var currentKeyDeltaTime = MathF.Abs(Keys[maybeNextIndex].TimeMilisec - Keys[_nextKeyIndex].TimeMilisec);

			while (currentKeyDeltaTime < deltaAnimationTime)
			{
				maybeNextIndex = TurnToValidKeysIndex(_nextKeyIndex + dir);
				if (Owner.IsLoop)
				{
					if (maybeNextIndex == _highestValidIndexKey && dir == -1)
						currentKeyDeltaTime += Keys[0].TimeMilisec + Keys[maybeNextIndex].TimeMilisec;
					else if (maybeNextIndex == 0 && dir == 1)
						currentKeyDeltaTime += Keys[0].TimeMilisec +
						                       Keys[_highestValidIndexKey].TimeMilisec;
					else
						currentKeyDeltaTime += Keys[maybeNextIndex].TimeMilisec;
				}
				else
				{
					if (maybeNextIndex == 0 || maybeNextIndex == _highestValidIndexKey)
						break;
					currentKeyDeltaTime += Keys[maybeNextIndex].TimeMilisec;
				}
			}

			return _nextKeyIndex = maybeNextIndex;
		}

		private int TurnToValidKeysIndex(int i)
		{
			if (_highestValidIndexKey == 0)
				return 0;
			return Owner.IsLoop ? MathEx.PosMod(i, _highestValidIndexKey + 1) :
				i < 0 ? 0 :
				i > _highestValidIndexKey ? _highestValidIndexKey : i;
		}

		private int UpdateLastValidKeysIndex(float animationLength)
		{
			if (Keys.Count == 0)
				return _highestValidIndexKey = -1;

			if (Keys.Count == 1)
				return _highestValidIndexKey = 0;

			for (var i = Keys.Count - 1; i >= 0; i--)
				if (Keys[i].TimeMilisec <= animationLength)
					return _highestValidIndexKey = i;

			return _highestValidIndexKey = Keys.Count - 1;
		}
	}

	public class ValueTrack<T> : Track<T>
	{
		private readonly FieldInfo _fieldInfo;
		private readonly ReflectionInfoType _infoType = ReflectionInfoType.None;
		private readonly PropertyInfo _propertyInfo;
		private readonly Action<object, T> _setter;
		private string _varName;

		public ValueTrack(AnimationSequence owner, object recordObject, string varName) : base(owner, recordObject)
		{
			_varName = varName;
			Type = TrackType.Value;

			var t = recordObject.GetType();
			while (t != null && _infoType == ReflectionInfoType.None)
			{
				_propertyInfo = t.GetProperty(varName,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (_propertyInfo != null)
				{
					_infoType = ReflectionInfoType.Property;
					var setterEmit = Emit<Action<object, T>>
						.NewDynamicMethod()
						.LoadArgument(0)
						.CastClass(RecordObject.GetType())
						.LoadArgument(1)
						.Call(_propertyInfo.GetSetMethod(true))
						.Return();
					_setter = setterEmit.CreateDelegate();

					break;
				}

				_fieldInfo = t.GetField(varName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (_fieldInfo != null)
				{
					_infoType = ReflectionInfoType.Field;
					break;
				}

				t = t.BaseType;
			}

			if (_fieldInfo == null && _propertyInfo == null)
				Log.PrintError("Variable name : " + varName + "not found at " + recordObject.GetType());
		}

		public void SetValueToTrackedObject(T param)
		{
			if (_infoType == ReflectionInfoType.Property)
				_setter(RecordObject, param);
			else if (_infoType == ReflectionInfoType.Field)
				_fieldInfo.SetValue(RecordObject, param);
			// Console.WriteLine(param);
		}
	}

	public class MethodTrack : Track<object>
	{
		private readonly MethodInfo _methodInfo;

		public MethodTrack(AnimationSequence owner, object recordObject, string methodName) : base(owner, recordObject)
		{
			var t = recordObject.GetType();
			while (t != null)
			{
				_methodInfo = t.GetMethod(methodName,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (_methodInfo != null) break;

				t = t.BaseType;
			}

			// if(_methodInfo == null)
			// 	Log.PrintError("Method name : " + methodName + "not found at " + recordObject.GetType());
		}

		/// <summary>
		///     Call method
		/// </summary>
		/// <param name="p">parameter from the original method</param>
		/// <returns></returns>
		public object CallMethod(object p)
		{
			if (_methodInfo != null)
				return _methodInfo.Invoke(RecordObject, new[] {p});
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