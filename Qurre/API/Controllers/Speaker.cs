using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using Footprinting;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Addons;
using Qurre.API.Addons.Audio;
using Qurre.API.Controllers.Components;
using UnityEngine;
using VoiceChat.Playbacks;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Speaker : ToyEntity<SpeakerToy, Speaker>
{
    protected sealed override SpeakerToy UnsafeBase { get; }

    public AudioPlayerSpeaker ApiPlayer { get; }

    public SpeakerToyPlaybackBase Playback => Base.Playback;

    public byte ControllerId
    {
        get => Base.NetworkControllerId;
        set => Base.NetworkControllerId = value;
    }

    public bool IsSpatial
    {
        get => Base.NetworkIsSpatial;
        set => Base.NetworkIsSpatial = value;
    }

    public float Volume
    {
        get => Base.NetworkVolume;
        set => Base.NetworkVolume = value;
    }

    public float MinDistance
    {
        get => Base.NetworkMinDistance;
        set => Base.NetworkMinDistance = value;
    }

    public float MaxDistance
    {
        get => Base.NetworkMaxDistance;
        set => Base.NetworkMaxDistance = value;
    }

    private Speaker(SpeakerToy speakerToy)
    {
        UnsafeBase = speakerToy;
        ApiPlayer = new AudioPlayerSpeaker(speakerToy);

        BaseToWrap[speakerToy] = this;
        AddEntityLink();
    }

    public Speaker(Vector3 position, byte controllerId = 0, bool isSpatial = true,
        float volume = 1.0F, float minDistance = 1.0F, float maxDistance = 15.0F)
    {
        if (Prefabs.Speaker == null)
            throw new NullReferenceException(nameof(Prefabs.Speaker));

        if (!Prefabs.Speaker.TryGetComponent<SpeakerToy>(out SpeakerToy? speakerToy))
            throw new NullReferenceException("SpeakerToy not found");

        UnsafeBase = speakerToy;
        Base.SpawnerFootprint = new Footprint(Server.Host.ReferenceHub);
        NetworkServer.Spawn(Base.gameObject);

        ApiPlayer = new AudioPlayerSpeaker(speakerToy);
        WorldPosition = position;

        ControllerId = controllerId;
        IsSpatial = isSpatial;
        Volume = volume;
        MinDistance = minDistance;
        MaxDistance = maxDistance;
        
        BaseToWrap[speakerToy] = this;
        AddEntityLink();
    }
    
    public static Speaker? Get(SpeakerToy speakerBase)
    {
        if (!speakerBase) return null;
        return BaseToWrap.TryGetValue(speakerBase, out var speaker) ? speaker : new Speaker(speakerBase);
    }

    public static bool TryGet(SpeakerToy speakerBase, [NotNullWhen(true)] out Speaker? speaker)
    {
        speaker = Get(speakerBase);
        return speaker is not null;
    }
}