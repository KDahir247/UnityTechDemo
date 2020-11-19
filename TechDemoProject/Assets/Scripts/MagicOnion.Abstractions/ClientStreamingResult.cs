using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Grpc.Core;
using MessagePack;

namespace MagicOnion
{
    /// <summary>
    ///     Wrapped AsyncClientStreamingCall.
    /// </summary>
    public struct ClientStreamingResult<TRequest, TResponse> : IDisposable
    {
        internal readonly TResponse rawValue;
        internal readonly bool hasRawValue;
        private readonly AsyncClientStreamingCall<byte[], byte[]> inner;
        private readonly MessagePackSerializerOptions serializerOptions;

        public ClientStreamingResult(TResponse rawValue)
        {
            hasRawValue = true;
            this.rawValue = rawValue;
            inner = null;
            RequestStream = null;
            serializerOptions = null;
        }

        public ClientStreamingResult(AsyncClientStreamingCall<byte[], byte[]> inner,
            IClientStreamWriter<TRequest> requestStream, MessagePackSerializerOptions serializerOptions)
        {
            hasRawValue = false;
            rawValue = default;
            this.inner = inner;
            RequestStream = requestStream;
            this.serializerOptions = serializerOptions;
        }

        private async Task<TResponse> Deserialize()
        {
            var bytes = await inner.ResponseAsync.ConfigureAwait(false);
            return MessagePackSerializer.Deserialize<TResponse>(bytes, serializerOptions);
        }

        /// <summary>
        ///     Asynchronous call result.
        /// </summary>
        public Task<TResponse> ResponseAsync
        {
            get
            {
                if (hasRawValue)
                    return Task.FromResult(rawValue);
                return Deserialize();
            }
        }

        /// <summary>
        ///     Asynchronous access to response headers.
        /// </summary>
        public Task<Metadata> ResponseHeadersAsync => inner.ResponseHeadersAsync;

        /// <summary>
        ///     Async stream to send streaming requests.
        /// </summary>
        public IClientStreamWriter<TRequest> RequestStream { get; }

        /// <summary>
        ///     Allows awaiting this object directly.
        /// </summary>
        /// <returns></returns>
        public TaskAwaiter<TResponse> GetAwaiter()
        {
            return ResponseAsync.GetAwaiter();
        }

        /// <summary>
        ///     Gets the call status if the call has already finished.
        ///     Throws InvalidOperationException otherwise.
        /// </summary>
        public Status GetStatus()
        {
            return inner.GetStatus();
        }

        /// <summary>
        ///     Gets the call trailing metadata if the call has already finished.
        ///     Throws InvalidOperationException otherwise.
        /// </summary>
        public Metadata GetTrailers()
        {
            return inner.GetTrailers();
        }

        /// <summary>
        ///     Provides means to cleanup after the call.
        ///     If the call has already finished normally (request stream has been completed and call result has been received),
        ///     doesn't do anything.
        ///     Otherwise, requests cancellation of the call which should terminate all pending async operations associated with
        ///     the call.
        ///     As a result, all resources being used by the call should be released eventually.
        /// </summary>
        /// <remarks>
        ///     Normally, there is no need for you to dispose the call unless you want to utilize the
        ///     "Cancel" semantics of invoking <c>Dispose</c>.
        /// </remarks>
        public void Dispose()
        {
            if (inner != null) inner.Dispose();
        }
    }
}