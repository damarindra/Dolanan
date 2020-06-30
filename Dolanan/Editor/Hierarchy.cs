using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Scene;
using Dolanan.Tools;
using ImGuiNET;

namespace Dolanan.Editor
{
#if DEBUG
	using ig = ImGuiNET.ImGui;
	internal static class Hierarchy
	{
		private static Actor actorBeginDragged;
		private static string[] names = {"01", "02", "03", "04", "05", "06"};

		internal static unsafe void DrawNames()
		{
			ig.SetNextWindowSize(new Vector2(200, 500), ImGuiCond.Appearing);
			ig.Begin("Test");
			for (int n = 0; n < names.Length; n++)
			{
				ig.PushID(n);
				ig.Button(names[n], Vector2.One * 60);

				if (ig.BeginDragDropSource(ImGuiDragDropFlags.None))
				{
					// IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
					// Marshal.WriteInt32(ptr, n);
					GCHandle handle = GCHandle.Alloc(names[n]);
					ig.SetDragDropPayload("test_drag_drop", GCHandle.ToIntPtr(handle), sizeof(int));
					
					ig.EndDragDropSource();
					handle.Free();
					// Marshal.FreeHGlobal(ptr);
				}

				if (ig.BeginDragDropTarget())
				{
					ImGuiPayloadPtr payload = ig.AcceptDragDropPayload("test_drag_drop");
					if (payload.NativePtr != null)
					{
						GCHandle handle = GCHandle.FromIntPtr(payload.Data);
						string tmp = names[n];
						// names[n] = names[Marshal.ReadInt32(payload.Data)];
						// names[Marshal.ReadInt32(payload.Data)] = tmp;
						Console.WriteLine(handle.Target.GetType());
						names[n] = (string) handle.Target;
						handle.Free();
					}
					
					ig.EndDragDropTarget();
				}
			}
			ig.End();
		}
		
		internal static void DrawHierarchy()
		{
			// DrawNames();
			
			ig.SetNextWindowSize(new Vector2(200, 500), ImGuiCond.Appearing);
			ig.Begin("Hierarchy");

			foreach (var layer in GameMgr.Game.World.Layers)
			{
				Draw(layer);
			}

			ig.End();
		}

		private static void Draw(Layer layer)
		{
			bool isShown = ig.TreeNode(layer.Name);
			ig.SameLine(ig.GetWindowWidth() - 30);
			ig.Button("yo");
			if (isShown)
			{
				foreach (var actor in layer.Actors)
				{
					if(actor.Transform.Parent == null)
						TryDraw(actor);
				}
				ig.TreePop();
			}
		}

		private static unsafe bool TryDraw(Actor actor)
		{
			// ig.Indent();
			// if (actor.Transform.Childs.Count != 0)
			// {
			// 	if (ig.TreeNode(actor.Name))
			// 	{
			// 		foreach (var transformChild in actor.Transform.Childs)
			// 		{
			// 			Draw(transformChild.Owner);
			// 		}
			// 		ig.Unindent();
			// 	}
			// }else ig.Selectable(actor.Name);

			bool isOpen = ig.TreeNode(actor.Name);
			

			if (ig.BeginDragDropSource(ImGuiDragDropFlags.None))
			{
				// IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
				// Marshal.WriteInt32(ptr, actor.Layer.Actors.IndexOf(actor));
				GCHandle tempHandle = GCHandle.Alloc(actor);
				ig.SetDragDropPayload("Move_Actor", GCHandle.ToIntPtr(tempHandle), sizeof(int));
				actorBeginDragged = actor;
				tempHandle.Free();
				ig.EndDragDropSource();
			}

			if (ig.BeginDragDropTarget() && actorBeginDragged != null)
			{
				ImGuiPayloadPtr payload = ig.AcceptDragDropPayload("Move_Actor");
				
				if (payload.NativePtr != null)
				{
					Transform2D parent = actor.Transform.Parent;
					Transform2D fromTr = actorBeginDragged.Transform;

					while (parent != null)
					{
						if (parent == fromTr)
						{
							Log.PrintError("Can't move to child!");
						
							// if(from != IntPtr.Zero) Marshal.FreeHGlobal(from);
							return false;
						}

						parent = parent.Parent;
					}
					Log.Print("Trying to move " + fromTr.Owner.Name + " to " + actor.Name);
					actorBeginDragged.SetParent(actor);
					actorBeginDragged = null;
					// force to stop iterating
					return false;
				}
				// else
				// {
				// 	// set to root
				// 	actorBeginDragged.SetParent(null);
				// }
				ig.EndDragDropTarget();
			}

			if (isOpen)
			{
				foreach (var transformChild in actor.Transform.Childs)
				{
					if(!TryDraw(transformChild.Owner))
						break;
				}
				ig.TreePop();
			}

			return true;
		}
	}
#endif
}