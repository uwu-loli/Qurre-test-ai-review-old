using JetBrains.Annotations;
using Qurre.API.Addons.Audio;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using VoiceChat.Playbacks;
using SpeakerBase = AdminToys.SpeakerToy;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public interface ISpeaker : IAdminToy
{
    #region Properties

    new UnityObjectWrapper<SpeakerBase> Base { get; }

    AudioPlayerSpeaker AudioAPI { get; }

    /// <exception cref="ObjectDestroyedException" />
    SpeakerToyPlaybackBase Playback { get; }

    /// <exception cref="ObjectDestroyedException" />
    byte ControllerId { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsSpatial { get; set; }

    /// <summary>
    ///     От 0 до 1
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    float Volume { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float MinDistance { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float MaxDistance { get; set; }

    #endregion
}