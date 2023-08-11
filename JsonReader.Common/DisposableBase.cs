namespace JsonReader.Common
{
    public abstract class DisposableBase : IDisposable
    {
        protected readonly object _syncRoot = new();
        private bool _disposing = false;
        private bool _isDisposed = false;

        /// <summary>
        /// A finalizer which calls Dispose (as per Dispose pattern)
        /// </summary>
        ~DisposableBase()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// Gets a flag indicating whether this object was already disposed
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (_syncRoot)
                {
                    return _isDisposed;
                }
            }
            private set
            {
                lock (_syncRoot)
                {
                    _isDisposed = value;
                }
            }
        }

        /// <summary>
        /// Disposes current object's resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles the disposal logic
        /// </summary>
        /// <param name="disposing">A flag to indicate if we need to dispose managed state</param>
        protected void Dispose(bool disposing)
        {
            // NOTE: this happens if object
            // was collected already
            if (_syncRoot == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (!_isDisposed && !_disposing)
                {
                    _disposing = true;

                    try
                    {
                        if (disposing)
                        {
                            DisposeManagedResources();
                        }

                        DisposeUnmanagedResources();
                    }
                    finally
                    {
                        IsDisposed = true;
                        _disposing = false;
                    }
                }
            }
        }

        /// <summary>
        /// Disposes object's managed resources
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Disposes object's unmanaged resources
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}