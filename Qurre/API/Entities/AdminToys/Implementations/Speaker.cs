using Qurre.API.Addons.Audio;
using Qurre.API.Core;
using Qurre.Internal.Attributes;
using VoiceChat.Playbacks;
using SpeakerBase = AdminToys.SpeakerToy;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(SpeakerBase))]
internal sealed class Speaker(SpeakerBase speakerBase) : AdminToy(speakerBase), ISpeaker
{
    /// <inheritdoc />
    public new UnityObjectWrapper<SpeakerBase> Base { get; } = speakerBase;

    /// <inheritdoc />
    public AudioPlayerSpeaker AudioAPI { get; } = new(speakerBase);

    /// <inheritdoc />
    public SpeakerToyPlaybackBase Playback => Base.Instance.Playback;

    /// <inheritdoc />
    public byte ControllerId
    {
        get => Base.Instance.NetworkControllerId;
        set => Base.Instance.NetworkControllerId = value;
    }

    /// <inheritdoc />
    public bool IsSpatial
    {
        get => Base.Instance.NetworkIsSpatial;
        set => Base.Instance.NetworkIsSpatial = value;
    }

    /// <inheritdoc />
    public float Volume
    {
        get => Base.Instance.NetworkVolume;
        set => Base.Instance.NetworkVolume = value;
    }

    /// <inheritdoc />
    public float MinDistance
    {
        get => Base.Instance.NetworkMinDistance;
        set => Base.Instance.NetworkMinDistance = value;
    }

    /// <inheritdoc />
    public float MaxDistance
    {
        get => Base.Instance.NetworkMaxDistance;
        set => Base.Instance.NetworkMaxDistance = value;
    }
}