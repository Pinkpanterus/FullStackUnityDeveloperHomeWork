using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable NotResolvedInText

namespace Inventories
{
    public sealed class Inventory : IEnumerable<Item>
    {
        private static readonly SizeComparer SizeComparer = new SizeComparer();
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => cells.GetLength(0);
        public int Height => cells.GetLength(1);

        public int Count => items.Count;

        private readonly Item[,] cells;
        private readonly Dictionary<Item, Vector2Int> items;


        public Inventory(in int width, in int height)
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException();

            cells = new Item[width, height];
            items = new Dictionary<Item, Vector2Int>();
        }

        public Inventory(
            in int width,
            in int height,
            params KeyValuePair<Item, Vector2Int>[] items
        )
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException();

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            cells = new Item[width, height];
            this.items = new Dictionary<Item, Vector2Int>();

            foreach (KeyValuePair<Item, Vector2Int> keyValuePair in items)
            {
                if (AddItem(keyValuePair.Key, keyValuePair.Value))
                    OnAdded?.Invoke(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public Inventory(
            in int width,
            in int height,
            params Item[] items
        )
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("width and height must be greater than 1");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            cells = new Item[width, height];
            this.items = new Dictionary<Item, Vector2Int>();

            foreach (Item item in items)
            {
                var itemSize = item.Size;
                FindFreePosition(itemSize, out var position);

                if (AddItem(item, position))
                    OnAdded?.Invoke(item, position);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<KeyValuePair<Item, Vector2Int>> items
        )
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("width and height must be greater than 1");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            cells = new Item[width, height];
            this.items = new Dictionary<Item, Vector2Int>();

            foreach (KeyValuePair<Item, Vector2Int> keyValuePair in items)
            {
                if (AddItem(keyValuePair.Key, keyValuePair.Value))
                    OnAdded?.Invoke(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items
        )
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("width and height must be greater than 1");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            cells = new Item[width, height];
            this.items = new Dictionary<Item, Vector2Int>();

            foreach (Item item in items)
            {
                var itemSize = item.Size;
                FindFreePosition(itemSize, out var position);

                if (AddItem(item, position))
                    OnAdded?.Invoke(item, position);
            }
        }


        private bool IsPositionValid(in Vector2Int position)
        {
            return position.x >= 0 && position.x < cells.GetLength(0) && position.y >= 0 && position.y < cells.GetLength(1);
        }


        private bool IsCellRangeFree(in Vector2Int position, Vector2Int itemSize)
        {
            int xRange = itemSize.x;
            int yRange = itemSize.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            for (int j = 0; j < yRange; j++)
            {
                var checkingPosition = new Vector2Int(xPosition + i, yPosition + j);
                if (!IsPositionValid(checkingPosition) || IsOccupied(checkingPosition))
                    return false;
            }

            return true;
        }


        private bool IsCellRangeFree(in Vector2Int position, Item item)
        {
            var itemSize = item.Size;

            int xRange = itemSize.x;
            int yRange = itemSize.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    var checkingPosition = new Vector2Int(xPosition + i, yPosition + j);
                    if (!IsPositionValid(checkingPosition))
                        return false;

                    Item cellItem = cells[checkingPosition.x, checkingPosition.y];
                    if (IsOccupied(checkingPosition) && !cellItem.Equals(item))
                        return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            if (item == null)
                return false;

            if (Contains(item))
                return false;

            Vector2Int itemSize = item.Size;
            if (itemSize.x < 1 || itemSize.y < 1)
                throw new ArgumentException();

            return IsCellRangeFree(position, item);
        }

        public bool CanAddItem(in Item item, in int posX, in int posY)
        {
            return CanAddItem(item, new Vector2Int(posX, posY));
        }

        /// <summary>
        /// Adds an item on a specified position if not exists
        /// </summary>
        public bool AddItem(in Item item, in Vector2Int position)
        {
            if (AddItemInternal(item, position))
            {
                OnAdded?.Invoke(item, position);
                return true;
            }

            return false;
        }

        private bool AddItemInternal(in Item item, in Vector2Int position)
        {
            if (CanAddItem(item, position))
            {
                OccupyCellRange(position, item);
                items.Add(item, position);

                return true;
            }

            return false;
        }

        private void OccupyCellRange(Vector2Int position, Item item)
        {
            Vector2Int itemSize = item.Size;
            for (int i = 0; i < itemSize.x; i++)
            for (int j = 0; j < itemSize.y; j++)
                cells[position.x + i, position.y + j] = item;
        }

        public bool AddItem(in Item item, in int posX, in int posY)
        {
            return AddItem(item, new Vector2Int(posX, posY));
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(in Item item)
        {
            if (item is null || Contains(item))
                return false;

            var itemSize = item.Size;
            if (itemSize.x <= 0 || itemSize.y <= 0)
                throw new ArgumentException();

            return FindFreePosition(item.Size, out Vector2Int position);
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(in Item item)
        {
            if (item == null)
                return false;

            if (Contains(item))
                return false;

            if (FindFreePosition(item.Size, out Vector2Int position))
            {
                AddItem(item, position);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(in Vector2Int size, out Vector2Int freePosition)
        {
            if (size.x <= 0 || size.y <= 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(size));

            if (size.x > Width || size.y > Height)
            {
                freePosition = Vector2Int.zero;
                return false;
            }

            for (int j = 0; j < cells.GetLength(1); j++)
            for (int i = 0; i < cells.GetLength(0); i++)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    if (IsCellRangeFree(position, size))
                    {
                        freePosition = position;
                        return true;
                    }
                }

            freePosition = Vector2Int.zero;
            return false;
        }


        /// <summary>
        /// Checks if a specified item exists
        /// </summary>
        public bool Contains(in Item item)
        {
            if (item == null)
                return false;

            return items.ContainsKey(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
        {
            int x = position.x;
            int y = position.y;

            return cells[x, y] != null;
        }

        public bool IsOccupied(in int x, in int y)
        {
            return IsOccupied(new Vector2Int(x, y));
        }

        /// <summary>
        /// Checks if a position is free
        /// </summary>
        public bool IsFree(in Vector2Int position)
        {
            if (!IsPositionValid(position))
                return false;

            int x = position.x;
            int y = position.y;

            return cells[x, y] == null;
        }

        public bool IsFree(in int x, in int y)
            => IsFree(new Vector2Int(x, y));

        /// <summary>
        /// Removes a specified item if exists
        /// </summary>
        public bool RemoveItem(in Item item)
        {
            return RemoveItem(item, out Vector2Int position);
        }

        public bool RemoveItem(in Item item, out Vector2Int position)
        {
            if (RemoveItemInternal(item, out position))
            {
                OnRemoved?.Invoke(item, position);
                return true;
            }

            return false;
        }

        private bool RemoveItemInternal(in Item item, out Vector2Int position)
        {
            if (item == null || !Contains(item))
            {
                position = Vector2Int.zero;
                return false;
            }

            Vector2Int pivot = items[item];
            Vector2Int itemSize = item.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    cells[pivot.x + x, pivot.y + y] = null;
                }
            }

            position = pivot;
            items.Remove(item);
            return true;
        }

        private Vector2Int GetItemPosition(in Item item)
        {
            items.TryGetValue(item, out Vector2Int position);
            return position;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            if (!IsPositionValid(position))
                throw new IndexOutOfRangeException();

            // Item item = cells[position.x, position.y];
            // if (item is null)
            //     throw new NullReferenceException();

            Item item = null;
            foreach (var inventoryItem in items.Keys)
            {
                var itemCells = GetPositions(inventoryItem);
                foreach (Vector2Int cell in itemCells)
                {
                    if (cell.x.Equals(position.x) && cell.y.Equals(position.y))
                    {
                        item = inventoryItem;
                    }
                }
            }
            
            if (item is null)
                throw new NullReferenceException();

            return item;
        }

        public Item GetItem(in int x, in int y)
            => GetItem(new Vector2Int(x, y));

        public bool TryGetItem(in Vector2Int position, out Item item)
        {
            if (!IsPositionValid(position))
            {
                item = null;
                return false;
            }

            item = cells[position.x, position.y];
            return item is not null;
        }

        public bool TryGetItem(in int x, in int y, out Item item)
            => TryGetItem(new Vector2Int(x, y), out item);

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(in Item item)
        {
            if (item is null)
                throw new NullReferenceException();

            if (!Contains(item))
                throw new KeyNotFoundException();

            if (TryGetPositions(item, out Vector2Int[] positions))
            {
                return positions;
            }

            return null;
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            if (item is null)
            {
                positions = null;
                return false;
            }
            
            if (!items.TryGetValue(item, out Vector2Int position))
            {
                positions = null;
                return false;
            }

            var itemSize = item.Size;
            positions = new Vector2Int[itemSize.x * itemSize.y];

            int i = 0;
            for (int x = 0; x < itemSize.x; x++)
            for (int y = 0; y < itemSize.y; y++)
                positions[i++] = new Vector2Int(x + position.x, y + position.y);

            return true;
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
                return;

            Array.Clear(cells, 0, cells.Length);
            items.Clear();

            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            if (name is "" or null)
                return 1;

            int itemCount = 0;
            foreach (var item in items.Keys)
            {
                if (item.Name == name)
                    itemCount++;
            }

            return itemCount;
        }

        /// <summary>
        /// Moves a specified item to a target position if it exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int newPosition)
        {
            if (MoveItemInternal(item, newPosition))
            {
                OnMoved?.Invoke(item, newPosition);
                return true;
            }

            return false;
        }


        private bool MoveItemInternal(Item item, Vector2Int newPosition)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!items.ContainsKey(item))
                return false;

            if (!IsCellRangeFree(newPosition, item))
                return false;

            ClearCellRange(item);
            OccupyCellRange(newPosition, item);

            return true;
        }

        private void ClearCellRange(Item item)
        {
            Vector2Int[] itemCells = GetPositions(item);
            foreach (Vector2Int cell in itemCells)
                cells[cell.x, cell.y] = null;
        }

        /// <summary>
        /// Reorganizes inventory space to make the free area uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            if (Count == 0)
                return;

            Item[] itemArray = items.Keys.ToArray();
            Array.Sort(itemArray, SizeComparer);

            Array.Clear(cells, 0, cells.Length);
            items.Clear();

            foreach (Item item in itemArray)
            {
                if (FindFreePosition(item.Size, out Vector2Int position))
                    AddItemInternal(item, position);
            }
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            if (matrix == null || cells == null || matrix.GetLength(0) != cells.GetLength(0) || matrix.GetLength(1) != cells.GetLength(1))
                return;

            Array.Copy(cells, 0, matrix, 0, cells.Length);
        }

        public IEnumerator<Item> GetEnumerator()
            => items.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}