﻿using System;
using System.Diagnostics;

namespace Mockingjay.Common.Json
{
    internal partial class ConventionBasedSerializer<TSvo>
    {
        /// <summary>Creates a new instance of a <see cref="ConventionBasedSerializer{TSvo}"/>.</summary>
        public ConventionBasedSerializer() => Initialize();

        /// <summary>Returns true if <typeparamref name="TSvo"/> is supported.</summary>
        public bool TypeIsSupported => fromJsonString != null;

        /// <summary>Serializes the <paramref name="svo"/> to a JSON node.</summary>
        /// <param name="svo">
        /// The object to serialize.
        /// </param>
        /// <returns>
        /// The serialized JSON node.
        /// </returns>
        public object ToJson(TSvo svo) => toJsonObject(svo);

        /// <summary>Deserializes the JSON string.</summary>
        /// <param name="json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The actual instance of <typeparamref name="TSvo"/>.
        /// </returns>
        public TSvo FromJson(string json) => fromJsonString(json);

        /// <summary>Deserializes the JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The actual instance of <typeparamref name="TSvo"/>.
        /// </returns>
        public TSvo FromJson(double json) => fromJsonDouble(json);

        /// <summary>Deserializes the JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The actual instance of <typeparamref name="TSvo"/>.
        /// </returns>
        public TSvo FromJson(long json) => fromJsonLong(json);

        /// <summary>Deserializes the JSON boolean.</summary>
        /// <param name="json">
        /// The number boolean to deserialize.
        /// </param>
        /// <returns>
        /// The actual instance of <typeparamref name="TSvo"/>.
        /// </returns>
        public TSvo FromJson(bool json) => fromJsonBool(json);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<string, TSvo> fromJsonString;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<double, TSvo> fromJsonDouble;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<long, TSvo> fromJsonLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<bool, TSvo> fromJsonBool;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<TSvo, object> toJsonObject;
    }
}
