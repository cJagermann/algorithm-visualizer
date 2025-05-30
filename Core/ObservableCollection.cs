﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AlgorithmVisualizer.Core;

public interface IDataFlowObservableCollection<T> {
    /// <summary>
    /// Called when a new item is added
    /// </summary>
    /// <param name="added">The callback to execute when an item is added</param>
    /// <returns>Returns self</returns>
    IDataFlowObservableCollection<T> WhenAdded(Action<T> added);

    /// <summary>
    /// Called when an existing item is removed
    /// Note: It is not called when items are cleared if <see cref="WhenCleared(Action{IList{T}})"/> is used
    /// </summary>
    /// <param name="added">The callback to execute when an item is removed</param>
    /// <returns>Returns self</returns>
    IDataFlowObservableCollection<T> WhenRemoved(Action<T> removed);

    /// <summary>
    /// Called when the collection is cleared
    /// NOTE: It does not call <see cref="WhenRemoved(Action{T})"/> on each item
    /// </summary>
    /// <param name="added">The callback to execute when the collection is cleared</param>
    /// <returns>Returns self</returns>
    IDataFlowObservableCollection<T> WhenCleared(Action<IList<T>> cleared);
}

public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged,
    IDataFlowObservableCollection<T> {
    protected static readonly PropertyChangedEventArgs IndexerPropertyChanged = new("Item[]");
    protected static readonly PropertyChangedEventArgs CountPropertyChanged = new("Count");

    protected static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged =
        new(NotifyCollectionChangedAction.Reset);

    private readonly List<Action<T>> _added = new();
    private readonly List<Action<T>> _removed = new();
    private readonly List<Action<IList<T>>> _cleared = new();

    public new event NotifyCollectionChangedEventHandler? CollectionChanged;
    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection() { }

    public ObservableCollection(IEnumerable<T> collection)
        : base(new List<T>(collection)) { }

    #region Collection Events

    public IDataFlowObservableCollection<T> WhenAdded(Action<T> added) {
        if (added != null) _added.Add(added);

        return this;
    }

    public IDataFlowObservableCollection<T> WhenRemoved(Action<T> removed) {
        if (removed != null) _removed.Add(removed);

        return this;
    }

    public IDataFlowObservableCollection<T> WhenCleared(Action<IList<T>> cleared) {
        if (cleared != null) _cleared.Add(cleared);

        return this;
    }

    protected virtual void NotifyOnItemAdded(T item) {
        for (var i = 0; i < _added.Count; i++) _added[i](item);
    }

    protected virtual void NotifyOnItemRemoved(T item) {
        for (var i = 0; i < _removed.Count; i++) _removed[i](item);
    }

    protected virtual void NotifyOnItemsCleared(IList<T> items) {
        for (var i = 0; i < _cleared.Count; i++) _cleared[i](items);
    }

    #endregion

    #region Collection Handlers

    protected override void ClearItems() {
        var items = new List<T>(this);
        base.ClearItems();

        if (_cleared.Count > 0)
            NotifyOnItemsCleared(items);
        else
            for (var i = 0; i < items.Count; i++)
                NotifyOnItemRemoved(items[i]);

        OnPropertyChanged(CountPropertyChanged);
        OnPropertyChanged(IndexerPropertyChanged);
        OnCollectionChanged(ResetCollectionChanged);
    }

    protected override void InsertItem(int index, T item) {
        base.InsertItem(index, item);

        OnPropertyChanged(CountPropertyChanged);
        OnPropertyChanged(IndexerPropertyChanged);
        OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        NotifyOnItemAdded(item);
    }

    protected override void RemoveItem(int index) {
        var item = base[index];
        base.RemoveItem(index);

        OnPropertyChanged(CountPropertyChanged);
        OnPropertyChanged(IndexerPropertyChanged);
        OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        NotifyOnItemRemoved(item);
    }

    protected override void SetItem(int index, T item) {
        var prev = base[index];
        base.SetItem(index, item);
        OnPropertyChanged(IndexerPropertyChanged);
        OnCollectionChanged(NotifyCollectionChangedAction.Replace, prev, item, index);
        NotifyOnItemRemoved(prev);
        NotifyOnItemAdded(item);
    }

    public void Move(int oldIndex, int newIndex) {
        var prev = base[oldIndex];
        base.RemoveItem(oldIndex);
        base.InsertItem(newIndex, prev);
        OnPropertyChanged(IndexerPropertyChanged);
        OnCollectionChanged(NotifyCollectionChangedAction.Move, prev, newIndex, oldIndex);
    }
    
    public void Swap(int firstIdx, int secondIdx) {
        var first = base[firstIdx];
            
        base.SetItem(firstIdx, base[secondIdx]);
        OnCollectionChanged(NotifyCollectionChangedAction.Move, base[firstIdx], firstIdx, secondIdx);
        OnPropertyChanged(IndexerPropertyChanged);
        
        base.SetItem(secondIdx, first);
        OnCollectionChanged(NotifyCollectionChangedAction.Move, base[secondIdx], secondIdx, firstIdx);
        OnPropertyChanged(IndexerPropertyChanged);
    }

    protected new virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
        CollectionChanged?.Invoke(this, e);
    }

    protected new virtual void OnPropertyChanged(PropertyChangedEventArgs args) {
        PropertyChanged?.Invoke(this, args);
    }
    
    private void OnCollectionChanged(NotifyCollectionChangedAction action) {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index) {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index, int oldIndex) {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? oldItem, object? newItem,
        int index) {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
    }

    #endregion
}