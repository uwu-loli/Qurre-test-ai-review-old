using System;
using JetBrains.Annotations;
using Mirror;
using VoiceChat.Networking;
using SpeakerBase = AdminToys.SpeakerToy;

namespace Qurre.API.Addons.Audio;

[PublicAPI]
public class AudioPlayerSpeaker : BaseAudioPlayer
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AudioPlayerSpeaker" /> class.
    /// </summary>
    /// <param name="speakerBase"><see cref="SpeakerBase" /> of the audio source.</param>
    /// <exception cref="ArgumentNullException" />
    public AudioPlayerSpeaker(SpeakerBase speakerBase)
    {
        SpeakerToy = speakerBase ?? throw new ArgumentNullException(nameof(speakerBase));
    }

    public SpeakerBase SpeakerToy { get; }

    public override void DestroySelf()
    {
        base.KillCoroutine();

        try
        {
            NetworkServer.Destroy(SpeakerToy.gameObject);
        }
        catch
        {
            Log.Debug("Can not destroy audio player");
        }
    }

    protected override ArraySegment<byte> SerializeAndPackToDataSegment(int dataLength, byte[] dataBuffer,
        int channelId = 0)
    {
        using var networkWriter = NetworkWriterPool.Get();
        var message = new AudioMessage(
            SpeakerToy.ControllerId,
            dataBuffer,
            dataLength
        );

        NetworkMessages.Pack(message, networkWriter);
        var maxMessageSize = NetworkMessages.MaxMessageSize(channelId);
        return networkWriter.Position > maxMessageSize ? ArraySegment<byte>.Empty : networkWriter.ToArraySegment();
    }
}