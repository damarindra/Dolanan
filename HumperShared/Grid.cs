﻿namespace Humper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Base;

	/// <summary>
	/// Basic spacial hashing of world's boxes.
	/// </summary>
	public class Grid
	{
		public struct Cell
		{
			public Cell(int x, int y, float cellSize)
			{
				this.Bounds = new RectangleF(x * cellSize, y * cellSize, cellSize, cellSize);
				children = new List<IBox>();
			}

			public readonly RectangleF Bounds { get; }

			public IEnumerable<IBox> Children => this.children;

			private List<IBox> children;

			public void Add(IBox box)
			{
				this.children.Add(box);
			}

			public bool Contains(IBox box)
			{
				return this.children.Contains(box);
			}

			public bool Remove(IBox box)
			{
				return this.children.Remove(box);
			}

			public int Count()
			{
				return this.children.Count;
			}

			public static Cell Zero => _emptyCell;
			static readonly Cell _emptyCell = new Cell(0,0,0);

			public static bool operator ==(Cell g1, Cell g2)
			{
				return g1.Bounds == g2.Bounds;
			}

			public static bool operator !=(Cell g1, Cell g2)
			{
				return g1.Bounds == g2.Bounds;
			}
		}

		public Grid(int width, int height, float cellSize)
		{
			this.Cells = new Cell[width, height];
			this.CellSize = cellSize;
		}

		public float CellSize { get; set; }

		#region Size

		public float Width => this.Columns * CellSize;

		public float Height => this.Rows * CellSize;

		public int Columns => this.Cells.GetLength(0);

		public int Rows => this.Cells.GetLength(1);

		#endregion

		public Cell[,] Cells { get; private set; }

		public IEnumerable<Cell> QueryCells(float x, float y, float w, float h)
		{
			var minX = (int)(x / this.CellSize);
			var minY = (int)(y / this.CellSize);
			var maxX = (int)((x + w - 1) / this.CellSize) + 1;
			var maxY = (int)((y + h - 1) / this.CellSize) + 1;

			minX = Math.Max(0, minX);
			minY = Math.Max(0, minY);
			maxX = Math.Min(this.Columns - 1, maxX);
			maxY = Math.Min(this.Rows - 1, maxY);

			//List<Cell> result = new List<Cell>();
			int maxIndex = (maxX - minX + 1) * (maxY - minY + 1);
			Cell[] results = new Cell[maxIndex];

			for (int ix = minX; ix <= maxX; ix++)
			{
				for (int iy = minY; iy <= maxY; iy++)
				{
					var cell = Cells[ix, iy];

					if (cell == Cell.Zero)
					{
						cell = new Cell(ix,iy,CellSize);
						Cells[ix, iy] = cell;
					}

					int index = (iy - minY) + (ix - minX) * (maxY - minY + 1);
					results[index] = cell;
					//result.Add(cell);
				}
			}

			return results;
			//return result;

		}

		public IEnumerable<IBox> QueryBoxes(float x, float y, float w, float h)
		{
			var cells = this.QueryCells(x, y, w, h);

			return cells.SelectMany((cell) => cell.Children).Distinct();
		}

		public void Add(IBox box)
		{
			var cells = this.QueryCells(box.X, box.Y, box.Width, box.Height);

			foreach (var cell in cells)
			{
				if(!cell.Contains(box))
					cell.Add(box);
			}
		}

		public void Update(IBox box, RectangleF from)
		{
			var fromCells = this.QueryCells(from.X, from.Y, from.Width, from.Height);
			var removed = false;
			foreach (var cell in fromCells)
			{
				removed |= cell.Remove(box);
			}

			if(removed)
				this.Add(box);
		}

		public bool Remove(IBox box)
		{
			var cells = this.QueryCells(box.X, box.Y, box.Width, box.Height);

			var removed = false;
			foreach (var cell in cells)
			{
				removed |= cell.Remove(box);
			}

			return removed;
		}

		public override string ToString()
		{
			return string.Format("[Grid: Width={0}, Height={1}, Columns={2}, Rows={3}]", Width, Height, Columns, Rows);
		}
	}
}

