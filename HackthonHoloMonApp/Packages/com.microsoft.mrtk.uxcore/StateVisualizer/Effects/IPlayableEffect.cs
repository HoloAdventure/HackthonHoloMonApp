// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License

using UnityEngine.Playables;

namespace Microsoft.MixedReality.Toolkit.UX
{
    /// <summary>
    /// An <see cref="IEffect"/> that is backed by a <see cref="Playable"/>.
    /// </summary>
    public interface IPlayableEffect : IEffect
    {
        /// <summary>
        /// The non-serialized runtime Playable generated by this <see cref="IEffect"/>.
        /// </summary>
        /// <remarks>
        /// Check <see cref="Playable.IsValid"/> before using this property.
        /// </remarks>
        Playable Playable { get; }
    }
}
