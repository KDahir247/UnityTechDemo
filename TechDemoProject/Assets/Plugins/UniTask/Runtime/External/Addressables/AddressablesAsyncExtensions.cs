// asmdef Version Defines, enabled when com.unity.addressables is imported.

#if UNITASK_ADDRESSABLE_SUPPORT

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Cysharp.Threading.Tasks
{
    public static class AddressablesAsyncExtensions
    {
        #region AsyncOperationHandle

        public static UniTask.Awaiter GetAwaiter(this AsyncOperationHandle handle)
        {
            return ToUniTask(handle).GetAwaiter();
        }

        public static UniTask WithCancellation(this AsyncOperationHandle handle, CancellationToken cancellationToken)
        {
            return ToUniTask(handle, cancellationToken: cancellationToken);
        }

        public static UniTask ToUniTask(this AsyncOperationHandle handle, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return UniTask.FromCanceled(cancellationToken);

            if (!handle.IsValid())
                // autoReleaseHandle:true handle is invalid(immediately internal handle == null) so return completed.
                return UniTask.CompletedTask;

            if (handle.IsDone)
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                    return UniTask.FromException(handle.OperationException);
                return UniTask.CompletedTask;
            }

            return new UniTask(
                AsyncOperationHandleConfiguredSource.Create(handle, timing, progress, cancellationToken, out var token),
                token);
        }

        public struct AsyncOperationHandleAwaiter : ICriticalNotifyCompletion
        {
            private AsyncOperationHandle handle;
            private Action<AsyncOperationHandle> continuationAction;

            public AsyncOperationHandleAwaiter(AsyncOperationHandle handle)
            {
                this.handle = handle;
                continuationAction = null;
            }

            public bool IsCompleted => handle.IsDone;

            public void GetResult()
            {
                if (continuationAction != null)
                {
                    handle.Completed -= continuationAction;
                    continuationAction = null;
                }

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    var e = handle.OperationException;
                    handle = default;
                    ExceptionDispatchInfo.Capture(e).Throw();
                }

                var result = handle.Result;
                handle = default;
            }

            public void OnCompleted(Action continuation)
            {
                UnsafeOnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                Error.ThrowWhenContinuationIsAlreadyRegistered(continuationAction);
                continuationAction = PooledDelegate<AsyncOperationHandle>.Create(continuation);
                handle.Completed += continuationAction;
            }
        }

        private sealed class AsyncOperationHandleConfiguredSource : IUniTaskSource, IPlayerLoopItem,
            ITaskPoolNode<AsyncOperationHandleConfiguredSource>
        {
            private static TaskPool<AsyncOperationHandleConfiguredSource> pool;

            private readonly Action<AsyncOperationHandle> continuationAction;
            private CancellationToken cancellationToken;
            private bool completed;

            private UniTaskCompletionSourceCore<AsyncUnit> core;
            private AsyncOperationHandle handle;
            private AsyncOperationHandleConfiguredSource nextNode;
            private IProgress<float> progress;

            static AsyncOperationHandleConfiguredSource()
            {
                TaskPool.RegisterSizeGetter(typeof(AsyncOperationHandleConfiguredSource), () => pool.Size);
            }

            private AsyncOperationHandleConfiguredSource()
            {
                continuationAction = Continuation;
            }

            public bool MoveNext()
            {
                if (completed)
                {
                    TryReturn();
                    return false;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    completed = true;
                    core.TrySetCanceled(cancellationToken);
                    return false;
                }

                if (progress != null && handle.IsValid()) progress.Report(handle.PercentComplete);

                return true;
            }

            public ref AsyncOperationHandleConfiguredSource NextNode => ref nextNode;

            public void GetResult(short token)
            {
                core.GetResult(token);
            }

            public UniTaskStatus GetStatus(short token)
            {
                return core.GetStatus(token);
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return core.UnsafeGetStatus();
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                core.OnCompleted(continuation, state, token);
            }

            public static IUniTaskSource Create(AsyncOperationHandle handle, PlayerLoopTiming timing,
                IProgress<float> progress, CancellationToken cancellationToken, out short token)
            {
                if (cancellationToken.IsCancellationRequested)
                    return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);

                if (!pool.TryPop(out var result)) result = new AsyncOperationHandleConfiguredSource();

                result.handle = handle;
                result.progress = progress;
                result.cancellationToken = cancellationToken;
                result.completed = false;

                TaskTracker.TrackActiveTask(result, 3);

                PlayerLoopHelper.AddAction(timing, result);

                handle.Completed += result.continuationAction;

                token = result.core.Version;
                return result;
            }

            private void Continuation(AsyncOperationHandle _)
            {
                handle.Completed -= continuationAction;

                if (completed)
                {
                    TryReturn();
                }
                else
                {
                    completed = true;
                    if (cancellationToken.IsCancellationRequested)
                        core.TrySetCanceled(cancellationToken);
                    else if (handle.Status == AsyncOperationStatus.Failed)
                        core.TrySetException(handle.OperationException);
                    else
                        core.TrySetResult(AsyncUnit.Default);
                }
            }

            private bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);
                core.Reset();
                handle = default;
                progress = default;
                cancellationToken = default;
                return pool.TryPush(this);
            }
        }

        #endregion

        #region AsyncOperationHandle_T

        public static UniTask<T>.Awaiter GetAwaiter<T>(this AsyncOperationHandle<T> handle)
        {
            return ToUniTask(handle).GetAwaiter();
        }

        public static UniTask<T> WithCancellation<T>(this AsyncOperationHandle<T> handle,
            CancellationToken cancellationToken)
        {
            return ToUniTask(handle, cancellationToken: cancellationToken);
        }

        public static UniTask<T> ToUniTask<T>(this AsyncOperationHandle<T> handle, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return UniTask.FromCanceled<T>(cancellationToken);

            if (!handle.IsValid()) throw new Exception("Attempting to use an invalid operation handle");

            if (handle.IsDone)
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                    return UniTask.FromException<T>(handle.OperationException);
                return UniTask.FromResult(handle.Result);
            }

            return new UniTask<T>(
                AsyncOperationHandleConfiguredSource<T>.Create(handle, timing, progress, cancellationToken,
                    out var token), token);
        }

        private sealed class AsyncOperationHandleConfiguredSource<T> : IUniTaskSource<T>, IPlayerLoopItem,
            ITaskPoolNode<AsyncOperationHandleConfiguredSource<T>>
        {
            private static TaskPool<AsyncOperationHandleConfiguredSource<T>> pool;

            private readonly Action<AsyncOperationHandle<T>> continuationAction;
            private CancellationToken cancellationToken;
            private bool completed;

            private UniTaskCompletionSourceCore<T> core;
            private AsyncOperationHandle<T> handle;
            private AsyncOperationHandleConfiguredSource<T> nextNode;
            private IProgress<float> progress;

            static AsyncOperationHandleConfiguredSource()
            {
                TaskPool.RegisterSizeGetter(typeof(AsyncOperationHandleConfiguredSource<T>), () => pool.Size);
            }

            private AsyncOperationHandleConfiguredSource()
            {
                continuationAction = Continuation;
            }

            public bool MoveNext()
            {
                if (completed)
                {
                    TryReturn();
                    return false;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    completed = true;
                    core.TrySetCanceled(cancellationToken);
                    return false;
                }

                if (progress != null && handle.IsValid()) progress.Report(handle.PercentComplete);

                return true;
            }

            public ref AsyncOperationHandleConfiguredSource<T> NextNode => ref nextNode;

            public T GetResult(short token)
            {
                return core.GetResult(token);
            }

            void IUniTaskSource.GetResult(short token)
            {
                GetResult(token);
            }

            public UniTaskStatus GetStatus(short token)
            {
                return core.GetStatus(token);
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return core.UnsafeGetStatus();
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                core.OnCompleted(continuation, state, token);
            }

            public static IUniTaskSource<T> Create(AsyncOperationHandle<T> handle, PlayerLoopTiming timing,
                IProgress<float> progress, CancellationToken cancellationToken, out short token)
            {
                if (cancellationToken.IsCancellationRequested)
                    return AutoResetUniTaskCompletionSource<T>.CreateFromCanceled(cancellationToken, out token);

                if (!pool.TryPop(out var result)) result = new AsyncOperationHandleConfiguredSource<T>();

                result.handle = handle;
                result.cancellationToken = cancellationToken;
                result.completed = false;
                result.progress = progress;

                TaskTracker.TrackActiveTask(result, 3);

                PlayerLoopHelper.AddAction(timing, result);

                handle.Completed += result.continuationAction;

                token = result.core.Version;
                return result;
            }

            private void Continuation(AsyncOperationHandle<T> argHandle)
            {
                handle.Completed -= continuationAction;

                if (completed)
                {
                    TryReturn();
                }
                else
                {
                    completed = true;
                    if (cancellationToken.IsCancellationRequested)
                        core.TrySetCanceled(cancellationToken);
                    else if (argHandle.Status == AsyncOperationStatus.Failed)
                        core.TrySetException(argHandle.OperationException);
                    else
                        core.TrySetResult(argHandle.Result);
                }
            }

            private bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);
                core.Reset();
                handle = default;
                progress = default;
                cancellationToken = default;
                return pool.TryPush(this);
            }
        }

        #endregion
    }
}

#endif