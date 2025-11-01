using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FoxyWebAppManager.Collections
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _suppressNotification = true;

            foreach (var item in items)
                Items.Add(item);

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Clear Collection and add them new Items
        /// </summary>
        /// <param name="items"></param>
        public void ClearAndAdd(IEnumerable<T> items)
        {
            this.Clear();
            this.AddRange(items);
        }

        /// <summary>
        /// async dispatcher operation
        /// </summary>
        /// <param name="items"></param>
        /// <param name="queue"></param>
        public void AddRange(IEnumerable<T> items, DispatcherQueue queue)
          => queue.TryEnqueue(() => this.AddRange(items));

        /// <summary>
        /// async dispatcher operation
        /// </summary>
        /// <param name="items"></param>
        /// <param name="queue"></param>
        public void ClearAndAdd(IEnumerable<T> items, DispatcherQueue queue)
          => queue.TryEnqueue(() => this.ClearAndAdd(items));


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }
    }
}
