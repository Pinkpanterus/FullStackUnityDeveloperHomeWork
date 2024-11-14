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
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => _inventoryCells.GetLength(0);
        public int Height => _inventoryCells.GetLength(1);

        public int Count => Items.Count(i => i != null);
        public Item[] Items => _inventoryCells.Cast<Item>().Distinct().ToArray();

        private Item[,] _inventoryCells;


        public Inventory(in int width, in int height)
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException();

            _inventoryCells = new Item[width, height];
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

            _inventoryCells = new Item[width, height];

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

            _inventoryCells = new Item[width, height];

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
            // Вроде дублирование кода, но не понял как по-другому сделать
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("width and height must be greater than 1");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _inventoryCells = new Item[width, height];

            foreach (KeyValuePair<Item, Vector2Int> keyValuePair in items)
            {
                if (AddItem(keyValuePair.Key, keyValuePair.Value))
                    OnAdded?.Invoke(keyValuePair.Key, keyValuePair.Value);
            }

            Debug.Log($"{_inventoryCells}");
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items
        )
        {
            // Вроде дублирование кода, но не понял как по-другому сделать
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("width and height must be greater than 1");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _inventoryCells = new Item[width, height];

            foreach (Item item in items)
            {
                var itemSize = item.Size;
                FindFreePosition(itemSize, out var position);

                if (AddItem(item, position))
                    OnAdded?.Invoke(item, position);
            }
        }


        private bool isPositionValid(in Vector2Int position)
        {
            return position.x >= 0 && position.x < _inventoryCells.GetLength(0) && position.y >= 0 && position.y < _inventoryCells.GetLength(1);
        }

        private bool isCellRangeFree(in Vector2Int position, in Vector2Int range)
        {
            int xRange = range.x;
            int yRange = range.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    var checkingPosition = new Vector2Int(xPosition + i, yPosition + j);
                    if (!isPositionValid(checkingPosition) || IsOccupied(checkingPosition))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private bool isCellRangeFree(in Vector2Int position, Item item)
        {
            var itemPosition = GetPositions(item);
            
            int xRange = item.Size.x;
            int yRange = item.Size.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    var checkingPosition = new Vector2Int(xPosition + i, yPosition + j);
                    if (!isPositionValid(checkingPosition) || (IsOccupied(checkingPosition)) && !itemPosition.Contains(checkingPosition))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            if (!isPositionValid(position))
                throw new ArgumentOutOfRangeException(paramName: nameof(position));
            
            if (item == null)
                return false;
           
            if(Items.Contains(item))
                return false;

            Vector2Int itemSize = item.Size;
            if(itemSize.x < 1 || itemSize.y < 1)
                throw new ArgumentException();
                
            return isCellRangeFree(position, itemSize);
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
            if (CanAddItem(item, position))
            {
                OccupyCellRange(position, item);
                OnAdded?.Invoke(item, position);

                return true;
            }
            
            return false;
        }

        private void PlaceItemInInventory(in Item item)
        {
            if (item == null)
                return;

            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    if (isCellRangeFree(position, item.Size))
                        OccupyCellRange(position, item);
                }
            }
        }

        private void OccupyCellRange(Vector2Int position, Item item)
        {
            Vector2Int itemSize = item.Size;
            int xRange = itemSize.x;
            int yRange = itemSize.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    _inventoryCells[xPosition + i, yPosition + j] = item;
                }
            }
        }

        public bool AddItem(in Item item, in int posX, in int posY)
        {
            // if (item == null || !CanAddItem(item, new Vector2Int(posX, posY)))
            //     return false;
            //
            // // if (!isCellRangeFree(new Vector2Int(posX, posY), item.Size))
            // //     return false;
            //
            // AddItem(item, posX, posY);
            // return true;

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
            
            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    if (isCellRangeFree(position, size))
                    {
                        freePosition = position;
                        return true;
                    }
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
            
            return Items.Contains(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
        {
            // if (!isPositionValid(position))
            //     return false;

            int x = position.x;
            int y = position.y;

            return _inventoryCells[x, y] != null;
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
            if (!isPositionValid(position))
                return false;

            int x = position.x;
            int y = position.y;

            return _inventoryCells[x, y] == null;
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
            if (item == null || !Contains(item))
            {
                position = Vector2Int.zero;
                return false;
            }

            var itemPositions = GetPositions(item);
            foreach (Vector2Int coord in itemPositions)
            {
                _inventoryCells[coord.x, coord.y] = null;
            }

            position = new Vector2Int(itemPositions.Min(cell => cell.x), itemPositions.Min(cell => cell.y));
            OnRemoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            // if (isPositionValid(position))
            //     return _inventoryCells[position.x, position.y];
            //
            // return null;
            if (position.x == 0 && position.y == 0)
                throw new NullReferenceException();
            
            if (!isPositionValid(position))
                throw new IndexOutOfRangeException();
            
            return _inventoryCells[position.x, position.y];
        }

        public Item GetItem(in int x, in int y)
            => GetItem(new Vector2Int(x, y));

        public bool TryGetItem(in Vector2Int position, out Item item)
        {
            if (!isPositionValid(position))
            {
                item = null;
                return false;
            }

            item = GetItem(position);
            return true;
        }

        public bool TryGetItem(in int x, in int y, out Item item)
            => TryGetItem(new Vector2Int(x, y), out item);

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(in Item item)
        {
            if (TryGetPositions(item, out Vector2Int[] positions))
            {
                return positions;
            }

            return null;
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            if (item is null)
                throw new NullReferenceException();
            // {
            //     positions = null;
            //     return false; 
            // }
            
            if (!Items.Contains(item))
                throw new KeyNotFoundException();

            List<Vector2Int> positionList = new List<Vector2Int>();
            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    if (_inventoryCells[i, j] == item)
                        positionList.Add(new Vector2Int(i, j));
                }
            }

            positions = positionList.Count > 0 ? positionList.ToArray() : null;
            return positions?.Length > 0;
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
                return;
            
            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    _inventoryCells[i, j] = null;
                }
            }
            
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            if (name == "")
                return 1;
            
            if (name is null)
                return Items.Count(i => i.Name is null);
            
            return Items.Count(i => i.Name == name);
        }

        /// <summary>
        /// Moves a specified item to a target position if it exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int newPosition)
        {
            if (item == null)
                // throw new ArgumentNullException(nameof(item));
                throw new System.ArgumentNullException(nameof(item));
            
            if (!Items.Contains(item))
                return false;

            if (!isCellRangeFree(newPosition, item))
                return false;

            Vector2Int[] itemCells = GetPositions(item);
            var currentPosition = new Vector2Int(itemCells.Min(cell => cell.x), itemCells.Min(cell => cell.y));
            var coordDelta = newPosition - currentPosition;
            var newItemCells = itemCells.Select(cell => cell + coordDelta).ToArray();

            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    if (itemCells.Contains(new Vector2Int(i, j)))
                        _inventoryCells[i, j] = null;

                    if (newItemCells.Contains(new Vector2Int(i, j)))
                        _inventoryCells[i, j] = item;
                }
            }

            OnMoved?.Invoke(item, newPosition);
            return true;
        }

        /// <summary>
        /// Reorganizes inventory space to make the free area uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            if (Count == 0)
                return;

            Vector2Int? emptyCell = null;

       
            for (int i = 0; i < _inventoryCells.GetLength(1); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(0); j++)
                {
                    if (_inventoryCells[i, j] == null)
                    {
                        emptyCell = new Vector2Int(i, j);
                    }
                    else
                    {
                        if (emptyCell != null)
                        {
                            var item = GetItem(i, j);
                            MoveItem(item, emptyCell.Value);
                            emptyCell = null;
                        }
                    }
                }
            }
            // Debug.Log(_inventoryCells.GetLength(0) * _inventoryCells.GetLength(1));
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            if (matrix == null)
                return;
            
            for (int i = 0; i < _inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < _inventoryCells.GetLength(1); j++)
                {
                    matrix[i, j] = _inventoryCells[i, j];
                }
            }
        }

        public IEnumerator<Item> GetEnumerator()
            => _inventoryCells.Cast<Item>().Distinct().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}