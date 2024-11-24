using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor;
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

        public int Width => inventoryCells.GetLength(0);
        public int Height => inventoryCells.GetLength(1);

        // public int Count => Items.Count(i => i != null); //TODO LINQ
        public int Count => inventoryItemsPosition.Count;
        public Item[] Items => inventoryItemsPosition.Keys.ToArray();

        private readonly Item[,] inventoryCells;
        private readonly Dictionary<Item, Vector2Int> inventoryItemsPosition;
        


        public Inventory(in int width, in int height)
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException();

            inventoryCells = new Item[width, height];
            inventoryItemsPosition = new Dictionary<Item, Vector2Int>();
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

            inventoryCells = new Item[width, height];
            inventoryItemsPosition = new Dictionary<Item, Vector2Int>();

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

            inventoryCells = new Item[width, height];
            inventoryItemsPosition = new Dictionary<Item, Vector2Int>();

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

            inventoryCells = new Item[width, height];
            inventoryItemsPosition = new Dictionary<Item, Vector2Int>();

            foreach (KeyValuePair<Item, Vector2Int> keyValuePair in items)
            {
                if (AddItem(keyValuePair.Key, keyValuePair.Value))
                    OnAdded?.Invoke(keyValuePair.Key, keyValuePair.Value);
            }

            // Debug.Log($"{inventoryCells}");
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

            inventoryCells = new Item[width, height];
            inventoryItemsPosition = new Dictionary<Item, Vector2Int>();

            foreach (Item item in items)
            {
                var itemSize = item.Size;
                FindFreePosition(itemSize, out var position);

                if (AddItem(item, position))
                    OnAdded?.Invoke(item, position);
            }
        }


        // private bool IsPositionValid(in Vector2Int position)
        // {
        //     return position.x >= 0 && position.x < inventoryCells.GetLength(0) && position.y >= 0 && position.y < inventoryCells.GetLength(1);
        // }
        
        private bool IsPositionValid(in Vector2Int position, Item[,] inventory)
        {
            return position.x >= 0 && position.x < inventory.GetLength(0) && position.y >= 0 && position.y < inventory.GetLength(1);
        }

        private bool IsCellRangeFreeInternal(in Vector2Int position, in Item item, Item[,] inventory)
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
                    if(!IsPositionValid(checkingPosition, inventory)) 
                        return false;
                    
                    Item cellItem = inventoryCells[checkingPosition.x, checkingPosition.y];
                    if (IsOccupied(checkingPosition) && !cellItem.Equals(item))
                        return false;
                }
            }
            return true;
        }
        
        private bool IsCellRangeFree(in Vector2Int position, Vector2Int itemSize, Item[,] inventory)
        {
            int xRange = itemSize.x;
            int yRange = itemSize.y;

            int xPosition = position.x;
            int yPosition = position.y;

            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    var checkingPosition = new Vector2Int(xPosition + i, yPosition + j);
                    if (!IsPositionValid(checkingPosition, inventory) || IsOccupied(checkingPosition))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        private bool IsCellRangeFree(in Vector2Int position, Item item, Item[,] inventory)
        {
            return IsCellRangeFreeInternal(position, item, inventory);
        }


        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            // if (!IsPositionValid(position))
            //     throw new ArgumentOutOfRangeException(paramName: nameof(position));
            
            if (item == null)
                return false;
           
            if(Contains(item))
                return false;

            Vector2Int itemSize = item.Size;
            if(itemSize.x < 1 || itemSize.y < 1)
                throw new ArgumentException();
                
            // return IsCellRangeFree(position, itemSize);
            return IsCellRangeFree(position, item, inventoryCells);
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
                inventoryItemsPosition.Add(item, position);
                
                return true;
            }
            return false;
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
                    inventoryCells[xPosition + i, yPosition + j] = item;
                }
            }
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
            // if (size.x <= 0 || size.y <= 0)
            //     throw new ArgumentOutOfRangeException(paramName: nameof(size));
            //
            // if (size.x > Width || size.y > Height)
            // {
            //     freePosition = Vector2Int.zero;
            //     return false;
            // }
            //
            // for (int i = 0; i < inventoryCells.GetLength(0); i++)
            // {
            //     for (int j = 0; j < inventoryCells.GetLength(1); j++)
            //     {
            //         Vector2Int position = new Vector2Int(i, j);
            //         if (IsCellRangeFree(position, size))
            //         {
            //             freePosition = position;
            //             return true;
            //         }
            //     }
            // }
            //
            // freePosition = Vector2Int.zero;
            // return false;
            return FindFreePositionInternal(size, inventoryCells, out freePosition);
        }
        
        public bool FindFreePositionInternal(in Vector2Int size, in Item[,] inventory, out Vector2Int freePosition)
        {
            if (size.x <= 0 || size.y <= 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(size));

            if (size.x > Width || size.y > Height)
            {
                freePosition = Vector2Int.zero;
                return false;
            }
            
            // for (int i = 0; i < inventory.GetLength(0); i++)
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                // for (int j = 0; j < inventory.GetLength(1); j++)
                for (int i = 0; i < inventory.GetLength(0); i++)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    if (IsCellRangeFree(position, size, inventory))
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
            
            return inventoryItemsPosition.ContainsKey(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
        {
            int x = position.x;
            int y = position.y;

            return inventoryCells[x, y] != null;
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
            if (!IsPositionValid(position, inventoryCells))
                return false;

            int x = position.x;
            int y = position.y;

            return inventoryCells[x, y] == null;
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

            var itemPositions = GetPositions(item);
            foreach (Vector2Int coord in itemPositions)
            {
                inventoryCells[coord.x, coord.y] = null;
            }
            
            // position = new Vector2Int(itemPositions.Min(cell => cell.x), itemPositions.Min(cell => cell.y));
            position = GetItemPosition(item);
            inventoryItemsPosition.Remove(item);
            return true;
        }

        private Vector2Int GetItemPosition(in Item item)
        {
            // for (int i = 0; i < inventoryCells.GetLength(0); i++)
            // {
            //     for (int j = 0; j < inventoryCells.GetLength(1); j++)
            //     {
            //         if (inventoryCells[i, j].Equals(item))
            //             return new Vector2Int(i, j);
            //     }
            // }
            
            // return Vector2Int.zero;
            return inventoryItemsPosition.ContainsKey(item) ? inventoryItemsPosition[item] : Vector2Int.zero;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            if (!IsPositionValid(position, inventoryCells))
                throw new IndexOutOfRangeException();

            Item item = null;
            foreach (var inventoryItem in inventoryItemsPosition.Keys)
            {
                
                
                var itemCells = GetPositions(inventoryItem);
                foreach (Vector2Int cell in itemCells)
                {
                    if (cell.x.Equals(position.x) && cell.y.Equals(position.y))
                        item = inventoryItem;
                }
            }
            
            // if (item is null)
            //     throw new NullReferenceException();
           
            return item;
        }

        public Item GetItem(in int x, in int y)
            => GetItem(new Vector2Int(x, y));

        public bool TryGetItem(in Vector2Int position,  out Item item)
        {
            if (!IsPositionValid(position, inventoryCells))
            {
                item = null;
                return false;
            }

            item = GetItem(position);
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
            
            if (TryGetPositions(item, out Vector2Int[] positions))
            {
                return positions;
            }

            return null;
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            if (item is null)
                // throw new NullReferenceException();
            {
                positions = null;
                return false;
            }

            if (!Contains(item))
                // throw new KeyNotFoundException();
            {
                positions = null;
                return false;
            }

            var itemPosition = GetItemPosition(item);
            var itemSize = item.Size;

            List<Vector2Int> positionList = new List<Vector2Int>();
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    positionList.Add(new Vector2Int(x + itemPosition.x, y + itemPosition.y));
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
            
            Array.Clear(inventoryCells, 0, inventoryCells.Length);
            inventoryItemsPosition.Clear();
            
            // for (int i = 0; i < inventoryCells.GetLength(0); i++)
            // {
            //     for (int j = 0; j < inventoryCells.GetLength(1); j++)
            //     {
            //         inventoryCells[i, j] = null;
            //     }
            // }
            
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            if (name == "" || name == null)
                return 1;
            //
            // Item searchingItem = null;
            // int nullCellCount = 0;
            // int itemCellCount = 0;
            // foreach (Item item in inventoryCells)
            // {
            //     if (item is null)
            //         nullCellCount++; 
            //     else if (item.Name.Equals(name))
            //     {
            //         itemCellCount++;
            //         searchingItem = item;
            //     }
            // }
            // var itemSize = searchingItem.Size.x * searchingItem.Size.y;
            // return searchingItem is not null? itemCellCount / itemSize : nullCellCount;
            
            int itemCount = 0;
            foreach (var item in inventoryItemsPosition.Keys)
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
            if (MoveItemInternal(item, newPosition, inventoryCells))
            {
                OnMoved?.Invoke(item, newPosition);
                return true;
            }
            return false;
        }

        private bool MoveItemInternal(Item item, Vector2Int newPosition, Item[,] invCells)
        {
            // Debug.Log($"{item.Name} is null: {item is null}, is moving to {newPosition}");
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            
            if (!Items.Contains(item))
                return false;

            if (!IsCellRangeFree(newPosition, item, invCells))
                return false;

            Vector2Int[] itemCells = GetPositions(item);
            Vector2Int currentPosition = GetItemPosition(item);
            Vector2Int coordDelta = newPosition - currentPosition;
            
            foreach (Vector2Int cell in itemCells)
            {
                var newCell = cell + coordDelta;
                invCells[cell.x, cell.y] = null;
                invCells[newCell.x,newCell.y] = item;
            }

            return true;
        }

        /// <summary>
        /// Reorganizes inventory space to make the free area uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            if (Count == 0)
                return;
            
            Dictionary<Item, Vector2Int> inventoryItemsPositionCopy = new Dictionary<Item, Vector2Int>(inventoryItemsPosition);
            var items = inventoryItemsPosition.Keys.ToArray();
            Array.Sort(items, new SizeComparer());
            
            Clear();
            
            // Debug.Log(items);
            foreach (Item item in items)
            {
                for (int i = 0; i < inventoryItemsPositionCopy.Count; i++)
                {
                    if(FindFreePosition(item.Size, out Vector2Int position))
                        AddItem(item, position);
                }
            }
            
            for (int i = 0; i < inventoryCells.GetLength(0); i++)
            {
                for (int j = 0; j < inventoryCells.GetLength(1); j++)
                {
                    Debug.Log($"Cell ({i}, {j}): {inventoryCells[i,j]}");
                }
            }
            

            // Vector2Int? emptyCell = null;


            // for (int i = 0; i < inventoryCells.GetLength(1); i++)
            // {
            //     for (int j = 0; j < inventoryCells.GetLength(0); j++)
            //     {
            //         if (inventoryCells[i, j] == null)
            //         {
            //             emptyCell = new Vector2Int(i, j);
            //         }
            //         else
            //         {
            //             if (emptyCell != null)
            //             {
            //                 var item = GetItem(i, j);
            //                 MoveItem(item, emptyCell.Value);
            //                 emptyCell = null;
            //             }
            //         }
            //     }
            // }
            // // Debug.Log(_inventoryCells.GetLength(0) * _inventoryCells.GetLength(1));
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            if (matrix == null || inventoryCells == null || matrix.GetLength(0) != inventoryCells.GetLength(0) || matrix.GetLength(1) != inventoryCells.GetLength(1))
                return;

            int rows = inventoryCells.GetLength(0);
            int cols = inventoryCells.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = inventoryCells[i, j];
                }
            }
        }

        public IEnumerator<Item> GetEnumerator() 
            => inventoryItemsPosition.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}